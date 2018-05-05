using UnityEngine;
using UnityEngine.UI;

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
