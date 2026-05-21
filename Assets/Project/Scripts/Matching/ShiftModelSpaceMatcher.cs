using UnityEngine;

public class ShiftModelSpaceMatcher : IModelSpaceMatcher
{
    public string Name => "Translation Brute Force";

    public MatchResult Find(Matrix4x4[] model, Matrix4x4[] space)
    {
        var result = new MatchResult();

        // TODO реалиазовать алгоритм матчинга
        Debug.Log("ShiftModelSpaceMatcher: алгоритм матчинга в процессе реализации");

        return result;
    }
}
