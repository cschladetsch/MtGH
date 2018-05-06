using UnityEngine;
using UnityEngine.UI;

class Main : MonoBehaviour
{
    public GameObject ScoresPanel;
    public GameObject MenuPanel;
    public DiceCanvas DieCanvas;

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
        DieCanvas.gameObject.SetActive(false);

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
        CloseMenu();
        DieCanvas.gameObject.SetActive(true);
        DieCanvas.RollDie(RollFinished);
    }

    private void RollFinished(int val)
    {
        DieCanvas.gameObject.SetActive(false);
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
        MusicText.text = _audioSource.isPlaying ? "Stop Music" : "Play Music";
    }
}
