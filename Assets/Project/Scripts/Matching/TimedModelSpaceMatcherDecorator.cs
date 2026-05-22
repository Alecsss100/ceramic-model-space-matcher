using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TimedModelSpaceMatcherDecorator : IModelSpaceMatcher, IMatchStepPlayback
{
    readonly IModelSpaceMatcher _inner;
    readonly IMatchStepTiming _stepTiming;

    public TimedModelSpaceMatcherDecorator(IModelSpaceMatcher inner, IMatchStepTiming stepTiming = null)
    {
        _inner = inner;
        _stepTiming = stepTiming;
    }

    public string Name => _inner.Name;

    public MatchResult Find(Matrix4x4[] model, Matrix4x4[] space) =>
        _inner.Find(model, space);

    public IEnumerable<MatchStep> FindSteps(Matrix4x4[] model, Matrix4x4[] space)
    {
        foreach (var step in _inner.FindSteps(model, space))
            yield return step;
    }

    public IEnumerator PlayFindSteps(
        Matrix4x4[] model,
        Matrix4x4[] space,
        Action<MatchStep> onStep = null,
        Func<bool> shouldCancel = null)
    {
        foreach (var step in _inner.FindSteps(model, space))
        {
            if (shouldCancel != null && shouldCancel())
                yield break;

            onStep?.Invoke(step);

            if (_stepTiming == null)
                continue;

            var delay = _stepTiming.GetDelay(step.Kind);
            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            if (shouldCancel != null && shouldCancel())
                yield break;
        }
    }
}
