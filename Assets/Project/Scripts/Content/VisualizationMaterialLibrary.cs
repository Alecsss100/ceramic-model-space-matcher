using UnityEngine;

[CreateAssetMenu(
    fileName = "VisualizationMaterialLibrary",
    menuName = "Project/Visualization Material Library")]
public sealed class VisualizationMaterialLibrary : ScriptableObject
{
    const string ResourcePath = "VisualizationMaterialLibrary";

    static VisualizationMaterialLibrary _instance;

    [SerializeField] Material _modelMaterial;
    [SerializeField] Material _spaceMaterial;
    [SerializeField] Material _stepCheckMaterial;
    [SerializeField] Material _stepStoredMaterial;
    [SerializeField] Material _stepFailedMaterial;
    [SerializeField] Material _stepAcceptedMaterial;

    public static VisualizationMaterialLibrary Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            _instance = Resources.Load<VisualizationMaterialLibrary>(ResourcePath);

            if (_instance == null)
                Debug.LogError(
                    $"VisualizationMaterialLibrary: ассет не найден по пути Resources/{ResourcePath}");

            return _instance;
        }
    }

    public Material Model => _modelMaterial;
    public Material Space => _spaceMaterial;

    public Material GetStepMaterial(MatchStepKind kind) => kind switch
    {
        MatchStepKind.Candidate => _stepCheckMaterial,
        MatchStepKind.SkippedDuplicate => _stepStoredMaterial,
        MatchStepKind.Rejected => _stepFailedMaterial,
        MatchStepKind.Accepted => _stepAcceptedMaterial,
        _ => _stepCheckMaterial
    };

    void OnEnable() => _instance = this;
}
