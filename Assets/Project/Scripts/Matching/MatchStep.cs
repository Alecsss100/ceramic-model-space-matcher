using UnityEngine;

public enum MatchStepKind
{
    Candidate,
    SkippedDuplicate,
    Rejected,
    Accepted
}

public readonly struct MatchStep
{
    public MatchStepKind Kind { get; }
    public int SpaceIndex { get; }
    public Matrix4x4 Offset { get; }

    public MatchStep(MatchStepKind kind, int spaceIndex, Matrix4x4 offset)
    {
        Kind = kind;
        SpaceIndex = spaceIndex;
        Offset = offset;
    }
}
