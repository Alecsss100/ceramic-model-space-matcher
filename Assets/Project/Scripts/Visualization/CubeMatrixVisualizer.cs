using UnityEngine;

public class CubeMatrixVisualizer : IMatrixVisualizer
{
    static Material CreateMaterial(Color color)
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit");
        var material = new Material(shader);
        material.color = color;

        if (color.a < 1f)
        {
            material.SetFloat("_Surface", 1f);
            material.SetFloat("_Blend", 0f);
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

        return material;
    }

    public Transform Visualize(Transform parent, Matrix4x4[] matrices, Color color)
    {
        var root = new GameObject($"Visualized ({matrices.Length})").transform;
        root.SetParent(parent, false);

        var material = CreateMaterial(color);

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
