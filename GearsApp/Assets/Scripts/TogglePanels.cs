using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePanels : MonoBehaviour
{
    [SerializeField]
    private GameObject[] panels;

    public void ToggleUIPanel()
    {
        foreach(GameObject panel in panels)
        {
            if(panel.activeSelf)
            {
                panel.SetActive(false);
            }
            else
            {
                panel.SetActive(true);
            }
        }
    }
}
