using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

class Movie : MonoBehaviour
{
#if UNITY_ANDROID
#else
    public MovieTexture texture;
#endif

    void Start()
    {
        //GetComponent<RawImage>().texture = texture;
        //texture.Play();
    }
}
