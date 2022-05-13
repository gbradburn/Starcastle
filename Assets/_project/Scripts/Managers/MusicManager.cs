using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private AudioSource _audioSource;

    public static MusicManager Instance;

    public enum MusicTracks : int
    {
        None = -1,
        TitleMusic = 0,
        PlayMusic = 1,
        GameOverMusic = 2
    }
    [SerializeField] List<AudioClip> _musicClips;

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
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(MusicTracks musicTrack)
    {
        switch (musicTrack)
        {
            case MusicTracks.None:
                _audioSource.Stop();
                break;
            default:
                int index = (int)musicTrack;
                if (index >= 0 && index < _musicClips.Count)
                {
                    _audioSource.clip = _musicClips[index];
                    _audioSource.Play();
                }
                break;
        }
    }
}
