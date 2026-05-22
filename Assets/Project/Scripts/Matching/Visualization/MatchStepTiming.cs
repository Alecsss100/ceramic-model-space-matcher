using UnityEngine;

[System.Serializable]
public class MatchStepTiming : IMatchStepTiming
{
    [SerializeField] float _candidate = 0;//0.0002f;
    [SerializeField] float _skippedDuplicate = 0.1f;
    [SerializeField] float _rejected = 0.0002f;
    [SerializeField] float _accepted = 1f;

    public float GetDelay(MatchStepKind kind) => kind switch
    {
        MatchStepKind.Candidate => _candidate,
        MatchStepKind.SkippedDuplicate => _skippedDuplicate,
        MatchStepKind.Rejected => _rejected,
        MatchStepKind.Accepted => _accepted,
        _ => _candidate,
    };
}
