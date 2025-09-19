using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Setup")]
    public Enemy enemyPrefab;

    [Header("Spawn Area (world space)")]
    public Vector2 minSpawn = new Vector2(-65, 0);
    public Vector2 maxSpawn = new Vector2(65, 0);

    [Header("Parent for spawned enemies (optional)")]
    public Transform spawnParent;

 

    void Start()
    {

    }

    public List<Enemy> SpawnEnemies(int count)
    {
        var spawned = new List<Enemy>();

        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner: No enemyPrefab assigned.");
            return spawned;
        }

        for (int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2(
                Random.Range(minSpawn.x, maxSpawn.x),
                Random.Range(minSpawn.y, maxSpawn.y)
            );

            var go = Instantiate(enemyPrefab.gameObject, (Vector3)pos, Quaternion.identity, spawnParent);
            var enemy = go.GetComponent<Enemy>();
            if (enemy != null) spawned.Add(enemy);
        }

        return spawned;
    }

    public List<Enemy> SpawnEnemies(int count, float patrolHalfWidth)
    {
        var spawned = new List<Enemy>();

        for (int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2(
                Random.Range(minSpawn.x, maxSpawn.x),
                Random.Range(minSpawn.y, maxSpawn.y)
            );

            var go = Instantiate(enemyPrefab.gameObject, (Vector3)pos, Quaternion.identity, spawnParent);
            var enemy = go.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.InitializePatrol(new Vector2(pos.x - patrolHalfWidth, pos.y), new Vector2(pos.x + patrolHalfWidth, pos.y));
                spawned.Add(enemy);
            }
        }

        return spawned;
    }
}