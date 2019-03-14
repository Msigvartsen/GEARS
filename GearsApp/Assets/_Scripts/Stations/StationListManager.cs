using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationListManager : MonoBehaviour
{
    private GameObject parent;
    private GameObject[] itemList;
    private Station[] stationArray;
    private string prefabName;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.gameObject;
        stationArray = StationController.GetInstance().stationList.ToArray();
        prefabName = "StationListItem";

        UpdateStationList();
    }

    void UpdateStationList()
    {
        LocationController locationController = LocationController.GetInstance();
        Location[] locations = locationController.locationList.ToArray();

        itemList = new GameObject[stationArray.Length];
        for (int i = 0; i < stationArray.Length; i++)
        {
            if (stationArray[i].location_ID == locationController.CurrentLocation.location_ID)
            {
                itemList[i] = GetListItem(i);
            }
        }
    }

    GameObject GetListItem(int index)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(parent.transform, false);
        go.GetComponent<StationListItem>().station = stationArray[index];
        Toggle visitedToggle = go.GetComponentInChildren<Toggle>();
        visitedToggle.isOn = stationArray[index].visited;

        print(stationArray[index].visited);

        if (!visitedToggle.GetComponent<Toggle>().isOn)
        {
            print("Station " + stationArray[index].station_NR + " in location " + stationArray[index].location_ID + " is turned off");
            visitedToggle.GetComponent<Image>().enabled = false;
            go.GetComponentInChildren<CanvasGroup>().alpha = 0.5f;
            go.GetComponentInChildren<Button>().enabled = false;
            visitedToggle.GetComponentInChildren<Text>().text = "Locked";
            visitedToggle.GetComponentInChildren<Text>().transform.localPosition = new Vector3(0,0,0);
        }
        else
        {
            print("Station " + stationArray[index].station_NR + " in location " + stationArray[index].location_ID + " is turned on");
            visitedToggle.GetComponent<Image>().enabled = true;
            go.GetComponentInChildren<CanvasGroup>().alpha = 1;
            go.GetComponentInChildren<Button>().enabled = true;
            visitedToggle.GetComponentInChildren<Text>().text = "Unlocked";
            visitedToggle.GetComponentInChildren<Text>().transform.localPosition = new Vector3(0, -100, 0);
        }

        return go;
    }
}
