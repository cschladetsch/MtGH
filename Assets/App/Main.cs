using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main controller of the game state changes
/// </summary>
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
        // you really don't want to screen to dim or the app to suspend
        Screen.sleepTimeout = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // turn off all other canvases
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

        // TODO: allow for more than two players
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

    // we don't really care about result of rolls yet. just for player's benefit
    private void RollFinished(int val)
    {
        PlayMenuSound();
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
