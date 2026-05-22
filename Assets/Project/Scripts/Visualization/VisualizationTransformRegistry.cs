using System.Collections.Generic;
using UnityEngine;

public sealed class VisualizationTransformRegistry
{
    readonly Dictionary<Transform, TransformState> _states = new();
    readonly List<Transform> _staleTransforms = new();

    public void Register(Transform target, Vector3 originalPosition, Quaternion originalRotation)
    {
        if (target == null)
            return;

        _states[target] = new TransformState(originalPosition, originalRotation);
    }

    public void Apply(IVisualizationProjection projection)
    {
        _staleTransforms.Clear();

        foreach (var pair in _states)
        {
            var target = pair.Key;
            if (target == null)
            {
                _staleTransforms.Add(target);
                continue;
            }

            var state = pair.Value;
            target.SetPositionAndRotation(
                projection.ProjectPosition(state.Position),
                projection.ProjectRotation(state.Rotation));
        }

        foreach (var target in _staleTransforms)
            _states.Remove(target);
    }

    public void Clear() => _states.Clear();

    readonly struct TransformState
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;

        public TransformState(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
