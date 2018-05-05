using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

class ScoreBoard : MonoBehaviour
{
    public int Score;
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
        Score += 1;
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
        Score -= 1;
        UpdateScore();
        PlayRandom(MinusClips);
    }

    void UpdateScore()
    {
        ScoreText.Set(Score.ToString());
    }

    public void Reset()
    {
        Score = 20;
        UpdateScore();
    }
}
