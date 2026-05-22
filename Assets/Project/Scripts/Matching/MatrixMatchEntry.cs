using System.Collections;
using System;
using UnityEngine;
using VContainer;

public class MatrixMatchEntry : MonoBehaviour
{
    MatchContextFactory _contextFactory;
    bool _isRunning;
    bool _cancelRequested;
    Coroutine _runCoroutine;
    MatchContext _currentContext;
    Action _onCompleted;

    [Inject]
    public void Construct(MatchContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public void Run(
        MatchRunOptions options,
        Action onCompleted,
        Action<float> onProgressChanged = null,
        Action<string> onLog = null)
    {
        if (_isRunning)
            return;

        _isRunning = true;
        _cancelRequested = false;
        _onCompleted = onCompleted;
        var context = _contextFactory.Create(options);
        _currentContext = context;

        if (options.EnableVisualization)
            _runCoroutine = StartCoroutine(RunStepByStep(context, options, onProgressChanged, onLog));
        else
            RunInstant(context, options, onProgressChanged, onLog);
    }

    public void Stop()
    {
        if (!_isRunning)
            return;

        _cancelRequested = true;

        if (_runCoroutine != null)
        {
            StopCoroutine(_runCoroutine);
            _runCoroutine = null;
        }

        CompleteRun();
    }

    void RunInstant(
        MatchContext context,
        MatchRunOptions options,
        Action<float> onProgressChanged,
        Action<string> onLog)
    {
        var model = MatrixJsonLoader.Load("Data/model");
        var space = MatrixJsonLoader.Load("Data/space");

        var acceptedCount = 0;

        if (options.EnableLogging)
        {
            foreach (var step in context.Matcher.FindSteps(model, space))
            {
                onProgressChanged?.Invoke(GetStepProgress(step, space.Length));

                if (TrackStepLog(step, true, onLog))
                    acceptedCount++;

                if (_cancelRequested)
                    break;
            }
        }
        else
        {
            acceptedCount = context.Matcher.Find(model, space).Offsets.Count;
        }

        onProgressChanged?.Invoke(1f);
        Debug.Log($"{context.Matcher.Name}: смещений найдено — {acceptedCount}");
        CompleteRun();
    }

    IEnumerator RunStepByStep(
        MatchContext context,
        MatchRunOptions options,
        Action<float> onProgressChanged,
        Action<string> onLog)
    {
        var model = MatrixJsonLoader.Load("Data/model");
        var space = MatrixJsonLoader.Load("Data/space");

        var acceptedCount = 0;

        if (context.StepPlayback != null)
        {
            yield return context.StepPlayback.PlayFindSteps(model, space, step =>
            {
                onProgressChanged?.Invoke(GetStepProgress(step, space.Length));

                if (TrackStepLog(step, options.EnableLogging, onLog))
                    acceptedCount++;
            }, () => _cancelRequested);
        }
        else
        {
            foreach (var step in context.Matcher.FindSteps(model, space))
            {
                onProgressChanged?.Invoke(GetStepProgress(step, space.Length));

                if (TrackStepLog(step, options.EnableLogging, onLog))
                    acceptedCount++;

                if (_cancelRequested)
                    break;
            }
        }

        _runCoroutine = null;
        onProgressChanged?.Invoke(_cancelRequested ? 0f : 1f);
        Debug.Log($"{context.Matcher.Name}: пошаговый режим завершен, смещений найдено — {acceptedCount}");
        CompleteRun();
    }

    void CompleteRun()
    {
        _isRunning = false;
        _runCoroutine = null;
        _currentContext?.Cleanup?.Invoke();
        _currentContext = null;
        _onCompleted?.Invoke();
        _onCompleted = null;
    }

    static bool TrackStepLog(MatchStep step, bool enableLogging, Action<string> onLog)
    {
        var message = $"space[{step.SpaceIndex}]: {MatchStepColors.GetLabel(step.Kind)}";

        if (enableLogging)
        {
            Debug.Log(message);
            onLog?.Invoke(message);
        }

        return step.Kind == MatchStepKind.Accepted;
    }

    static float GetStepProgress(MatchStep step, int spaceCount)
    {
        if (spaceCount <= 0)
            return 1f;

        return Mathf.Clamp01((step.SpaceIndex + 1f) / spaceCount);
    }
}
