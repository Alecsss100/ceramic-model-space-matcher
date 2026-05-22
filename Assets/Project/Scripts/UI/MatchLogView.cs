using TMPro;
using UnityEngine;

public class MatchLogView : MonoBehaviour
{
    [SerializeField] TMP_Text[] _lines;

    public void Clear()
    {
        if (_lines == null)
            return;

        for (var i = 0; i < _lines.Length; i++)
        {
            if (_lines[i] == null)
                continue;

            _lines[i].text = string.Empty;
            _lines[i].gameObject.SetActive(false);
        }
    }

    public void Add(string message)
    {
        if (_lines == null || _lines.Length == 0)
            return;

        var insertIndex = GetFirstInactiveIndex();
        if (insertIndex < 0)
        {
            for (var i = 1; i < _lines.Length; i++)
                _lines[i - 1].text = _lines[i].text;

            insertIndex = _lines.Length - 1;
        }

        if (_lines[insertIndex] == null)
            return;

        _lines[insertIndex].text = message;
        _lines[insertIndex].gameObject.SetActive(true);
    }

    int GetFirstInactiveIndex()
    {
        for (var i = 0; i < _lines.Length; i++)
        {
            if (_lines[i] != null && !_lines[i].gameObject.activeSelf)
                return i;
        }

        return -1;
    }
}
