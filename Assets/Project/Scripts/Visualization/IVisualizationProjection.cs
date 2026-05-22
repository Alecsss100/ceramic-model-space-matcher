using UnityEngine;

public interface IVisualizationProjection
{
    Vector3 ProjectPosition(Vector3 position);
    Quaternion ProjectRotation(Quaternion rotation);
}
