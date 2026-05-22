public sealed class MatchRunOptions
{
    public bool RunStepByStep { get; }
    public bool EnableVisualization { get; }

    public MatchRunOptions(bool runStepByStep, bool enableVisualization)
    {
        RunStepByStep = runStepByStep;
        EnableVisualization = enableVisualization;
    }
}
