using UnityEngine;
using TMPro;

public class CoinBoard : MonoBehaviour
{
    private int coinCount = 0;
    public TextMeshProUGUI scoreText; // Changed from TextMeshPro to TextMeshProUGUI

    void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddCoin()
    {
        coinCount++;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Coins: " + coinCount.ToString();
        }
    }
}