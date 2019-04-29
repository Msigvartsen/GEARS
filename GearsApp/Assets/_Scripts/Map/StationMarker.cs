﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationMarker : MonoBehaviour
{

    public Station StationMarkerStation { get; set; }

    private Location coherentLocation;

    private void Start()
    {
        for (int i = 0; i < LocationController.GetInstance().locationList.Count; i++)
        {
            if (LocationController.GetInstance().locationList[i].location_ID == StationMarkerStation.location_ID)
            {
                coherentLocation = LocationController.GetInstance().locationList[i];
                break;
            }
        }
    }

    public void UpdateData()
    {
        if (StationMarkerStation != null)
        {
            GetComponentInChildren<TextMesh>().text = StationMarkerStation.helptext;
        }
    }

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
            button.onClick.AddListener(OpenLocationTab);
        }
    }

    void OpenLocationTab()
    {
        LocationController manager = LocationController.GetInstance();
        manager.CurrentLocation = coherentLocation;
        UserController.GetInstance().PreviousPage = "Locations";
        LoadingScreen.LoadScene("LocationNew");
    }
}