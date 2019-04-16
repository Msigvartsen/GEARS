using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject newPanel;
    [SerializeField]
    private GameObject previousPanel;

    private void Start()
    {
        if (GetComponent<Toggle>().isOn)
            SetNewActivePanel();
    }

    public void SetNewActivePanel()
    {
        newPanel.SetActive(true);
        previousPanel.SetActive(false);
    }
}

