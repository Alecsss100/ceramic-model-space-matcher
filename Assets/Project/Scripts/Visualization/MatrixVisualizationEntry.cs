using UnityEngine;

public class MatrixVisualizationEntry : MonoBehaviour
{
    IMatrixVisualizer _visualizer;

    void Awake()
    {
        _visualizer = new CubeMatrixVisualizer();
    }

    void Start()
    {
        var model = MatrixJsonLoader.Load("Data/model");
        var space = MatrixJsonLoader.Load("Data/space");

        var modelRoot = new GameObject("Model").transform;
        var spaceRoot = new GameObject("Space").transform;

        _visualizer.Visualize(modelRoot, model, Color.orange);
        _visualizer.Visualize(spaceRoot, space, Color.whiteSmoke);
    }
}
