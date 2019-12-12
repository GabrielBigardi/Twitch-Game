using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Viewer : MonoBehaviour
{
    public Text nameText;

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void ShowName()
    {
        nameText.enabled = true;
    }

    public void HideName()
    {
        nameText.enabled = false;
    }
}
