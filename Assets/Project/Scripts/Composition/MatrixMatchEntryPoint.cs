using VContainer.Unity;

public sealed class MatrixMatchEntryPoint : IStartable
{
    readonly MatrixMatchEntry _entry;

    public MatrixMatchEntryPoint(MatrixMatchEntry entry)
    {
        _entry = entry;
    }

    public void Start() => _entry.Run();
}
