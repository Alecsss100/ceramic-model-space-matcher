using System.Collections.Generic;
using UnityEngine;

public interface IModelSpaceMatcher
{
    string Name { get; }
    MatchResult Find(Matrix4x4[] model, Matrix4x4[] space);
    IEnumerable<MatchStep> FindSteps(Matrix4x4[] model, Matrix4x4[] space);
}
