using TMPro;
using UnityEngine;

public class MatchJsonView : MonoBehaviour
{
    [SerializeField] TMP_Text _jsonText;
    [SerializeField] string _emptyText = "{}";

    public void Clear()
    {
        SetJson(_emptyText);
    }

    public void SetJson(string json)
    {
        if (_jsonText != null)
            _jsonText.text = string.IsNullOrWhiteSpace(json) ? _emptyText : json;
    }
}
