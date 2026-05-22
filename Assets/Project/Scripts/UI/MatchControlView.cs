using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchControlView : MonoBehaviour
{
    [SerializeField] Button _view2DButton;
    [SerializeField] Button _view3DButton;
    [SerializeField] Button _runButton;
    [SerializeField] Toggle _visualizeToggle;
    [SerializeField] Toggle _loggingToggle;
    [SerializeField] TMP_Dropdown _algorithmDropdown;

    public event Action RunClicked;
    public event Action View2DClicked;
    public event Action View3DClicked;

    public bool EnableVisualization => _visualizeToggle != null && _visualizeToggle.isOn;
    public bool EnableLogging => _loggingToggle != null && _loggingToggle.isOn;
    public int AlgorithmIndex => _algorithmDropdown != null ? _algorithmDropdown.value : 0;

    void OnEnable()
    {
        _runButton?.onClick.AddListener(OnRunClicked);
        _view2DButton?.onClick.AddListener(OnView2DClicked);
        _view3DButton?.onClick.AddListener(OnView3DClicked);
    }

    void OnDisable()
    {
        _runButton?.onClick.RemoveListener(OnRunClicked);
        _view2DButton?.onClick.RemoveListener(OnView2DClicked);
        _view3DButton?.onClick.RemoveListener(OnView3DClicked);
    }

    public void SetAlgorithms(IReadOnlyList<string> algorithmNames)
    {
        if (_algorithmDropdown == null)
            return;

        _algorithmDropdown.ClearOptions();
        _algorithmDropdown.AddOptions(new List<string>(algorithmNames));
        _algorithmDropdown.value = 0;
        _algorithmDropdown.RefreshShownValue();
    }

    public void SetControlsEnabled(bool enabled)
    {
        if (_runButton != null)
            _runButton.interactable = enabled;
        if (_visualizeToggle != null)
            _visualizeToggle.interactable = enabled;
        if (_loggingToggle != null)
            _loggingToggle.interactable = enabled;
        if (_algorithmDropdown != null)
            _algorithmDropdown.interactable = enabled;

        if (_view2DButton != null)
            _view2DButton.interactable = true;
        if (_view3DButton != null)
            _view3DButton.interactable = true;
    }

    void OnRunClicked() => RunClicked?.Invoke();
    void OnView2DClicked() => View2DClicked?.Invoke();
    void OnView3DClicked() => View3DClicked?.Invoke();
}
