using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Help
{
    SELECT,
    SEARCH,
    PLACE,
    DISTANCE,
    STATION_PLACEMENT,
    LOCATION_DISTANCE
};

public class HelpTextManager : MonoBehaviour
{
    private HelpTextManager instance;

    private GameObject helpPanel;
    private TMPro.TextMeshProUGUI helpText;
    private Button helpButton;
    private CanvasGroup helpGroup;

    private float fadeSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        helpPanel = GameObject.FindGameObjectWithTag("HelpPanel");
        if(helpPanel != null)
        {
            helpText = helpPanel.GetComponent<TMPro.TextMeshProUGUI>();
            helpButton = helpPanel.GetComponent<Button>();
            helpButton.onClick.AddListener(OpenMap);
            helpGroup = helpPanel.GetComponent<CanvasGroup>();
            helpGroup.alpha = 0;
        }

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
            case (int) Help.DISTANCE:
                messageText = "Too far away from nearest station, tap here to see the map";
                break;
            case (int) Help.STATION_PLACEMENT:
                messageText = "Look around and find *this* to get the best experience, then tap the screen";
                break;
            case (int) Help.LOCATION_DISTANCE:
                messageText = "Not close enough to selected location, tap here to see the map";
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

    public void EnableButton()
    {
        helpButton.interactable = true;
    }

    public void DisableButton()
    {
        helpButton.interactable = false;
    }

    public void OpenMap()
    {
        Debug.Log("BUtton pressed OPEN MAP");
        UserController.GetInstance().PreviousPage = "Locations";
        LoadingScreen.LoadScene(GEARSApp.Constants.MainScene);
    }
}
