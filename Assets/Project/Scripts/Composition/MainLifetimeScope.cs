using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainLifetimeScope : LifetimeScope
{
    [SerializeField] MatchControlView _matchControlView;
    [SerializeField] MatchProgressView _matchProgressView;
    [SerializeField] MatchLogView _matchLogView;

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
        builder.Register<MatchAlgorithmRegistry>(Lifetime.Singleton);
        builder.Register<VisualizationTransformRegistry>(Lifetime.Singleton);
        builder.Register<VisualizationModeController>(Lifetime.Singleton);
        builder.Register<IVisualizationProjection>(
            resolver => resolver.Resolve<VisualizationModeController>(),
            Lifetime.Singleton);
        builder.Register<IMatrixVisualizer>(
            resolver => new CubeMatrixVisualizer(
                resolver.Resolve<IVisualizationProjection>(),
                resolver.Resolve<VisualizationTransformRegistry>()),
            Lifetime.Singleton);
        builder.Register(
            resolver => new MatchContextFactory(
                resolver.Resolve<MatchAlgorithmRegistry>(),
                _visualizationRoot,
                resolver.Resolve<IVisualizationProjection>(),
                resolver.Resolve<VisualizationTransformRegistry>()),
            Lifetime.Singleton);

        builder.RegisterComponentOnNewGameObject<MatrixMatchEntry>(
                Lifetime.Singleton,
                nameof(MatrixMatchEntry))
            .UnderTransform(transform);

        builder.RegisterComponent(_matchControlView);
        builder.RegisterComponent(_matchProgressView);
        builder.RegisterComponent(_matchLogView);
        builder.RegisterComponentInHierarchy<MatrixVisualizationEntry>();

        builder.RegisterEntryPoint<MatchControlPresenter>();
    }
}
