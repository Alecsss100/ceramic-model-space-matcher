using System.Collections.Generic;
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

        foreach (var step in FindSteps(model, space))
        {
            if (step.Kind == MatchStepKind.Accepted)
                result.Offsets.Add(step.Offset);
        }

        return result;
    }

    public IEnumerable<MatchStep> FindSteps(Matrix4x4[] model, Matrix4x4[] space)
    {
        if (model == null || model.Length == 0 || space == null || space.Length == 0)
        {
            Debug.LogWarning($"{nameof(BruteForceShiftModelSpaceMatcher)}: пустые входные данные");
            yield break;
        }

        var foundOffsets = new List<Matrix4x4>();
        var anchorInverse = model[0].inverse;

        for (var spaceIndex = 0; spaceIndex < space.Length; spaceIndex++)
        {
            var offset = space[spaceIndex] * anchorInverse;

            if (ContainsOffset(foundOffsets, offset))
            {
                yield return new MatchStep(MatchStepKind.SkippedDuplicate, spaceIndex, offset);
                continue;
            }

            yield return new MatchStep(MatchStepKind.Candidate, spaceIndex, offset);

            if (MatchesAllModelMatrices(model, space, offset))
            {
                foundOffsets.Add(offset);
                yield return new MatchStep(MatchStepKind.Accepted, spaceIndex, offset);
            }
            else
            {
                yield return new MatchStep(MatchStepKind.Rejected, spaceIndex, offset);
            }
        }
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

    bool ContainsOffset(List<Matrix4x4> offsets, Matrix4x4 offset)
    {
        for (var i = 0; i < offsets.Count; i++)
        {
            if (offsets[i].Approximately(offset, _epsilon))
                return true;
        }

        return false;
    }
}
