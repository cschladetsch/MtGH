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
    public AudioClip MusicClip;
    public AudioClip MenuClip;
    public AudioClip IntroClip;

    private AudioSource _audioSource;

    void Start()
    {
        Screen.sleepTimeout = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        ScoresPanel.SetActive(true);
        MenuPanel.SetActive(false);
        DieCanvas.gameObject.SetActive(false);

        _audioSource = GetComponent<AudioSource>();
        _audioSource.PlayOneShot(IntroClip);

        _audioSource.loop = true;
        _audioSource.clip = MusicClip;
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
        _audioSource.PlayOneShot(MenuClip);
    }

    public void RestartPressed()
    {
        PlayMenuSound();
        Score1.Reset();
        Score2.Reset();
        CloseMenu();
        _audioSource.PlayOneShot(IntroClip);
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
