using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        GetReady,
        Playing,
        PlayerShipDestroyed,
        StarcastleDestroyed
    }

    [SerializeField] Vector3 _startPosition = new Vector3(20f, 0, 0);
    [SerializeField] GameObject _playerShipPrefab;
    [SerializeField] GameObject _starcastlePrefab;

    public static GameManager Instance;
    public GameStateChangedEvent GameStateChanged = new GameStateChangedEvent();

    private GameStates _gameState;
    public GameStates GameState
    {
        get
        {
            return _gameState;
        }
        set
        {
            _gameState = value;
            GameStateChanged.Invoke(GameState);
        }
    }
    public bool IsPlaying => GameState == GameStates.Playing;
    public int Lives { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void RestartLevel()
    {
        Lives = 3;
        GetReady();
    }

    public void GetReady()
    {
        MusicManager.Instance.PlayMusic(MusicManager.MusicTracks.None);
        SpawnPlayerShip();
        SpawnStarcastle();
        GameState = GameStates.GetReady;
    }


    public void StartPlaying()
    {
        MusicManager.Instance.PlayMusic(MusicManager.MusicTracks.PlayMusic);
        GameState = GameStates.Playing;
    }

    public void PlayerShipDestroyed()
    {
        GameState = GameStates.PlayerShipDestroyed;
        Lives--;
        if (Lives > 0)
        {
            GetReady();
        }
        else
        {
            Invoke(nameof(GameOver), 3f);
        }
    }

    public void StarcastleDestroyed()
    {
        GameState = GameStates.StarcastleDestroyed;
        Invoke(nameof(GetReady), 3f);
    }


    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    private void SpawnPlayerShip()
    {
        PlayerShip playerShip = FindObjectOfType<PlayerShip>(true);
        if (playerShip == null)
        {
            Instantiate(_playerShipPrefab);
        }
        else
        {
            playerShip.transform.position = _startPosition;
            playerShip.transform.rotation = Quaternion.Euler(0, 0, 90f);
            playerShip.gameObject.SetActive(true);
        }
    }

    private void SpawnStarcastle()
    {
        if (FindObjectOfType<Starcastle>() == null)
        {
            Instantiate(_starcastlePrefab);
        }
    }

}

public class GameStateChangedEvent : UnityEvent<GameManager.GameStates> { }
