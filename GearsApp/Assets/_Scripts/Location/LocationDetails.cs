using UnityEngine;

/// <summary>
/// Script used on Location Scene to delegate information to components in Unity about the current location selected.
/// </summary>
public class LocationDetails : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private TMPro.TextMeshProUGUI locationNameText;
    [SerializeField]
    private TMPro.TextMeshProUGUI infoText;

    private Location currentLocation;

    /// <summary>
    /// Ran on creation of script. Sets up text from Location objects to Unity UI text components.
    /// </summary>
    private void Start()
    {
        currentLocation = LocationController.GetInstance().CurrentLocation;
        if (currentLocation != null)
        {
            locationNameText.text = currentLocation.name;
            UpdateInfoText();
        }
    }

    /// <summary>
    /// Loads text from resources.
    /// </summary>
    private void UpdateInfoText()
    {
        LocationController manager = LocationController.GetInstance();
        var infotext = Resources.Load<TextAsset>("_Locations/" + manager.CurrentLocation.name + "/Information/basicinfo");
        infoText.text = infotext.text;
    }
}
