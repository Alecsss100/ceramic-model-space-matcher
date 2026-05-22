using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

public sealed class MatchControlPresenter : IStartable, IDisposable
{
    readonly MatchControlView _view;
    readonly MatchProgressView _progressView;
    readonly MatchLogView _logView;
    readonly MatrixMatchEntry _entry;
    readonly MatchAlgorithmRegistry _algorithmRegistry;
    readonly VisualizationModeController _visualizationMode;

    public MatchControlPresenter(
        MatchControlView view,
        MatchProgressView progressView,
        MatchLogView logView,
        MatrixMatchEntry entry,
        MatchAlgorithmRegistry algorithmRegistry,
        VisualizationModeController visualizationMode)
    {
        _view = view;
        _progressView = progressView;
        _logView = logView;
        _entry = entry;
        _algorithmRegistry = algorithmRegistry;
        _visualizationMode = visualizationMode;
    }

    public void Start()
    {
        var algorithmNames = new List<string>();
        foreach (var algorithm in _algorithmRegistry.Algorithms)
            algorithmNames.Add(algorithm.Name);

        _view.SetAlgorithms(algorithmNames);
        _view.SetControlsEnabled(true);
        _progressView.ResetProgress();
        _logView.Clear();

        _view.RunClicked += OnRunClicked;
        _view.View2DClicked += OnView2DClicked;
        _view.View3DClicked += OnView3DClicked;
        _progressView.StopClicked += OnStopClicked;
    }

    public void Dispose()
    {
        _view.RunClicked -= OnRunClicked;
        _view.View2DClicked -= OnView2DClicked;
        _view.View3DClicked -= OnView3DClicked;
        _progressView.StopClicked -= OnStopClicked;
    }

    void OnRunClicked()
    {
        _view.SetControlsEnabled(false);
        _progressView.SetRunning(true);
        _progressView.SetProgress(0f);
        _logView.Clear();

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

    void OnRunCompleted()
    {
        _view.SetControlsEnabled(true);
        _progressView.SetRunning(false);
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
