using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Enumerator used to display different help messages.
/// </summary>
public enum Help
{
    SELECT,
    SCANNING,
    PLACE,
    DISTANCE,
    STATION_PLACEMENT,
    LOCATION_DISTANCE
};

/// <summary>
/// Class that determines what type of help messages is displayed to the user regarding AR.
/// </summary>
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

    /// <summary>
    /// Called the first frame.
    /// Used to set instances and values.
    /// </summary>
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

    /// <summary>
    /// Sets different help messages based on in parameter.
    /// </summary>
    /// <param name="message">Uses a switch to determine which message is shown.</param>
    public void SetHelpText(int message)
    {
        switch (message)
        {
            case (int) Help.SELECT:
                messageText = "Please select a model.";
                break;
            case (int) Help.SCANNING:
                messageText = "Scanning for a flat surface...";
                break;
            case (int) Help.PLACE:
                messageText = "Tap to place the model.";
                break;
            case (int) Help.DISTANCE:
                messageText = "Too far away from nearest station, tap here to see the map.";
                break;
            case (int) Help.STATION_PLACEMENT:
                messageText = "Look around and find *this* to get the best experience, then tap the screen.";
                break;
            case (int) Help.LOCATION_DISTANCE:
                messageText = "Not close enough to selected location, tap here to see the map.";
                break;
            default:
                messageText = "Look here for tips and help";
                break;
        }

        helpText.text = messageText;
    }

    /// <summary>
    /// Retrieve tutorial message for model viewing.
    /// </summary>
    /// <returns>Returns a string with the help message.</returns>
    public string GetModelHelpText()
    {
        modelHelpText = "1. Select a model you want to look at.\n2. Look around for a flat surface.\n3. Tap to place the model on the indicator";
        return modelHelpText;
    }

    /// <summary>
    /// Fade out the help text.
    /// </summary>
    public void FadeOutHelpText()
    {
        if (helpGroup.alpha > 0)
            helpGroup.alpha -= Time.deltaTime * fadeSpeed;
    }

    /// <summary>
    /// Fade in the help text.
    /// </summary>
    public void FadeInHelpText()
    {
        if (helpGroup.alpha < 1)
            helpGroup.alpha += Time.deltaTime * fadeSpeed;
    }

    /// <summary>
    /// Retrieve this instance of the manager.
    /// </summary>
    /// <returns>Returns this.</returns>
    public HelpTextManager GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Enables the return-to-map button.
    /// </summary>
    public void EnableButton()
    {
        helpButton.interactable = true;
    }

    /// <summary>
    /// Disables the return-to-map button.
    /// </summary>
    public void DisableButton()
    {
        helpButton.interactable = false;
    }

    /// <summary>
    /// Opens the map view in another scene.
    /// </summary>
    public void OpenMap()
    {
        Debug.Log("BUtton pressed OPEN MAP");
        UserController.GetInstance().PreviousPage = "Locations";
        LoadingScreen.LoadScene(GEARSApp.Constants.MainScene);
    }
}
