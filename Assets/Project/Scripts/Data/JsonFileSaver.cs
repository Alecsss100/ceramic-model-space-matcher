using System;
using System.IO;
using System.Text;
using UnityEngine;

public sealed class JsonFileSaver
{
    const string ResultsDirectoryName = "MatchResults";

    public string Save(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            json = "{}";

        var directoryPath = Path.Combine(Application.persistentDataPath, ResultsDirectoryName);
        Directory.CreateDirectory(directoryPath);

        var fileName = $"match-result-{DateTime.Now:yyyyMMdd-HHmmss}.json";
        var filePath = Path.Combine(directoryPath, fileName);
        File.WriteAllText(filePath, json, Encoding.UTF8);

        return filePath;
    }
}
