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

    private string panelFadeIn = "Panel In";
    private string panelFadeOut = "Panel Out";

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

        currentPanel = panelList[0];
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

            currentButton = buttonList[currentButtonlIndex];

            currentButtonlIndex = newPanel;
            nextButton = buttonList[currentButtonlIndex];

            currentButtonAnimator = currentButton.GetComponent<Animator>();
            nextButtonAnimator = nextButton.GetComponent<Animator>();

            currentButtonAnimator.Play(buttonFadeOut);
            nextButtonAnimator.Play(buttonFadeIn);
        }
    }

    public void PanelAnim(string newPanel)
    {
        for (int i = 0; i < panelList.Count; i++)
        {
            var panel = panelList[i];

            if (panel.name == newPanel)
            {
                currentPanel = panel;

                currentPanelIndex = i;
                nextPanel = panelList[currentPanelIndex];

                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                nextPanelAnimator = nextPanel.GetComponent<Animator>();

                currentPanelAnimator.Play(panelFadeOut);
                nextPanelAnimator.Play(panelFadeIn);

                currentButton = buttonList[currentButtonlIndex];

                currentButtonlIndex = i;
                nextButton = buttonList[currentButtonlIndex];

                currentButtonAnimator = currentButton.GetComponent<Animator>();
                nextButtonAnimator = nextButton.GetComponent<Animator>();

                currentButtonAnimator.Play(buttonFadeOut);
                nextButtonAnimator.Play(buttonFadeIn);
            }
        }
    }
}

