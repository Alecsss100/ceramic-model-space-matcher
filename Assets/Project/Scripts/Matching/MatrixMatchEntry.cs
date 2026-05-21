using System.Collections;
using UnityEngine;

public class MatrixMatchEntry : MonoBehaviour
{
    [SerializeField] float _stepDelay = 0.05f;
    [SerializeField] bool _runStepByStep = true;

    IModelSpaceMatcher _matcher;

    void Awake()
    {
        _matcher = new BruteForceShiftModelSpaceMatcher();
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

        var result = _matcher.Find(model, space);
        Debug.Log($"{_matcher.Name}: смещений найдено — {result.Offsets.Count}");
    }

    IEnumerator RunStepByStep()
    {
        var model = MatrixJsonLoader.Load("Data/model");
        var space = MatrixJsonLoader.Load("Data/space");

        var acceptedCount = 0;

        foreach (var step in _matcher.FindSteps(model, space))
        {
            LogStep(step);

            if (step.Kind == MatchStepKind.Accepted)
                acceptedCount++;

            yield return new WaitForSeconds(_stepDelay);
        }

        Debug.Log($"{_matcher.Name}: пошаговый режим завершен, смещений найдено — {acceptedCount}");
    }

    static void LogStep(MatchStep step)
    {
        var message = step.Kind switch
        {
            MatchStepKind.Candidate => $"space[{step.SpaceIndex}]: проверка кандидата",
            MatchStepKind.SkippedDuplicate => $"space[{step.SpaceIndex}]: дубликат, пропуск",
            MatchStepKind.Rejected => $"space[{step.SpaceIndex}]: не подошло",
            MatchStepKind.Accepted => $"space[{step.SpaceIndex}]: найдено смещение",
            _ => $"space[{step.SpaceIndex}]: {step.Kind}"
        };

        Debug.Log(message);
    }
}
