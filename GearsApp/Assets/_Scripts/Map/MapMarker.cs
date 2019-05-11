using UnityEngine;
using UnityEngine.UI;

public class MapMarker : MonoBehaviour
{
    public Location MapMarkerLocation { get; set; }

    /// <summary>
    /// Update marker display name.
    /// </summary>
    public void UpdateData()
    {
        if (MapMarkerLocation != null)
            GetComponentInChildren<TextMesh>().text = MapMarkerLocation.name;
        //Add other relevant info :)
    }

    /// <summary>
    /// Fade-in popup information about selected location.
    /// </summary>
    public void SpawnLocationPopupInfo()
    {
        GameObject popupWindow = GameObject.FindGameObjectWithTag("LocationPopupContainer");
        popupWindow.GetComponent<Animator>().Play("Fade-in");

        if (MapMarkerLocation != null)
        {
            GameObject popupInfo = GameObject.FindGameObjectWithTag("LocationPopupInfo");
            popupInfo.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = MapMarkerLocation.name;
            popupInfo.GetComponentInChildren<RawImage>().texture = MapMarkerLocation.thumbnail;

            Button button = popupInfo.GetComponentInChildren<Button>();
            button.onClick.AddListener(OpenLocationScene);
        }
    }

    /// <summary>
    /// Loads scene with selected location.
    /// </summary>
    void OpenLocationScene()
    {
        LocationController manager = LocationController.GetInstance();
        manager.CurrentLocation = MapMarkerLocation;
        UserController.GetInstance().PreviousPage = "Locations";
        LoadingScreen.LoadScene(ConstantsNS.Constants.LocationScene);
    }
}
