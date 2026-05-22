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
    MatchResult _currentResult;
    int _currentModelCount;
    int _currentSpaceCount;
    Action<MatchRunResult> _onCompleted;

    [Inject]
    public void Construct(MatchContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public void Run(
        MatchRunOptions options,
        Action<MatchRunResult> onCompleted,
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
        _currentResult = new MatchResult();
        _currentModelCount = 0;
        _currentSpaceCount = 0;

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

        CompleteRun(CreateCurrentRunResult());
    }

    void RunInstant(
        MatchContext context,
        MatchRunOptions options,
        Action<float> onProgressChanged,
        Action<string> onLog)
    {
        var model = MatrixJsonLoader.Load("Data/model");
        var space = MatrixJsonLoader.Load("Data/space");
        _currentModelCount = model.Length;
        _currentSpaceCount = space.Length;

        var result = new MatchResult();
        _currentResult = result;

        if (options.EnableLogging)
        {
            foreach (var step in context.Matcher.FindSteps(model, space))
            {
                onProgressChanged?.Invoke(GetStepProgress(step, space.Length));

                if (TrackStepLog(step, true, onLog))
                    result.Offsets.Add(step.Offset);

                if (_cancelRequested)
                    break;
            }
        }
        else
        {
            result = context.Matcher.Find(model, space);
        }

        _currentResult = result;
        onProgressChanged?.Invoke(1f);
        Debug.Log($"{context.Matcher.Name}: смещений найдено — {result.Offsets.Count}");
        CompleteRun(CreateRunResult(context, model, space, result));
    }

    IEnumerator RunStepByStep(
        MatchContext context,
        MatchRunOptions options,
        Action<float> onProgressChanged,
        Action<string> onLog)
    {
        var model = MatrixJsonLoader.Load("Data/model");
        var space = MatrixJsonLoader.Load("Data/space");
        _currentModelCount = model.Length;
        _currentSpaceCount = space.Length;

        var result = new MatchResult();
        _currentResult = result;

        if (context.StepPlayback != null)
        {
            yield return context.StepPlayback.PlayFindSteps(model, space, step =>
            {
                onProgressChanged?.Invoke(GetStepProgress(step, space.Length));

                if (TrackStepLog(step, options.EnableLogging, onLog))
                    result.Offsets.Add(step.Offset);
            }, () => _cancelRequested);
        }
        else
        {
            foreach (var step in context.Matcher.FindSteps(model, space))
            {
                onProgressChanged?.Invoke(GetStepProgress(step, space.Length));

                if (TrackStepLog(step, options.EnableLogging, onLog))
                    result.Offsets.Add(step.Offset);

                if (_cancelRequested)
                    break;
            }
        }

        _currentResult = result;
        _runCoroutine = null;
        onProgressChanged?.Invoke(_cancelRequested ? 0f : 1f);
        Debug.Log($"{context.Matcher.Name}: пошаговый режим завершен, смещений найдено — {result.Offsets.Count}");
        CompleteRun(CreateRunResult(context, model, space, result));
    }

    void CompleteRun(MatchRunResult runResult)
    {
        _isRunning = false;
        _runCoroutine = null;
        _currentContext?.Cleanup?.Invoke();
        _currentContext = null;
        _currentResult = null;
        _currentModelCount = 0;
        _currentSpaceCount = 0;
        _onCompleted?.Invoke(runResult);
        _onCompleted = null;
    }

    MatchRunResult CreateCurrentRunResult()
    {
        return new MatchRunResult(
            _currentContext?.Matcher.Name,
            _currentModelCount,
            _currentSpaceCount,
            _cancelRequested,
            _currentResult ?? new MatchResult());
    }

    MatchRunResult CreateRunResult(
        MatchContext context,
        Matrix4x4[] model,
        Matrix4x4[] space,
        MatchResult result)
    {
        return new MatchRunResult(
            context.Matcher.Name,
            model?.Length ?? 0,
            space?.Length ?? 0,
            _cancelRequested,
            result ?? new MatchResult());
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
