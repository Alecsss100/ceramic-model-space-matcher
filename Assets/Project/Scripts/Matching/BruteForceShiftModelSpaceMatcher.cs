using UnityEngine;

public class BruteForceShiftModelSpaceMatcher : IModelSpaceMatcher
{
    readonly float _epsilon;

    public string Name => "Matrix Brute Force";

    public BruteForceShiftModelSpaceMatcher(float epsilon = Matrix4x4Extensions.DefaultEpsilon)
    {
        _epsilon = epsilon;
    }

    public MatchResult Find(Matrix4x4[] model, Matrix4x4[] space)
    {
        var result = new MatchResult();

        // edge кейсы
        if (model == null || model.Length == 0 || space == null || space.Length == 0)
        {
            Debug.LogWarning($"{nameof(BruteForceShiftModelSpaceMatcher)}: пустые входные данные");
            return result;
        }

        var anchorInverse = model[0].inverse;

        for (var spaceIndex = 0; spaceIndex < space.Length; spaceIndex++)
        {
            var offset = space[spaceIndex] * anchorInverse;

            if (ContainsOffset(result, offset))
                continue;

            if (MatchesAllModelMatrices(model, space, offset))
                result.Offsets.Add(offset);
        }

        return result;
    }

    bool MatchesAllModelMatrices(Matrix4x4[] model, Matrix4x4[] space, Matrix4x4 offset)
    {
        for (var modelIndex = 0; modelIndex < model.Length; modelIndex++)
        {
            var expected = offset * model[modelIndex];

            if (!ContainsMatrix(space, expected))
                return false;
        }

        return true;
    }

    bool ContainsMatrix(Matrix4x4[] space, Matrix4x4 expected)
    {
        for (var spaceIndex = 0; spaceIndex < space.Length; spaceIndex++)
        {
            if (space[spaceIndex].Approximately(expected, _epsilon))
                return true;
        }

        return false;
    }

    bool ContainsOffset(MatchResult result, Matrix4x4 offset)
    {
        for (var i = 0; i < result.Offsets.Count; i++)
        {
            if (result.Offsets[i].Approximately(offset, _epsilon))
                return true;
        }

        return false;
    }
}
