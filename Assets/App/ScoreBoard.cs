using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// One or each player: shows their current health.
/// TODO: Add PlanesWalkers for each player via a tab system.
/// </summary>
class ScoreBoard : MonoBehaviour
{
    public int Health;
    public DropText ScoreText;
    public AudioClip[] PlusClips;
    public AudioClip[] MinusClips;
    private AudioSource _audioSource;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlusPressed()
    {
        Health += 1;
        UpdateScore();
        PlayRandom(PlusClips);
    }

    private void PlayRandom(IList<AudioClip> clips)
    {
        _audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.0f);
        _audioSource.PlayOneShot(SelectRandom(clips));
    }

    private static AudioClip SelectRandom(IList<AudioClip> clips)
    {
        if (clips.Count == 0)
            return null;
        return clips[UnityEngine.Random.Range(0, clips.Count)];
    }

    public void MinusPressed()
    {
        Health -= 1;
        UpdateScore();
        PlayRandom(MinusClips);
    }

    void UpdateScore()
    {
        ScoreText.Set(Health.ToString());
    }

    public void Reset()
    {
        Health = 20;
        UpdateScore();
    }
}
