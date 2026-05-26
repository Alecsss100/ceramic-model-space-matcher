using UnityEngine;

public class MatchStepCubeVisualizer : IMatchStepVisualizer
{
    readonly CubePool _modelPool;
    readonly CubePool _markerPool;
    readonly VisualizationMaterialLibrary _materials;

    public MatchStepCubeVisualizer(
        Transform parent,
        int modelCount,
        IVisualizationProjection projection,
        VisualizationTransformRegistry transformRegistry)
    {
        _materials = VisualizationMaterialLibrary.Instance;
        _modelPool = new CubePool(parent, modelCount, projection, transformRegistry, "MatchStepModelPool");
        _markerPool = new CubePool(parent, 1, projection, transformRegistry, "MatchStepMarkerPool");
    }

    public void ShowStep(MatchStep step, Matrix4x4[] model, Matrix4x4[] space)
    {
        _modelPool.HideAll();
        _markerPool.HideAll();

        switch (step.Kind)
        {
            case MatchStepKind.Candidate:
            case MatchStepKind.Rejected:
            case MatchStepKind.Accepted:
                ShowShiftedModel(step.Offset, model, _materials.GetStepMaterial(step.Kind));
                break;
            case MatchStepKind.SkippedDuplicate:
                ShowSpaceMarker(step.SpaceIndex, space, _materials.GetStepMaterial(step.Kind));
                break;
        }
    }

    public void Clear()
    {
        _modelPool.HideAll();
        _markerPool.HideAll();
    }

    void ShowShiftedModel(Matrix4x4 offset, Matrix4x4[] model, Material material)
    {
        for (var i = 0; i < model.Length; i++)
        {
            var shifted = offset * model[i];
            _modelPool.Place(i, shifted.GetColumn(3), Vector3.one * 2, shifted.rotation, material);
        }

        _modelPool.SetActiveCount(model.Length);
    }

    void ShowSpaceMarker(int spaceIndex, Matrix4x4[] space, Material material)
    {
        var matrix = space[spaceIndex];
        _markerPool.Place(0, matrix.GetColumn(3), Vector3.one * 2, matrix.rotation, material);
        _markerPool.SetActiveCount(1);
    }
}
