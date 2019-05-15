using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that updates station map marker data.
/// </summary>
public class StationMarker : MonoBehaviour
{
    public Station StationMarkerStation { get; set; }
    private Location coherentLocation;

    private void Start()
    {
        for (int i = 0; i < LocationController.GetInstance().LocationList.Count; i++)
        {
            if (LocationController.GetInstance().LocationList[i].location_ID == StationMarkerStation.location_ID)
            {
                coherentLocation = LocationController.GetInstance().LocationList[i];
                break;
            }
        }
    }

    /// <summary>
    /// Fade-in popup panel with information about the selected location.
    /// </summary>
    public void SpawnStationPoputInfo()
    {
        GameObject popupWindow = GameObject.FindGameObjectWithTag("LocationPopupContainer");
        //popupWindow.GetComponent<PopupPanel>().SetPopupPanelActive(true);
        popupWindow.GetComponent<Animator>().Play("Fade-in");

        if (StationMarkerStation != null)
        {
            GameObject popupInfo = GameObject.FindGameObjectWithTag("LocationPopupInfo");
            popupInfo.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = coherentLocation.name + " " + StationMarkerStation.helptext;
            popupInfo.GetComponentInChildren<RawImage>().texture = coherentLocation.thumbnail;

            //Button button = popupInfo.GetComponent<Button>();
            Button button = popupInfo.GetComponentInChildren<Button>();
            button.onClick.AddListener(OpenLocationScene);
        }
    }

    /// <summary>
    /// Loads new scene where location is determined by the station selected.
    /// </summary>
    void OpenLocationScene()
    {
        LocationController manager = LocationController.GetInstance();
        manager.CurrentLocation = coherentLocation;
        UserController.GetInstance().PreviousPage = "Locations";
        LoadingScreen.LoadScene(GEARSApp.Constants.LocationScene);
    }
}
