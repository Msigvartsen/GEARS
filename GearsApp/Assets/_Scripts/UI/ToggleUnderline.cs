using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUnderline : MonoBehaviour
{
    public GameObject underline;

    private Toggle toggle;
    public Color selectedColor;
    public Color normalColor;
    private Image img;


    private void Start()
    {
        toggle = GetComponent<Toggle>();
        underline.SetActive(toggle.isOn);
        img = GetComponent<Image>();
        selectedColor = new Color(0.9882354f, 0.6039216f, 0.2313726f, 1);
        normalColor = new Color(0.8156863f, 0.3607843f, 0.01176471f, 1);
        UpdateColors(toggle.isOn);
    }

    public void ToggleActive()
    {
        underline.SetActive(toggle.isOn);
        img.color = selectedColor;
        UpdateColors(toggle.isOn); 
    }

    private void UpdateColors(bool isActive)
    {

        if (isActive)
        {
            img.color = selectedColor;
        }
        else
        {
            img.color = normalColor;
        }
    }
}
