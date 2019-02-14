using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListItem : MonoBehaviour
{
    private GameObject parent;
    private GameObject[] list;
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
        list = new GameObject[locationArray.Length];

        for (int i = 0; i < locationArray.Length; i++)
        {
            list[i] = GetListItem(i);
        }
    }

    private GameObject GetListItem(int index)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.parent = parent.transform;

        go.GetComponentInChildren<Text>().text = locationArray[index].name;

        return go;
    }
}
