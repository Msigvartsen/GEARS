using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Help
{
    SELECT,
    SEARCH,
    PLACE
};

public class HelpTextManager : MonoBehaviour
{
    private HelpTextManager instance;

    private GameObject helpPanel;
    private Text helpText;
    private CanvasGroup helpGroup;

    private float fadeSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        helpPanel = GameObject.FindGameObjectWithTag("HelpPanel");
        helpText = helpPanel.GetComponentInChildren<Text>();
        helpGroup = helpPanel.GetComponent<CanvasGroup>();
        helpGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHelpText(int message)
    {
        string messageText = "";
        switch (message)
        {
            case (int) Help.SELECT:
                messageText = "Select a model you want to look at";
                break;
            case (int) Help.SEARCH:
                messageText = "Look around for a flat surface";
                break;
            case (int) Help.PLACE:
                messageText = "Tap to place the model on the indicator";
                break;
            default:
                messageText = "Look here for tips and help";
                break;
        }

        helpText.text = messageText;
    }

    public void FadeOutHelpText()
    {
        if (helpGroup.alpha > 0)
            helpGroup.alpha -= Time.deltaTime * fadeSpeed;

    }

    public void FadeInHelpText()
    {
        if (helpGroup.alpha < 1)
            helpGroup.alpha += Time.deltaTime * fadeSpeed;
    }

    public HelpTextManager GetInstance()
    {
        return instance;
    }


}
