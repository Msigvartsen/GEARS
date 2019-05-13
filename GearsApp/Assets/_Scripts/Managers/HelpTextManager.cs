using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Help
{
    DISTANCE,
    STATION_PLACEMENT,
    LOCATION_DISTANCE
};

public class HelpTextManager : MonoBehaviour
{
    private HelpTextManager instance;
    [SerializeField]
    private GameObject backToMapButton;
    private TMPro.TextMeshProUGUI helpText;
    private Button helpButton;
    private CanvasGroup helpGroup;
    private string messageText = "";
    private string modelHelpText = "";
    private string stationHelpText = "";

    private float fadeSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (backToMapButton != null)
        {
            helpButton = backToMapButton.GetComponent<Button>();
            helpText = backToMapButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            helpButton.onClick.AddListener(OpenMap);
            helpGroup = backToMapButton.GetComponent<CanvasGroup>();
            helpGroup.alpha = 0;
        }

    }

    public void SetHelpText(int message)
    {
        switch (message)
        {
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

    public string GetModelHelpText()
    {
        modelHelpText = "1. Select a model you want to look at.\n2. Look around for a flat surface.\n3. Tap to place the model on the indicator";
        return modelHelpText;
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
        LoadingScreen.LoadScene(ConstantsNS.Constants.MainScene);
    }
}
