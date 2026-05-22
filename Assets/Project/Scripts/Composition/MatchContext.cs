using System;

public sealed class MatchContext
{
    public IModelSpaceMatcher Matcher { get; }
    public IMatchStepPlayback StepPlayback { get; }
    public Action Cleanup { get; }

    public MatchContext(IModelSpaceMatcher matcher, IMatchStepPlayback stepPlayback = null, Action cleanup = null)
    {
        Matcher = matcher;
        StepPlayback = stepPlayback;
        Cleanup = cleanup;
    }
}
