public sealed class MatchContext
{
    public IModelSpaceMatcher Matcher { get; }
    public IMatchStepPlayback StepPlayback { get; }

    public MatchContext(IModelSpaceMatcher matcher, IMatchStepPlayback stepPlayback = null)
    {
        Matcher = matcher;
        StepPlayback = stepPlayback;
    }
}
