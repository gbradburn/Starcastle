using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _highScoreText;
    [SerializeField] Transform _lives;
    [SerializeField] GameObject _playerLifePrefab;

    private void OnEnable()
    {
        ScoreManager.Instance.ScoreUpdatedEvent.AddListener(OnScoreChanged);
        GameManager.Instance.GameStateChanged.AddListener(OnGameStateChanged);
    }

    private void OnDisable()
    {
        ScoreManager.Instance.ScoreUpdatedEvent.RemoveListener(OnScoreChanged);
        GameManager.Instance.GameStateChanged.RemoveListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameManager.GameStates gameState)
    {
        switch (gameState)
        {
            case GameManager.GameStates.GetReady:
            case GameManager.GameStates.PlayerShipDestroyed:
                UpdateLivesDisplay();
                break;
        }
    }

    private void OnScoreChanged()
    {
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        _scoreText.text = ScoreManager.Instance.Score.ToString();
        _highScoreText.text = ScoreManager.Instance.HighScore.ToString();
    }

    private void UpdateLivesDisplay()
    {
        var playerLives = _lives.GetComponentsInChildren<PlayerLife>();
        foreach(var life in playerLives)
        {
            Destroy(life.gameObject);
        }
        int lives = GameManager.Instance.Lives;
        for (int i = 0; i < lives; ++i)
        {
            Instantiate(_playerLifePrefab, _lives);
        }
    }

}
