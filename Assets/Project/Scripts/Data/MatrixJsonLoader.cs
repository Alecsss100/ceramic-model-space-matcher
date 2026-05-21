using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public static class MatrixJsonLoader
{
    public static Matrix4x4[] Load(string resourcePath)
    {
        var textAsset = Resources.Load<TextAsset>(resourcePath);
        if (textAsset == null)
        {
            Debug.LogError($"MatrixJsonLoader: ресурс не найден по пути '{resourcePath}'");
            return System.Array.Empty<Matrix4x4>();
        }

        var dtos = JsonConvert.DeserializeObject<Matrix4x4JsonDto[]>(textAsset.text);
        if (dtos == null || dtos.Length == 0)
        {
            Debug.LogError($"MatrixJsonLoader: не удалось десериализовать '{resourcePath}'");
            return System.Array.Empty<Matrix4x4>();
        }

        return dtos.Select(d => d.ToMatrix4x4()).ToArray();
    }
}
