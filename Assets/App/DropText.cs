using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Text that has a dropshadow
/// </summary>
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
