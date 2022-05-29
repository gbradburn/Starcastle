using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int Level { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameStateChanged.AddListener(OnGameStateChanged);
        GameManager.Instance.RestartLevel();
    }

    private void OnEnable()
    {
        Level = 0;
    }

    private void OnGameStateChanged(GameManager.GameStates gameState)
    {
        switch (gameState)
        {
            case GameManager.GameStates.GetReady:
                StartCountdown();
                break;
            case GameManager.GameStates.StarcastleDestroyed:
                ++Level;
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
