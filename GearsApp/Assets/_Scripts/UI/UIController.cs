using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private static UIController _instance;

    public static UIController GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }



    [Header("PANEL LIST")]
    public List<GameObject> panelList = new List<GameObject>();

    [Header("BUTTON LIST")]
    public List<GameObject> buttonList = new List<GameObject>();

    [Header("TITLE")]
    public Text topTitleText;

    // [Header("PANEL ANIMS")]
    private string panelFadeIn = "Demo Panel In";
    private string panelFadeOut = "Demo Panel Out";

    // [Header("BUTTON ANIMS")]
    private string buttonFadeIn = "HB Hover to Pressed";
    private string buttonFadeOut = "HB Pressed to Normal";

    private GameObject currentPanel;
    private GameObject nextPanel;

    private GameObject currentButton;
    private GameObject nextButton;

    [Header("SETTINGS")]
    public int currentPanelIndex = 0;
    private int currentButtonlIndex = 0;

    private Animator currentPanelAnimator;
    private Animator nextPanelAnimator;

    private Animator currentButtonAnimator;
    private Animator nextButtonAnimator;

    void Start()
    {
        currentButton = buttonList[currentPanelIndex];
        currentButtonAnimator = currentButton.GetComponent<Animator>();
        currentButtonAnimator.Play(buttonFadeIn);

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
            Debug.Log("CurrentIndex: " + currentPanelIndex);
            currentPanelIndex = newPanel;
            nextPanel = panelList[currentPanelIndex];

            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            nextPanelAnimator = nextPanel.GetComponent<Animator>();

            currentPanelAnimator.Play(panelFadeOut);
            nextPanelAnimator.Play(panelFadeIn);

            currentButton = buttonList[currentButtonlIndex];

            currentButtonlIndex = newPanel;
            nextButton = buttonList[currentButtonlIndex];

            currentButtonAnimator = currentButton.GetComponent<Animator>();
            nextButtonAnimator = nextButton.GetComponent<Animator>();

            currentButtonAnimator.Play(buttonFadeOut);
            nextButtonAnimator.Play(buttonFadeIn);

            ChangeTopTitle(currentPanel.name);
            Debug.Log("CurrentIndex: " + currentPanelIndex);

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

            currentButton = buttonList[currentButtonlIndex];

            currentButtonlIndex = index;
            nextButton = buttonList[currentButtonlIndex];

            currentButtonAnimator = currentButton.GetComponent<Animator>();
            nextButtonAnimator = nextButton.GetComponent<Animator>();

            currentButtonAnimator.Play(buttonFadeOut);
            nextButtonAnimator.Play(buttonFadeIn);

            ChangeTopTitle(currentPanel.name);

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

