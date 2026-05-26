using UnityEngine;
using VContainer;

public class MatrixVisualizationEntry : MonoBehaviour
{
    IMatrixVisualizer _visualizer;

    [Inject]
    public void Construct(IMatrixVisualizer visualizer)
    {
        _visualizer = visualizer;
    }

    void Start()
    {
        var model = MatrixJsonLoader.Load("Data/model");
        var space = MatrixJsonLoader.Load("Data/space");

        var modelRoot = new GameObject("Model").transform;
        var spaceRoot = new GameObject("Space").transform;

        var materials = VisualizationMaterialLibrary.Instance;
        _visualizer.Visualize(modelRoot, model, materials.Model);
        _visualizer.Visualize(spaceRoot, space, materials.Space);
    }
}
