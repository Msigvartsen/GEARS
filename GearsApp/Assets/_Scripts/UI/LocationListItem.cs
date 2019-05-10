using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// LocationListItem is a script meant to be attached to LocationListItem prefab in Unity.
/// Delegates correct location information to the object (Text, images).
/// </summary>
public class LocationListItem : MonoBehaviour
{
    public Location Location { get; set; }
    [SerializeField]
    private GameObject imagePanel;
    [SerializeField]
    private TMPro.TextMeshProUGUI placeName;
    [SerializeField]
    private TMPro.TextMeshProUGUI lengthToLocation;

    /// <summary>
    /// Runs on creation.
    /// </summary>
    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Updates Each Frame.
    /// </summary>
    private void Update()
    {
        if (lengthToLocation != null)
            lengthToLocation.text = DistanceBetweenUserAndLocation();
    }

    /// <summary>
    /// Initialize on start. Set up correct name, length to location and thumbnails for the different locations.
    /// </summary>
    private void Init()
    {
        string length = DistanceBetweenUserAndLocation();

        placeName.text = Location.name;
        lengthToLocation.text = length;

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            if (go.name == "ThumbnailMask")
            {
                GameObject child = go.transform.GetChild(0).gameObject;
                if (child.name == "Thumbnail")
                {
                    child.GetComponent<RawImage>().texture = Location.thumbnail;
                }
            }
        }
    }

    /// <summary>
    /// Distance between Location Position (Latitude / Longitude) and  User position is calculated. 
    /// </summary>
    /// <returns>Returns string message with the distance between user and location.</returns>
    private string DistanceBetweenUserAndLocation()
    {
        float locationLat = (float)gameObject.GetComponent<LocationListItem>().Location.latitude;
        float locationLong = (float)gameObject.GetComponent<LocationListItem>().Location.longitude;

        string length = string.Empty;
        if (LocationServiceNS.LocationService.IsLocationServiceRunning())
        {
            if (CalculateDistance(locationLat, locationLong) < 5)
            {
                length = "< 5 km";
            }
            else
            {
                length = CalculateDistance(locationLat, locationLong).ToString() + " km";
            }
        }
        else
        {
            length = "Turn on Location Service";
        }

        return length;
    }

    /// <summary>
    /// Function is set on LocationListItem button. Opens Location Scene with correct location information.
    /// </summary>
    public void OpenLocationTab()
    {
        LocationController manager = LocationController.GetInstance();
        manager.CurrentLocation = Location;
        UserController.GetInstance().PreviousPage = "Locations";
        LoadingScreen.LoadScene("LocationNew");
    }

    /// <summary>
    /// Calculates distance between user and location
    /// </summary>
    /// <param name="locationLat"></param>
    /// <param name="locationLong"></param>
    /// <returns>Returns a double with total distance between user and location</returns>
    private double CalculateDistance(float locationLat, float locationLong)
    {
        float userLat = Input.location.lastData.latitude;
        float userLong = Input.location.lastData.longitude;
        int R = 6371;
        var lat_rad_1 = Mathf.Deg2Rad * userLat;
        var lat_rad_2 = Mathf.Deg2Rad * locationLat;
        var d_lat_rad = Mathf.Deg2Rad * (locationLat - userLat);
        var d_long_rad = Mathf.Deg2Rad * (locationLong - userLong);
        var a = Mathf.Pow(Mathf.Sin(d_lat_rad / 2), 2) + (Mathf.Pow(Mathf.Sin(d_long_rad / 2), 2) * Mathf.Cos(lat_rad_1) * Mathf.Cos(lat_rad_2));
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        var total_dist = R * c;

        // Round to closest 0.5 km
        if (total_dist - (int)total_dist > 0.74f)
            total_dist = (int)total_dist + 1;
        else if (total_dist - (int)total_dist > 0.24f)
            total_dist = (int)total_dist + 0.5f;
        else
            total_dist = (int)total_dist;

        return total_dist;
    }
}
