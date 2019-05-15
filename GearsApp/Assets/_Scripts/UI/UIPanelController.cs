using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to switch between UI Pages with animations.
/// </summary>
public class UIPanelController : MonoBehaviour
{
    //All panels/pages to switch between.
    [Header("PANEL LIST")]
    public List<GameObject> panelList = new List<GameObject>();

    [Header("TITLE")]
    public TMPro.TextMeshProUGUI topTitleText;

    private string panelFadeIn = "PanelIn";
    private string panelFadeOut = "PanelOut";

    private GameObject previousPanel;
    private GameObject currentPanel;
    private GameObject nextPanel;

    [Header("SETTINGS")]
    public int currentPanelIndex = 0;
    [SerializeField]
    private bool updateOnStart = false;
    [SerializeField]
    private GameObject headerBackButton;

    private Animator currentPanelAnimator;
    private Animator nextPanelAnimator;

    /// <summary>
    /// Is called before the first frame update
    /// Starts correct page on each scene, and sets up name and panels.
    /// </summary>
    void Start()
    {
        if (updateOnStart)
        {
            PanelAnim(UserController.GetInstance().PreviousPage);
        }

        currentPanel = panelList[currentPanelIndex];
        nextPanel = currentPanel;
        currentPanelAnimator = currentPanel.GetComponent<Animator>();
        currentPanelAnimator.Play(panelFadeIn);
        ChangeTopTitle(currentPanel.name);
    }

    /// <summary>
    /// Run each frame. Updates visibility for the Header back button.
    /// </summary>
    private void Update()
    {
        UpdateMainBackButton();
    }

    /// <summary>
    /// Checks whether the backbutton on the header should be visible. 
    /// Not visible if current page is Main.
    /// </summary>
    private void UpdateMainBackButton()
    {
        if (headerBackButton == null || nextPanel == null)
            return;

        if (nextPanel.name == "Main")
            headerBackButton.SetActive(false);
        else
            headerBackButton.SetActive(true);
    }

    /// <summary>
    /// Renames top title of the header.
    /// </summary>
    /// <param name="newTitle">New title of the header</param>
    public void ChangeTopTitle(string newTitle)
    {
        if (topTitleText != null)
            topTitleText.text = newTitle;
    }

    /// <summary>
    /// Set up panels and switch to the next panel.
    /// Animation is played between the two panels.
    /// </summary>
    /// <param name="newPanel">new panel index</param>
    public void PanelAnim(int newPanel)
    {
        if (newPanel != currentPanelIndex)
        {
            currentPanel = panelList[currentPanelIndex];
            previousPanel = currentPanel;
            currentPanelIndex = newPanel;
            nextPanel = panelList[currentPanelIndex];

            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            nextPanelAnimator = nextPanel.GetComponent<Animator>();

            currentPanelAnimator.Play(panelFadeOut);
            nextPanelAnimator.Play(panelFadeIn);


            ChangeTopTitle(nextPanel.name);
        }
    }

    /// <summary>
    /// Set up panels and switch to the next panel.
    /// Animation is played between the two panels.
    /// Set new top title in header.
    /// </summary>
    /// <param name="newPanel"></param>
    public void PanelAnim(string newPanel)
    {
        int index = GetPanelIndexByName(newPanel);
        if (index != currentPanelIndex)
        {
            currentPanel = panelList[currentPanelIndex];
            previousPanel = currentPanel;

            currentPanelIndex = index;
            nextPanel = panelList[currentPanelIndex];

            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            nextPanelAnimator = nextPanel.GetComponent<Animator>();

            currentPanelAnimator.Play(panelFadeOut);
            nextPanelAnimator.Play(panelFadeIn);

            ChangeTopTitle(nextPanel.name);
        }
    }

    /// <summary>
    /// Go back to previous panel.
    /// </summary>
    public void PanelAnimPreviousPanel()
    {
        PanelAnim(previousPanel.name);
    }

    /// <summary>
    /// Get the current active panel.
    /// </summary>
    /// <returns>REturns the current panel.</returns>
    private GameObject GetCurrentPanel()
    {
        return panelList[currentPanelIndex];
    }

    /// <summary>
    /// Get the index of the new panel.
    /// </summary>
    /// <param name="newPanel">Returns index of new panel</param>
    /// <returns></returns>
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

