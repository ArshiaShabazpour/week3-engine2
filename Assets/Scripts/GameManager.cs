using UnityEngine;
using System;
public class GameManager : Singleton<GameManager>
{
    [Header("Game Rules")]
    public int targetScore = 20; 
    public float loseY = -10f;   

    [Header("References")]
    public FruitSpawner fruitSpawner;

    [Header("Enemy Spawner")]
    public EnemySpawner enemySpawner; 
    public int startingEnemyCount = 3;
    public int winSpawnCount = 5;
    public float winSpawnPatrolWidth = 1.8f;


    public event Action OnWin;
    public event Action OnLose;

    public int Score { get; private set; }
    public bool IsGameOver { get; private set; }

    void Start()
    {
        Score = 0;
        IsGameOver = false;

        if (UIManager.Instance == null)
        {
            Debug.LogWarning("UIManager instance not found/created.");
        }

        UIManager.Instance?.UpdateScore(Score);

        fruitSpawner?.SpawnRandomFruits(10, new Vector2(-65, 1), new Vector2(65, 4));
        enemySpawner?.SpawnEnemies(startingEnemyCount);
    }

    void Update()
    {
        if (!IsGameOver)
        {
            var player = UnityEngine.GameObject.FindGameObjectWithTag("Player");
            if (player != null && player.transform.position.y < loseY)
            {
                Lose();
            }
        }
    }



    public void AddScore(int amount)
    {
        if (IsGameOver) return;
        Score += amount;

        // Always use singleton so we don't rely on inspector wiring
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateScore(Score);
        else
            Debug.LogWarning("AddScore: UIManager.Instance is null - score won't display.");

        if (Score >= targetScore)
        {
            Win();
        }
    }

    public void Win()
    {
        if (IsGameOver) return;
        IsGameOver = true;

        if (UIManager.Instance != null)
            UIManager.Instance.ShowWin();
        else
            Debug.LogWarning("Win: UIManager.Instance is null - can't show win message.");

        OnWin?.Invoke();
        Debug.Log("You win!");
    }

    public void Lose()
    {
        if (IsGameOver) return;
        IsGameOver = true;

        if (UIManager.Instance != null)
            UIManager.Instance.ShowLose();
        else
            Debug.LogWarning("Lose: UIManager.Instance is null - can't show lose message.");

        OnLose?.Invoke();
        Debug.Log("You lose!");
    }
}
