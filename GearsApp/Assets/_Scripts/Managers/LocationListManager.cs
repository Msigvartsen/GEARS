using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListManager : MonoBehaviour
{
    public enum ArrayType{ Locations, Favorites};
    public ArrayType arrayType;
    private GameObject parent;
    private GameObject[] itemList;
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

        if(arrayType == ArrayType.Locations)
            locationArray = LocationsManager.GetInstance().locationList.ToArray();
        if (arrayType == ArrayType.Favorites)
            locationArray = LocationsManager.GetInstance().favoriteLocationList.ToArray();

        prefabName = "ListItem";
    }

    private void UpdateLocationList()
    {
        itemList = new GameObject[locationArray.Length];

        for (int i = 0; i < locationArray.Length; i++)
        {
            itemList[i] = GetListItem(i);
        }
    }

    private GameObject GetListItem(int index)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(parent.transform, false);
        go.GetComponent<LocationListItem>().location = locationArray[index];

        return go;
    }
    private void OpenLocationTab()
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/LocationCanvas"));
    }
}
