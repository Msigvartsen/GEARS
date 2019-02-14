using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListManager : MonoBehaviour
{
    private GameObject parent;
    private GameObject[] locationList;
    private Location[] locationArray;
    private string prefabName;

    private void Start()
    {
        Init();
        UpdateLocationList();

    }

    private void Init()
    {
        parent = transform.gameObject;
        locationArray = LocationsManager.GetInstance().locationList.ToArray();
        prefabName = "ListItem";
    }

    private void UpdateLocationList()
    {
        locationList = new GameObject[locationArray.Length];

        for (int i = 0; i < locationArray.Length; i++)
        {
            locationList[i] = GetListItem(i);
        }
    }

    private GameObject GetListItem(int index)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.parent = parent.transform;
        go.GetComponent<LocationListItem>().location = locationArray[index];

        return go;
    }
    private void OpenLocationTab()
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/LocationCanvas"));
    }
}
