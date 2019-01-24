using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUnderline : MonoBehaviour
{
    public GameObject underline;
    private bool isActive = false;

    private void Start()
    {
        underline.SetActive(isActive);
    }

    public void ToggleActive()
    {
        isActive = !isActive;
        underline.SetActive(isActive);
    }
}
