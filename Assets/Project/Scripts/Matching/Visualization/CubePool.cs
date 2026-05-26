using System.Collections.Generic;
using UnityEngine;

public class CubePool
{
    readonly List<GameObject> _cubes = new();
    readonly IVisualizationProjection _projection;
    readonly VisualizationTransformRegistry _transformRegistry;

    public CubePool(
        Transform parent,
        int capacity,
        IVisualizationProjection projection,
        VisualizationTransformRegistry transformRegistry,
        string name = "CubePool")
    {
        _projection = projection;
        _transformRegistry = transformRegistry;

        var root = new GameObject(name).transform;
        root.SetParent(parent, false);

        for (var i = 0; i < capacity; i++)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = $"PooledCube_{i}";
            Object.Destroy(cube.GetComponent<BoxCollider>());
            cube.transform.SetParent(root, false);
            cube.SetActive(false);
            _cubes.Add(cube);
        }
    }

    public int Capacity => _cubes.Count;

    public void Place(int index, Vector3 position, Vector3 scale, Quaternion rotation, Material material)
    {
        var cube = _cubes[index];
        cube.SetActive(true);
        _transformRegistry.Register(cube.transform, position, rotation);
        cube.transform.SetPositionAndRotation(
            _projection.ProjectPosition(position),
            _projection.ProjectRotation(rotation));
        cube.transform.localScale = scale;
        cube.GetComponent<Renderer>().sharedMaterial = material;
    }

    public void SetActiveCount(int count)
    {
        for (var i = 0; i < _cubes.Count; i++)
            _cubes[i].SetActive(i < count);
    }

    public void HideAll() => SetActiveCount(0);
}
