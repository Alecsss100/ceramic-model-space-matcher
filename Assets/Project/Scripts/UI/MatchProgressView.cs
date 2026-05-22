using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchProgressView : MonoBehaviour
{
    [SerializeField] Button _stopButton;
    [SerializeField] Slider _progressSlider;
    [SerializeField] TMP_Text _progressText;

    public event Action StopClicked;

    void OnEnable()
    {
        _stopButton?.onClick.AddListener(OnStopClicked);

        if (_progressSlider != null)
            _progressSlider.interactable = false;
    }

    void OnDisable()
    {
        _stopButton?.onClick.RemoveListener(OnStopClicked);
    }

    public void ResetProgress()
    {
        SetProgress(0f);
        SetRunning(false);
    }

    public void SetRunning(bool isRunning)
    {
        if (_stopButton != null)
            _stopButton.interactable = isRunning;
    }

    public void SetProgress(float progress)
    {
        progress = Mathf.Clamp01(progress);

        if (_progressSlider != null)
            _progressSlider.normalizedValue = progress;
        if (_progressText != null)
            _progressText.text = $"{Mathf.RoundToInt(progress * 100f)}%";
    }

    void OnStopClicked() => StopClicked?.Invoke();
}
