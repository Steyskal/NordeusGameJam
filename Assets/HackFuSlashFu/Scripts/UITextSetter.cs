using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Happy;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UITextSetter : MonoBehaviour
{
    private Text _text;

    void Awake()
    {
        _text = GetComponent<Text>();
    }

    public void SetText(int value)
    {
        _text.text = value.ToString();
    }
    public void SetText(float value)
    {
        _text.text = value.ToString();
    }
}
