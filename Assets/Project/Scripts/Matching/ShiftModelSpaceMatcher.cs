using System.Collections.Generic;
using UnityEngine;

public class ShiftModelSpaceMatcher : IModelSpaceMatcher
{
    public string Name => "Translation Brute Force";

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
        yield break;
    }
}
