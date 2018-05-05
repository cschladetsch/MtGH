using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

class Main : MonoBehaviour
{
    public GameObject ScoresPanel;
    public GameObject MenuPanel;
    public ScoreBoard Score1;
    public ScoreBoard Score2;
    public Text MusicText;
    public AudioClip Music;
    public AudioClip Menu;

    private AudioSource _audioSource;

    void Start()
    {
        Screen.sleepTimeout = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        ScoresPanel.SetActive(true);
        MenuPanel.SetActive(false);

        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.clip = Music;
        _audioSource.Play();
        UpdateMusicMenuText();
    }

    public void MenuPressed()
    {
        MenuPanel.SetActive(true);
        PlayMenuSound();
    }

    void PlayMenuSound()
    {
        _audioSource.PlayOneShot(Menu);
    }

    public void RestartPressed()
    {
        PlayMenuSound();
        Score1.Reset();
        Score2.Reset();
        CloseMenu();
    }

    void CloseMenu()
    {
        MenuPanel.SetActive(false);
    }

    public void RollPressed()
    {
        PlayMenuSound();
        var rand = UnityEngine.Random.Range(0, 6);
    }

    public void BackPressed()
    {
        PlayMenuSound();
        CloseMenu();
    }

    public void ToggleMusic()
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();
        else
            _audioSource.Play();

        UpdateMusicMenuText();
        PlayMenuSound();
    }

    void UpdateMusicMenuText()
    {
        if (_audioSource.isPlaying)
            MusicText.text = "Stop Music";
        else
            MusicText.text = "Play Music";
    }
}
