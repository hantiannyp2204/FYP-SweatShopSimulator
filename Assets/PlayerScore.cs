using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    float playerScore;
    [SerializeField] TMP_Text PlayerScoreText;
    public void Init()
    {
        playerScore= 0;
        UpdateScoreText();
    }
    public float GetScore() => playerScore;
    public void AddScore(float addedScore)
    {
        playerScore+=addedScore;
        UpdateScoreText();
    }
    void UpdateScoreText()
    {
        PlayerScoreText.text = "Score: " + playerScore.ToString();
    }
    private void OnEnable()
    {
        GameManager.OnScoreAdded += AddScore;
    }

    private void OnDisable()
    {
        GameManager.OnScoreAdded -= AddScore;
    }
}
