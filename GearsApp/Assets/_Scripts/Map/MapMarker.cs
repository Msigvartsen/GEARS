using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMarker : MonoBehaviour
{
    public Location MapMarkerLocation { get; set; }

    public void UpdateData()
    {
        if (MapMarkerLocation != null)
            GetComponentInChildren<TextMesh>().text = MapMarkerLocation.name;
        //Add other relevant info :)
    }

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
            button.onClick.AddListener(OpenLocationTab);
        }
    }

    void OpenLocationTab()
    {
        LocationController manager = LocationController.GetInstance();
        manager.CurrentLocation = MapMarkerLocation;
        UserController.GetInstance().PreviousPage = "Locations";
        LoadingScreen.LoadScene("LocationNew");
    }
}
