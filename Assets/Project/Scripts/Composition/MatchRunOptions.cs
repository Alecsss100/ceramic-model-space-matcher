public sealed class MatchRunOptions
{
    public bool EnableVisualization { get; }
    public bool EnableLogging { get; }
    public int AlgorithmIndex { get; }

    public MatchRunOptions(bool enableVisualization, bool enableLogging, int algorithmIndex)
    {
        EnableVisualization = enableVisualization;
        EnableLogging = enableLogging;
        AlgorithmIndex = algorithmIndex;
    }
}
