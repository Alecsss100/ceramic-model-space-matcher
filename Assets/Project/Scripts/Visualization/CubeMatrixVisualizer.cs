using UnityEngine;

public class CubeMatrixVisualizer : IMatrixVisualizer
{
    readonly IVisualizationProjection _projection;
    readonly VisualizationTransformRegistry _transformRegistry;

    public CubeMatrixVisualizer(
        IVisualizationProjection projection,
        VisualizationTransformRegistry transformRegistry)
    {
        _projection = projection;
        _transformRegistry = transformRegistry;
    }

    public Transform Visualize(Transform parent, Matrix4x4[] matrices, Material material)
    {
        var root = new GameObject($"Visualized ({matrices.Length})").transform;
        root.SetParent(parent, false);

        for (var i = 0; i < matrices.Length; i++)
        {
            var matrix = matrices[i];
            var position = matrix.GetColumn(3);
            var rotation = matrix.rotation;
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = $"Cube_{i}";
            _transformRegistry.Register(cube.transform, position, rotation);
            cube.transform.SetPositionAndRotation(
                _projection.ProjectPosition(position),
                _projection.ProjectRotation(rotation));
            cube.transform.SetParent(root, false);

            Object.Destroy(cube.GetComponent<BoxCollider>());
            cube.GetComponent<Renderer>().sharedMaterial = material;
        }

        return root;
    }
}
