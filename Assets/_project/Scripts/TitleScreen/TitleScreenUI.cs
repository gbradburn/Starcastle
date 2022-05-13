using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenUI : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance?.PlayMusic(MusicManager.MusicTracks.TitleMusic);
    }

    public void PlayButtonClicked()
    {
        LoadMainScene();
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
