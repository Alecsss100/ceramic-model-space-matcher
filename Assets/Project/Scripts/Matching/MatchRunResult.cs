public sealed class MatchRunResult
{
    public string AlgorithmName { get; }
    public int ModelCount { get; }
    public int SpaceCount { get; }
    public bool WasCancelled { get; }
    public MatchResult Result { get; }

    public MatchRunResult(
        string algorithmName,
        int modelCount,
        int spaceCount,
        bool wasCancelled,
        MatchResult result)
    {
        AlgorithmName = algorithmName;
        ModelCount = modelCount;
        SpaceCount = spaceCount;
        WasCancelled = wasCancelled;
        Result = result;
    }
}
