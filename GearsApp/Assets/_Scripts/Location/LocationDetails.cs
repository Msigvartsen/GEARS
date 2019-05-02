using UnityEngine;

public class LocationDetails : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private TMPro.TextMeshProUGUI locationNameText;
    [SerializeField]
    private TMPro.TextMeshProUGUI infoText;
    [SerializeField]
    public Texture2D[] imagePanel; //Update ImagePanel with correct images from location
    private Location currentLocation;

    private void Start()
    {
        currentLocation = LocationController.GetInstance().CurrentLocation;
        if (currentLocation != null)
        {
            locationNameText.text = currentLocation.name;
            UpdateInfoText();
        }
    }

    private void UpdateInfoText()
    {
        LocationController manager = LocationController.GetInstance();
        var infotext = Resources.Load<TextAsset>("_Locations/" + manager.CurrentLocation.name + "/Information/basicinfo");
        infoText.text = infotext.text;
    }
}
