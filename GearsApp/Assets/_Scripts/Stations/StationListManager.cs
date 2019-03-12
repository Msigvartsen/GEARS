using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        return go;
    }
}
