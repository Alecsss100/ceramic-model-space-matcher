using System;
using System.Collections;
using UnityEngine;

public interface IMatchStepPlayback
{
    IEnumerator PlayFindSteps(
        Matrix4x4[] model,
        Matrix4x4[] space,
        Action<MatchStep> onStep = null,
        Func<bool> shouldCancel = null);
}
