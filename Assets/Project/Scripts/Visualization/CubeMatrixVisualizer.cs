using UnityEngine;

public class CubeMatrixVisualizer : IMatrixVisualizer
{
    public Transform Visualize(Transform parent, Matrix4x4[] matrices, Color color)
    {
        var root = new GameObject($"Visualized ({matrices.Length})").transform;
        root.SetParent(parent, false);

        var material = MaterialExtensions.CreateMaterial(color);

        for (var i = 0; i < matrices.Length; i++)
        {
            var matrix = matrices[i];
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = $"Cube_{i}";
            cube.transform.SetPositionAndRotation(matrix.GetColumn(3), matrix.rotation);
            cube.transform.SetParent(root, false);

            Object.Destroy(cube.GetComponent<BoxCollider>());
            cube.GetComponent<Renderer>().material = material;
        }

        return root;
    }
}
