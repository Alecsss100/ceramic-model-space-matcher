using System.Collections;
using UnityEngine;
using VContainer;

public class MatrixMatchEntry : MonoBehaviour
{
    IModelSpaceMatcher _matcher;
    IMatchStepPlayback _stepPlayback;
    MatchRunOptions _options;

    [Inject]
    public void Construct(MatchContext context, MatchRunOptions options)
    {
        _matcher = context.Matcher;
        _stepPlayback = context.StepPlayback;
        _options = options;
    }

    public void Run()
    {
        if (_options.RunStepByStep)
            StartCoroutine(RunStepByStep());
        else
            RunInstant();
    }

    void RunInstant()
    {
        var model = MatrixJsonLoader.Load("Data/model");
        var space = MatrixJsonLoader.Load("Data/space");

        Debug.Log($"{_matcher.Name}: смещений найдено — {_matcher.Find(model, space).Offsets.Count}");
    }

    IEnumerator RunStepByStep()
    {
        var model = MatrixJsonLoader.Load("Data/model");
        var space = MatrixJsonLoader.Load("Data/space");

        var acceptedCount = 0;

        if (_stepPlayback != null)
            yield return _stepPlayback.PlayFindSteps(model, space, step => TrackStepLog(step, ref acceptedCount));
        else
        {
            foreach (var step in _matcher.FindSteps(model, space))
                TrackStepLog(step, ref acceptedCount);
        }

        Debug.Log($"{_matcher.Name}: пошаговый режим завершен, смещений найдено — {acceptedCount}");
    }

    static void TrackStepLog(MatchStep step, ref int acceptedCount)
    {
        Debug.Log($"space[{step.SpaceIndex}]: {MatchStepColors.GetLabel(step.Kind)}");

        if (step.Kind == MatchStepKind.Accepted)
            acceptedCount++;
    }
}
