using UnityEngine;

public class MatrixDataTest : MonoBehaviour
{
    Matrix4x4[] _model;
    Matrix4x4[] _space;

    void Start()
    {
        _model = MatrixJsonLoader.Load("Data/model");
        _space = MatrixJsonLoader.Load("Data/space");

        Debug.Log($"model: {_model.Length}, space: {_space.Length}");

        Debug.Log("Элементы массива model: \n" + string.Join("\n", _model));
        Debug.Log("Элементы массива space: \n" + string.Join("\n", _space));
    }
}
