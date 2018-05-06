using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate int RollDieDelegate();

class DiceCanvas : MonoBehaviour
{
    public Main Main;
    public Die Die;

    public void BackgroundPressed()
    {
    }

    public void Reset()
    {
        Die.Reset();
    }

    public void RollDie(Action<int> cb)
    {
        gameObject.SetActive(true);
        _finished = cb;
        Die.Roll(Done);
    }

    void Done(int result)
    {
        gameObject.SetActive(false);
        _finished(result);
        _finished = null;
    }

    private Action<int> _finished;
}
