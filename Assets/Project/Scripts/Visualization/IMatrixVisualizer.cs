using UnityEngine;

public interface IMatrixVisualizer
{
    Transform Visualize(Transform parent, Matrix4x4[] matrices, Material material);
}
