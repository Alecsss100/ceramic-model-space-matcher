using UnityEngine;

public sealed class MatchContextFactory
{
    readonly MatchAlgorithmRegistry _algorithmRegistry;
    readonly Transform _visualizationRoot;
    readonly IVisualizationProjection _projection;
    readonly VisualizationTransformRegistry _transformRegistry;

    public MatchContextFactory(
        MatchAlgorithmRegistry algorithmRegistry,
        Transform visualizationRoot,
        IVisualizationProjection projection,
        VisualizationTransformRegistry transformRegistry)
    {
        _algorithmRegistry = algorithmRegistry;
        _visualizationRoot = visualizationRoot;
        _projection = projection;
        _transformRegistry = transformRegistry;
    }

    public MatchContext Create(MatchRunOptions options)
    {
        var matcher = _algorithmRegistry.CreateMatcher(options.AlgorithmIndex);

        if (!options.EnableVisualization)
            return new MatchContext(matcher);

        var visualizer = new MatchStepCubeVisualizer(
            _visualizationRoot,
            modelCount: 100,
            _projection,
            _transformRegistry);
        var timed = new TimedModelSpaceMatcherDecorator(
            new VisualizingModelSpaceMatcherDecorator(matcher, visualizer),
            new MatchStepTiming());

        return new MatchContext(timed, timed, visualizer.Clear);
    }
}
