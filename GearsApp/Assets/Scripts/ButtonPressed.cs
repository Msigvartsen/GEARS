using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{
    public bool isActive = false;
    public Button button;

    private void Start()
    {
        //button.onClick.AddListener(toggleActive);
        this.enabled = isActive;
       
    }

    private void Update()
    {
        //this.enabled = isActive;
    }

    private void toggleActive()
    {
        isActive = !isActive;
        Debug.Log("Flipping button");

    }
}
