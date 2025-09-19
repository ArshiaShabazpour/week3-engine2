using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct FruitMapping
{
    public FruitType type;
    public GameObject prefab; 
}

public class FruitSpawner : MonoBehaviour
{
    public FruitMapping[] mappings;
    private Dictionary<FruitType, GameObject> _map;

    void Awake()
    {
        _map = new Dictionary<FruitType, GameObject>();
        foreach (var m in mappings)
        {
            if (m.prefab != null && !_map.ContainsKey(m.type))
                _map.Add(m.type, m.prefab);
        }
    }

    public GameObject SpawnFruit(FruitType type, Vector2 position)
    {
        var prefab = _map[type];
        var go = Instantiate(prefab, (Vector3)position, Quaternion.identity, transform);
        return go;
    }

    public void SpawnRandomFruits(int count, Vector2 min, Vector2 max)
    {
        var types = (FruitType[])System.Enum.GetValues(typeof(FruitType));
        for (int i = 0; i < count; i++)
        {
            var t = types[UnityEngine.Random.Range(0, types.Length)];
            var pos = new Vector2(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y)
            );
            SpawnFruit(t, pos);
        }
    }
}