using UnityEngine;

public class MatrixMatchEntry : MonoBehaviour
{
    IModelSpaceMatcher _matcher;

    void Awake()
    {
        _matcher = new BruteForceShiftModelSpaceMatcher();
    }

    void Start()
    {
        var model = MatrixJsonLoader.Load("Data/model");
        var space = MatrixJsonLoader.Load("Data/space");

        var result = _matcher.Find(model, space);
        Debug.Log($"{_matcher.Name}: смещений найдено - {result.Offsets.Count}");
    }
}
