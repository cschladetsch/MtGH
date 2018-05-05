using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

class DropText : MonoBehaviour
{
    public Text Text;
    public Text Shadow;

    public void Set(string text)
    {
        Text.text = text;
        Shadow.text = text;
    }
}
