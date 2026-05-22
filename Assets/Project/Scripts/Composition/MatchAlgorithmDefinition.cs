using System;

public sealed class MatchAlgorithmDefinition
{
    readonly Func<IModelSpaceMatcher> _createMatcher;

    public string Name { get; }

    public MatchAlgorithmDefinition(string name, Func<IModelSpaceMatcher> createMatcher)
    {
        Name = name;
        _createMatcher = createMatcher;
    }

    public IModelSpaceMatcher CreateMatcher() => _createMatcher();
}
