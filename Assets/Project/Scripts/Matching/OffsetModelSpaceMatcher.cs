using System.Collections.Generic;
using UnityEngine;

public class OffsetModelSpaceMatcher : IModelSpaceMatcher
{
    readonly float _epsilon;

    public string Name => "Offset";

    public OffsetModelSpaceMatcher(float epsilon = Matrix4x4Extensions.DefaultEpsilon)
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
            Debug.LogWarning($"{nameof(OffsetModelSpaceMatcher)}: пустые входные данные");
            yield break;
        }

        var indexedSpace = BuildSpaceIndex(space);
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

            if (MatchesAllModelMatrices(model, space, indexedSpace, offset))
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

    bool MatchesAllModelMatrices(Matrix4x4[] model, Matrix4x4[] space, Dictionary<Matrix4x4, List<Matrix4x4>> spaceIndex, Matrix4x4 offset)
    {
        for (var modelIndex = 0; modelIndex < model.Length; modelIndex++)
        {
            var expected = offset * model[modelIndex];

            if (!ContainsMatrix(space, spaceIndex, expected))
                return false;
        }

        return true;
    }

    Dictionary<Matrix4x4, List<Matrix4x4>> BuildSpaceIndex(Matrix4x4[] space)
    {
        var spaceIndex = new Dictionary<Matrix4x4, List<Matrix4x4>>();

        for (var i = 0; i < space.Length; i++)
        {
            var key = space[i].SnapToEpsilon(_epsilon);

            if (!spaceIndex.TryGetValue(key, out var bucket))
            {
                spaceIndex[key] = new List<Matrix4x4> { space[i] };
                continue;
            }

            bucket.Add(space[i]);
        }

        return spaceIndex;
    }

    bool ContainsMatrix(Matrix4x4[] space, Dictionary<Matrix4x4, List<Matrix4x4>> spaceIndex, Matrix4x4 expected)
    {
        var key = expected.SnapToEpsilon(_epsilon);

        if (spaceIndex.TryGetValue(key, out var bucket))
        {
            for (var i = 0; i < bucket.Count; i++)
            {
                if (bucket[i].Approximately(expected, _epsilon))
                    return true;
            }
        }

        return ContainsMatrixSlow(space, expected);
    }

    bool ContainsMatrixSlow(Matrix4x4[] space, Matrix4x4 expected)
    {
        for (var i = 0; i < space.Length; i++)
        {
            if (space[i].Approximately(expected, _epsilon))
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
