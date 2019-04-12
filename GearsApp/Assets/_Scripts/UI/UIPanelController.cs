﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelController : MonoBehaviour
{

    [Header("PANEL LIST")]
    public List<GameObject> panelList = new List<GameObject>();

    [Header("TITLE")]
    public Text topTitleText;

    // [Header("PANEL ANIMS")]
    private string panelFadeIn = "Demo Panel In";
    private string panelFadeOut = "Demo Panel Out";

    private GameObject currentPanel;
    private GameObject nextPanel;

    [Header("SETTINGS")]
    public int currentPanelIndex = 0;

    private Animator currentPanelAnimator;
    private Animator nextPanelAnimator;

    void Start()
    {
        currentPanel = panelList[currentPanelIndex];
        currentPanelAnimator = currentPanel.GetComponent<Animator>();
        currentPanelAnimator.Play(panelFadeIn);
    }

    public void ChangeTopTitle(string newTitle)
    {
        topTitleText.text = newTitle;
    }

    public void PanelAnim(int newPanel)
    {
        if (newPanel != currentPanelIndex)
        {
            currentPanel = panelList[currentPanelIndex];
            currentPanelIndex = newPanel;
            nextPanel = panelList[currentPanelIndex];

            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            nextPanelAnimator = nextPanel.GetComponent<Animator>();

            currentPanelAnimator.Play(panelFadeOut);
            nextPanelAnimator.Play(panelFadeIn);

            ChangeTopTitle(nextPanel.name);
        }
    }

    public void PanelAnim(string newPanel)
    {
        int index = GetPanelIndexByName(newPanel);
        if (index != currentPanelIndex)
        {
            currentPanel = panelList[currentPanelIndex];
            currentPanelIndex = index;
            nextPanel = panelList[currentPanelIndex];

            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            nextPanelAnimator = nextPanel.GetComponent<Animator>();

            currentPanelAnimator.Play(panelFadeOut);
            nextPanelAnimator.Play(panelFadeIn);

            ChangeTopTitle(nextPanel.name);

        }
    }

    private int GetPanelIndexByName(string newPanel)
    {
        for (int i = 0; i < panelList.Count; i++)
        {
            var panel = panelList[i];
            if (panel.name == newPanel)
            {
                return i;
            }
        }
        return 0;
    }
}
