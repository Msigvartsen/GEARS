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
                itemList[i] = GetListItem(i);
        }
    }

    GameObject GetListItem(int index)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(parent.transform, false);
        go.GetComponent<StationListItem>().station = stationArray[index];
        GameObject visitedToggle = GameObject.FindGameObjectWithTag("VisitedToggle");
        visitedToggle.GetComponent<Toggle>().isOn = stationArray[index].visited;

        if (!visitedToggle.GetComponent<Toggle>().isOn)
        {
            visitedToggle.GetComponent<Image>().enabled = false;
            GetComponentInChildren<CanvasGroup>().alpha = 0.5f;
            GetComponentInChildren<Button>().enabled = false;
            visitedToggle.GetComponentInChildren<Text>().text = "Locked";
            visitedToggle.GetComponentInChildren<Text>().transform.localPosition = new Vector3(0,0,0);
        }
        else
        {
            visitedToggle.GetComponent<Image>().enabled = true;
            GetComponentInChildren<CanvasGroup>().alpha = 1;
            GetComponentInChildren<Button>().enabled = true;
            visitedToggle.GetComponentInChildren<Text>().text = "Unlocked";
            visitedToggle.GetComponentInChildren<Text>().transform.localPosition = new Vector3(0, -100, 0);
        }

        return go;
    }
}
