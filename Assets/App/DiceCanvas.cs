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
        if (!_done)
            return;

        gameObject.SetActive(false);
        _finished(_result);
        _finished = null;
    }

    public void Reset()
    {
        _done = false;
        Die.Reset();
    }

    public void RollDie(Action<int> cb)
    {
        Reset();
        gameObject.SetActive(true);
        _finished = cb;
        Die.Roll(Done);
    }

    void Done(int result)
    {
        _done = true;
        _result = result;
    }

    private Action<int> _finished;
    private int _result;
    private bool _done;
}
