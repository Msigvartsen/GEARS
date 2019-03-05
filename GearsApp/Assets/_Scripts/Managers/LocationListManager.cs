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
        locationArray = LocationsManager.GetInstance().locationList.ToArray();
        prefabName = "ListItem";
    }

    private void UpdateLocationList()
    {
        Location[] locations = locationArray;
    
        if(arrayType == ArrayType.Favorites)
        {
            List<Location> list = new List<Location>();
            for (int i = 0; i < locationArray.Length; i++)
            {
                if(locationArray[i].favorite == true)
                {
                    list.Add(locationArray[i]);
                }
            }
            locations = list.ToArray();
        }

        itemList = new GameObject[locations.Length];

        for (int i = 0; i < locations.Length; i++)
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
