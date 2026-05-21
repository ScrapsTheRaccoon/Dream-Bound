using UnityEngine;
using TMPro;

public class ScoreControl : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    public int Score { get; private set; }

    private void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        scoreText.text = "SCORE: " + Score;
    }
}
