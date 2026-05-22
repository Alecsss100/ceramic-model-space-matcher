using UnityEngine;

public static class MatchStepColors
{
    public static Color GetColor(MatchStepKind kind) => kind switch
    {
        MatchStepKind.Candidate => Candidate,
        MatchStepKind.SkippedDuplicate => SkippedDuplicate,
        MatchStepKind.Rejected => Rejected,
        MatchStepKind.Accepted => Accepted,
        _ => Color.magenta
    };

    public static string GetLabel(MatchStepKind kind) => kind switch
    {
        MatchStepKind.Candidate => "Кандидат — проверяем смещение (синий, сдвинутый model)",
        MatchStepKind.SkippedDuplicate => "Дубликат — offset уже был (чёрный маркер на space)",
        MatchStepKind.Rejected => "Не подошло — model не совпал со space (красный, сдвинутый model)",
        MatchStepKind.Accepted => "Найдено — все model совпали (зелёный, сдвинутый model)",
        _ => kind.ToString()
    };

    /// <summary>Синий — проверяемый offset, model со смещением.</summary>
    public static readonly Color Candidate = new(0.2f, 0.4f, 1f, 0.7f);

    /// <summary>Чёрный — дубликат offset, один маркер на точке space.</summary>
    public static readonly Color SkippedDuplicate = new(0.1f, 0.1f, 0.1f, 0.8f);

    /// <summary>Красный — offset отклонён.</summary>
    public static readonly Color Rejected = new(1f, 0.2f, 0.2f, 0.7f);

    /// <summary>Зелёный — offset принят.</summary>
    public static readonly Color Accepted = new(0.2f, 1f, 0.3f, 0.8f);
}
