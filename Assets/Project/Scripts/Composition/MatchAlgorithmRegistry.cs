using System.Collections.Generic;

public sealed class MatchAlgorithmRegistry
{
    readonly MatchAlgorithmDefinition[] _algorithms =
    {
        new("Offset", () => new OffsetModelSpaceMatcher()),
        new("Matrix Brute Force", () => new BruteForceShiftModelSpaceMatcher()),
    };

    public IReadOnlyList<MatchAlgorithmDefinition> Algorithms => _algorithms;

    public IModelSpaceMatcher CreateMatcher(int index)
    {
        if (index < 0 || index >= _algorithms.Length)
            index = 0;

        return _algorithms[index].CreateMatcher();
    }
}
