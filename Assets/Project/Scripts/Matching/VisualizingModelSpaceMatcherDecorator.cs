using System.Collections.Generic;
using UnityEngine;

public class VisualizingModelSpaceMatcherDecorator : IModelSpaceMatcher
{
    readonly IModelSpaceMatcher _inner;
    readonly IMatchStepVisualizer _visualizer;

    public VisualizingModelSpaceMatcherDecorator(IModelSpaceMatcher inner, IMatchStepVisualizer visualizer)
    {
        _inner = inner;
        _visualizer = visualizer;
    }

    public string Name => _inner.Name;

    public MatchResult Find(Matrix4x4[] model, Matrix4x4[] space)
    {
        var result = new MatchResult();

        foreach (var step in FindSteps(model, space))
        {
            if (step.Kind == MatchStepKind.Accepted)
                result.Offsets.Add(step.Offset);
        }

        return result;
    }

    public IEnumerable<MatchStep> FindSteps(Matrix4x4[] model, Matrix4x4[] space)
    {
        try
        {
            foreach (var step in _inner.FindSteps(model, space))
            {
                _visualizer.ShowStep(step, model, space);
                yield return step;
            }
        }
        finally
        {
            _visualizer.Clear();
        }
    }
}
