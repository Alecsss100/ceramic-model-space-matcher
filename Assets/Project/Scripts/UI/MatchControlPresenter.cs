using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

public sealed class MatchControlPresenter : IStartable, IDisposable
{
    readonly MatchControlView _view;
    readonly MatchProgressView _progressView;
    readonly MatchLogView _logView;
    readonly MatchJsonView _jsonView;
    readonly MatrixMatchEntry _entry;
    readonly MatchAlgorithmRegistry _algorithmRegistry;
    readonly VisualizationModeController _visualizationMode;
    readonly MatchResultJsonBuilder _jsonBuilder;
    readonly JsonFileSaver _jsonFileSaver;
    string _lastJson;

    public MatchControlPresenter(
        MatchControlView view,
        MatchProgressView progressView,
        MatchLogView logView,
        MatchJsonView jsonView,
        MatrixMatchEntry entry,
        MatchAlgorithmRegistry algorithmRegistry,
        VisualizationModeController visualizationMode,
        MatchResultJsonBuilder jsonBuilder,
        JsonFileSaver jsonFileSaver)
    {
        _view = view;
        _progressView = progressView;
        _logView = logView;
        _jsonView = jsonView;
        _entry = entry;
        _algorithmRegistry = algorithmRegistry;
        _visualizationMode = visualizationMode;
        _jsonBuilder = jsonBuilder;
        _jsonFileSaver = jsonFileSaver;
    }

    public void Start()
    {
        var algorithmNames = new List<string>();
        foreach (var algorithm in _algorithmRegistry.Algorithms)
            algorithmNames.Add(algorithm.Name);

        _view.SetAlgorithms(algorithmNames);
        _view.SetControlsEnabled(true);
        _view.SetSaveJsonEnabled(false);
        _progressView.ResetProgress();
        _logView.Clear();
        _jsonView.Clear();

        _view.RunClicked += OnRunClicked;
        _view.SaveJsonClicked += OnSaveJsonClicked;
        _view.View2DClicked += OnView2DClicked;
        _view.View3DClicked += OnView3DClicked;
        _progressView.StopClicked += OnStopClicked;
    }

    public void Dispose()
    {
        _view.RunClicked -= OnRunClicked;
        _view.SaveJsonClicked -= OnSaveJsonClicked;
        _view.View2DClicked -= OnView2DClicked;
        _view.View3DClicked -= OnView3DClicked;
        _progressView.StopClicked -= OnStopClicked;
    }

    void OnRunClicked()
    {
        _view.SetControlsEnabled(false);
        _view.SetSaveJsonEnabled(false);
        _progressView.SetRunning(true);
        _progressView.SetProgress(0f);
        _logView.Clear();
        _jsonView.Clear();
        _lastJson = null;

        var options = new MatchRunOptions(
            _view.EnableVisualization,
            _view.EnableLogging,
            _view.AlgorithmIndex);

        _entry.Run(
            options,
            OnRunCompleted,
            _progressView.SetProgress,
            _logView.Add);
    }

    void OnStopClicked()
    {
        _entry.Stop();
    }

    void OnRunCompleted(MatchRunResult result)
    {
        _lastJson = _jsonBuilder.Build(result);
        _jsonView.SetJson(_lastJson);

        _view.SetControlsEnabled(true);
        _view.SetSaveJsonEnabled(true);
        _progressView.SetRunning(false);
    }

    void OnSaveJsonClicked()
    {
        if (string.IsNullOrWhiteSpace(_lastJson))
            return;

        var path = _jsonFileSaver.Save(_lastJson);
        Debug.Log($"JSON сохранен: {path}");
        _logView.Add($"JSON сохранен: {path}");
    }

    void OnView2DClicked()
    {
        _visualizationMode.Set2D();
    }

    void OnView3DClicked()
    {
        _visualizationMode.Set3D();
    }
}
