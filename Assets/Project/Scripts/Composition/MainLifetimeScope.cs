using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainLifetimeScope : LifetimeScope
{
    [SerializeField] bool _runStepByStep = true;
    [SerializeField] bool _enableVisualization = true;

    Transform _visualizationRoot;

    protected override void Awake()
    {
        var visualizationObject = new GameObject("MatchVisualization");
        visualizationObject.transform.SetParent(transform, false);
        _visualizationRoot = visualizationObject.transform;

        base.Awake();
    }

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(new MatchRunOptions(_runStepByStep, _enableVisualization));

        builder.Register(resolver =>
        {
            var options = resolver.Resolve<MatchRunOptions>();
            var inner = new OffsetModelSpaceMatcher();

            if (options.EnableVisualization && options.RunStepByStep)
            {
                var visualizer = new MatchStepCubeVisualizer(_visualizationRoot, modelCount: 100);
                var timed = new TimedModelSpaceMatcherDecorator(
                    new VisualizingModelSpaceMatcherDecorator(inner, visualizer),
                    new MatchStepTiming());
                return new MatchContext(timed, timed);
            }

            return new MatchContext(inner);
        }, Lifetime.Singleton);

        builder.RegisterComponentOnNewGameObject<MatrixMatchEntry>(
                Lifetime.Singleton,
                nameof(MatrixMatchEntry))
            .UnderTransform(transform);
        builder.RegisterEntryPoint<MatrixMatchEntryPoint>();
        Debug.Log("MatrixMatchEntry registered");
    }
        
}
