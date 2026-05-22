using UnityEngine;

public interface IMatchStepVisualizer
{
    void ShowStep(MatchStep step, Matrix4x4[] model, Matrix4x4[] space);
    void Clear();
}
