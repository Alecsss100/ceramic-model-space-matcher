using System.Collections.Generic;
using UnityEngine;

public class CubePool
{
    readonly List<GameObject> _cubes = new();

    public CubePool(Transform parent, int capacity, string name = "CubePool")
    {
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
        cube.transform.SetPositionAndRotation(position, rotation);
        cube.transform.localScale = scale;
        cube.GetComponent<Renderer>().material = material;
    }

    public void SetActiveCount(int count)
    {
        for (var i = 0; i < _cubes.Count; i++)
            _cubes[i].SetActive(i < count);
    }

    public void HideAll() => SetActiveCount(0);
}
