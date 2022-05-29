using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public UnityEvent ScoreUpdatedEvent;
    public static ScoreManager Instance;

    public int Score { get; private set; }
    public int HighScore { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            ScoreUpdatedEvent = new UnityEvent();
        }
    }

    private void OnEnable()
    {
        ResetScore();
    }

    public void AddScore(int points)
    {
        Score += points;
        if (Score > HighScore)
        {
            HighScore = Score;
        }
        ScoreUpdatedEvent.Invoke();

    }

    public void ResetScore()
    {
        Score = 0;
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        ScoreUpdatedEvent.Invoke();
    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", HighScore);
        PlayerPrefs.Save();
    }

}
