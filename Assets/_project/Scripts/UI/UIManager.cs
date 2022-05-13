using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _getReadyPanel;

    void Start()
    {
        UpdateUI(GameManager.Instance.GameState);
        GameManager.Instance.GameStateChanged.AddListener(OnGameStateChanged);
    }

    private void UpdateUI(GameManager.GameStates gameState)
    {
        _getReadyPanel.SetActive(gameState == GameManager.GameStates.GetReady);
    }

    private void OnGameStateChanged(GameManager.GameStates gameState)
    {
        UpdateUI(gameState);
    }

}
