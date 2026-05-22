using System.Collections;
using UnityEngine;

public class MatrixMatchEntry : MonoBehaviour
{
    [SerializeField] bool _runStepByStep = true;
    [SerializeField] bool _enableVisualization = true;

    IModelSpaceMatcher _matcher;
    IMatchStepPlayback _stepPlayback;

    void Awake()
    {
        var inner = new BruteForceShiftModelSpaceMatcher();

        if (_enableVisualization && _runStepByStep)
        {
            var timed = new TimedModelSpaceMatcherDecorator(
                new VisualizingModelSpaceMatcherDecorator(inner, new MatchStepCubeVisualizer(transform, 100)),
                new MatchStepTiming());
            _matcher = timed;
            _stepPlayback = timed;
        }
        else
        {
            _matcher = inner;
        }
    }

    void Start()
    {
        if (_runStepByStep)
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
