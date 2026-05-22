using UnityEngine;

public sealed class VisualizationModeController : IVisualizationProjection
{
    const float Mode2DDistanceFromCamera = 25f;

    readonly VisualizationTransformRegistry _transformRegistry;

    public VisualizationMode CurrentMode { get; private set; } = VisualizationMode.Mode3D;

    public VisualizationModeController(VisualizationTransformRegistry transformRegistry)
    {
        _transformRegistry = transformRegistry;
    }

    public void Set2D()
    {
        CurrentMode = VisualizationMode.Mode2D;
        _transformRegistry.Apply(this);
    }

    public void Set3D()
    {
        CurrentMode = VisualizationMode.Mode3D;
        _transformRegistry.Apply(this);
    }

    public Vector3 ProjectPosition(Vector3 position)
    {
        if (CurrentMode != VisualizationMode.Mode2D)
            return position;

        var camera = Camera.main;
        if (camera == null)
            return new Vector3(position.x, position.y, -Mode2DDistanceFromCamera);

        var transform = camera.transform;
        var planeOrigin = transform.position + transform.forward * Mode2DDistanceFromCamera;
        var offset = position - planeOrigin;

        return planeOrigin
            + transform.right * Vector3.Dot(offset, transform.right)
            + transform.up * Vector3.Dot(offset, transform.up);
    }

    public Quaternion ProjectRotation(Quaternion rotation) =>
        CurrentMode == VisualizationMode.Mode2D
            ? Quaternion.identity
            : rotation;
}
