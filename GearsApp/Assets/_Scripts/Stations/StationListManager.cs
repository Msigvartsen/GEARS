﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationListManager : MonoBehaviour
{
    private GameObject parent;
    private GameObject[] itemList;
    private Station[] stationArray;
    LocationController locationController;
    private string prefabName;

    // Start is called before the first frame update
    void Start()
    {
        SetData();
    }

    private void SetData()
    {
        parent = transform.gameObject;
        locationController = LocationController.GetInstance();
        stationArray = StationController.GetInstance().stationList.ToArray();
        itemList = new GameObject[stationArray.Length];
        prefabName = "StationListItem";

        for (int i = 0; i < stationArray.Length; i++)
        {
            if (stationArray[i].location_ID == locationController.CurrentLocation.location_ID)
            {
                itemList[i] = GetListItem(i, stationArray[i]);
            }
        }
    }

    GameObject GetListItem(int index, Station station)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(parent.transform, false);
        go.GetComponent<StationListItem>().station = station;

        SetButtonValues(go, station);

        return go;
    }

    public void UpdateVisitedStations()
    {
        //stationArray = StationController.GetInstance().stationList.ToArray();

        //if (itemList != null)
        //{
        //    for (int i = 0; i < stationArray.Length; i++)
        //    {
        //        if (stationArray[i].location_ID == locationController.CurrentLocation.location_ID)
        //        {
        //            Debug.Log("UpdateVisitedStations() -> stationArray[" + i + "] = locId" + stationArray[i].location_ID + " //// stationNr = " + stationArray[i].station_NR + " //// visited = " + stationArray[i].visited);
        //            itemList[i].GetComponent<StationListItem>().station = stationArray[i];
        //            SetButtonValues(itemList[i], stationArray[i]);
        //        }
        //    }
        //}
    }

    void SetButtonValues(GameObject ListItem, Station station)
    {
        Toggle visitedToggle = ListItem.GetComponentInChildren<Toggle>();
        visitedToggle.isOn = ListItem.GetComponentInChildren<StationListItem>().station.visited;

        Debug.Log("Station nr = " + station.station_NR + " at location id = " + station.location_ID + ", visited = " + visitedToggle.isOn);

        if (!visitedToggle.isOn)
        {
            visitedToggle.GetComponent<Image>().enabled = false;
            ListItem.GetComponentInChildren<CanvasGroup>().alpha = 0.5f;
            ListItem.GetComponentInChildren<Button>().enabled = false;
            visitedToggle.GetComponentInChildren<Text>().text = "Locked";
            visitedToggle.GetComponentInChildren<Text>().transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            visitedToggle.GetComponent<Image>().enabled = true;
            ListItem.GetComponentInChildren<CanvasGroup>().alpha = 1;
            ListItem.GetComponentInChildren<Button>().enabled = true;
            visitedToggle.GetComponentInChildren<Text>().text = "Unlocked";
            visitedToggle.GetComponentInChildren<Text>().transform.localPosition = new Vector3(0, -100, 0);
        }
    }
}
