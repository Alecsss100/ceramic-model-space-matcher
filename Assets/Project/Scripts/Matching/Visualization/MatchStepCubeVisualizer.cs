using UnityEngine;

public class MatchStepCubeVisualizer : IMatchStepVisualizer
{
    readonly CubePool _modelPool;
    readonly CubePool _markerPool;
    readonly Material _candidateMaterial;
    readonly Material _skippedMaterial;
    readonly Material _rejectedMaterial;
    readonly Material _acceptedMaterial;

    public MatchStepCubeVisualizer(
        Transform parent,
        int modelCount,
        IVisualizationProjection projection,
        VisualizationTransformRegistry transformRegistry)
    {
        _modelPool = new CubePool(parent, modelCount, projection, transformRegistry, "MatchStepModelPool");
        _markerPool = new CubePool(parent, 1, projection, transformRegistry, "MatchStepMarkerPool");

        _candidateMaterial = MaterialExtensions.CreateMaterial(MatchStepColors.Candidate);
        _skippedMaterial = MaterialExtensions.CreateMaterial(MatchStepColors.SkippedDuplicate);
        _rejectedMaterial = MaterialExtensions.CreateMaterial(MatchStepColors.Rejected);
        _acceptedMaterial = MaterialExtensions.CreateMaterial(MatchStepColors.Accepted);
    }

    public void ShowStep(MatchStep step, Matrix4x4[] model, Matrix4x4[] space)
    {
        _modelPool.HideAll();
        _markerPool.HideAll();

        switch (step.Kind)
        {
            case MatchStepKind.Candidate:
                ShowShiftedModel(step.Offset, model, _candidateMaterial);
                break;
            case MatchStepKind.Rejected:
                ShowShiftedModel(step.Offset, model, _rejectedMaterial);
                break;
            case MatchStepKind.Accepted:
                ShowShiftedModel(step.Offset, model, _acceptedMaterial);
                break;
            case MatchStepKind.SkippedDuplicate:
                ShowSpaceMarker(step.SpaceIndex, space, _skippedMaterial);
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
