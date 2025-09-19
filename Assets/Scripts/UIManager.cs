using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("UI Elements (TextMeshPro)")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    void Start()
    {
        if (scoreText) scoreText.text = "Score: 0";
        if (winText) winText.gameObject.SetActive(false);
        if (loseText) loseText.gameObject.SetActive(false);

        Debug.Log($"UIManager.Start(): scoreText={(scoreText != null)}, winText={(winText != null)}, loseText={(loseText != null)}");
    }

    public void UpdateScore(int score)
    {
        if (scoreText)
        {
            scoreText.text = $"Score: {score}";
        }
    }

    public void ShowWin()
    {
        Debug.Log("UIManager.ShowWin()");
        if (winText)
        {
            winText.gameObject.SetActive(true);
            winText.text = "YOU WIN!";
        }
        if (loseText) loseText.gameObject.SetActive(false);
    }

    public void ShowLose()
    {
        Debug.Log("UIManager.ShowLose()");
        if (loseText)
        {
            loseText.gameObject.SetActive(true);
            loseText.text = "YOU LOSE!";
        }
        if (winText) winText.gameObject.SetActive(false);
    }
}