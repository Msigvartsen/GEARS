using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUnderline : MonoBehaviour
{
    public GameObject underline;

    private Toggle toggle;
    private Color selectedColor;
    private Color normalColor;
    private ColorBlock cb;

    private void Start()
    {
        toggle = GetComponent<Toggle>();
        underline.SetActive(toggle.isOn);

        //cb = toggle.colors;
        //selectedColor = new Color(252, 154, 59, 255);
        //normalColor = new Color(208, 92, 3, 255);
        //UpdateColors(toggle.isOn);
    }
    
    public void ToggleActive()
    {
        underline.SetActive(toggle.isOn);
        //UpdateColors(toggle.isOn); 
    }

    private void UpdateColors(bool isActive)
    {

    //    if(isActive)
    //    {
    //        cb.normalColor = selectedColor;
    //        cb.highlightedColor = selectedColor;
    //        cb.pressedColor = selectedColor;
    //    }
    //    else
    //    {
    //        cb.normalColor = normalColor;
    //        cb.highlightedColor = normalColor;
    //        cb.pressedColor = normalColor;
    //    }

    //    toggle.colors = cb;
    }
}
