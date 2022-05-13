using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameStateChanged.AddListener(OnGameStateChanged);
        GameManager.Instance.RestartLevel();
    }

    private void OnGameStateChanged(GameManager.GameStates gameState)
    {
        switch (gameState)
        {
            case GameManager.GameStates.GetReady:
                StartCountdown();
                break;
            default:
                break;
        }
    }

    private void StartCountdown()
    {
        Invoke(nameof(StartPlaying), 3f);
    }

    private void StartPlaying()
    {
        GameManager.Instance.StartPlaying();
    }
}
