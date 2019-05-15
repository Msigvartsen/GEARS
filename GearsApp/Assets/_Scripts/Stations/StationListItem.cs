using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that handles button data connected to different stations.
/// </summary>
public class StationListItem : MonoBehaviour
{
    public Station Station { get; set; }

    private Button listButton;
    [Header("Image Panels")]
    [SerializeField]
    private RawImage lockImageGameObject;
    [SerializeField]
    private RawImage thumbnail;

    [Header("Textures")]
    [SerializeField]
    private Texture2D lockedImage;
    [SerializeField]
    private Texture2D unlockedImage;

    /// <summary>
    /// Called the first frame.
    /// Set thumbnail image and correct text to the button.
    /// </summary>
    void Start()
    {
        SetStationThumbnail();
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Station " + Station.station_NR.ToString();
        listButton = GetComponentInChildren<Button>();
        listButton.onClick.AddListener(OpenStationTab);
    }

    /// <summary>
    /// Opens a popup information page with more information about the selected station.
    /// </summary>
    void OpenStationTab()
    {
        StationController manager = StationController.GetInstance();
        manager.CurrentStation = Station;
        GameObject popupWindow = GameObject.FindGameObjectWithTag("StationPopupContainer");
        popupWindow.GetComponent<Animator>().Play("Fade-in");

        if (Station != null)
        {
            GameObject popupInfo = GameObject.FindGameObjectWithTag("StationPopupInfo");
            popupInfo.GetComponentInChildren<RawImage>().texture = thumbnail.texture;
            var textFields = popupInfo.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            foreach(var textField in textFields)
            {
                if(textField.name == "Name")
                    textField.text = "Station # " + Station.station_NR;
                if (textField.name == "Latitude")
                    textField.text = Station.latitude.ToString();
                if(textField.name == "Longitude")
                    textField.text = Station.longitude.ToString();
                if (textField.name == "Details")
                    textField.text = Station.helptext;
            }
            popupInfo.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Station # "+ Station.station_NR;
        }
    }

    /// <summary>
    /// Sets the thumbnail image based on the model at the station.
    /// </summary>
    private void SetStationThumbnail()
    {
        Texture2D img = ModelController.GetInstance().GetModelThumbnail(Station.model_ID);
        if (img != null)
            thumbnail.texture = img;
    }

    /// <summary>
    /// Updates button visuals based on whether or not the station is unlocked.
    /// </summary>
    public void UpdateStationStatus()
    {
        if (!Station.visited)
        {
            GetComponentInChildren<Button>().enabled = false;
            GetComponentInChildren<CanvasGroup>().alpha = 0.5f;
            GetComponentInChildren<Text>().text = "Locked";

            if (lockedImage != null)
                lockImageGameObject.texture = lockedImage;
        }
        else
        {
            GetComponentInChildren<Button>().enabled = true;
            GetComponentInChildren<CanvasGroup>().alpha = 1f;
            GetComponentInChildren<Text>().text = "Unlocked";

            if (unlockedImage != null)
                lockImageGameObject.texture = unlockedImage;
        }
    }
}
