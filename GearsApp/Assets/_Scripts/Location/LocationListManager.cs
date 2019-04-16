using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListManager : MonoBehaviour
{
    public enum ArrayType { Locations, Favorites };
    public ArrayType arrayType;
    private GameObject parent;
    //private GameObject[] itemList;
    private List<GameObject> itemList = new List<GameObject>();
    private Location[] locationArray;
    private string prefabName;
    private bool isInitiated = false;

    private void Init()
    {
        parent = transform.gameObject;
        locationArray = LocationController.GetInstance().locationList.ToArray();
        prefabName = "LocationListItem";
        isInitiated = true;
    }

    private void Start()
    {
        if (!isInitiated)
        {
            Init();
            UpdateLocationList();
        }

    }
    public void UpdateFavorites()
    {
        //List<Location> list = GetFavoriteList();
        Debug.Log("UPDATE FAV LIST)");
        Location[] locations = GetFavoriteList().ToArray();
        bool isActive = false;
        Debug.Log("# Fav: " + locations.Length + " # items = " + itemList.Count);
        foreach (var fav in locations)
        {
            foreach (var item in itemList)
            {
                if (item.GetComponent<LocationListItem>().location.location_ID == fav.location_ID)
                {
                    isActive = true;
                }
            }
            if (!isActive)
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
                go.transform.SetParent(parent.transform, false);
                go.GetComponent<LocationListItem>().location = fav;
                itemList.Add(go);
                isActive = false;
            }
        }
    }

    private List<Location> GetFavoriteList()
    {
        List<Location> list = new List<Location>();
        for (int i = 0; i < locationArray.Length; i++)
        {
            if (locationArray[i].favorite == true)
            {
                Debug.Log("Adding to Favlist: " + locationArray[i].name);
                list.Add(locationArray[i]);
            }
        }

        return list;
    }

    private void UpdateLocationList()
    {
        Location[] locations = new Location[0];

        if (arrayType == ArrayType.Favorites)
        {
            locations = GetFavoriteList().ToArray();
        }
        else
        {
            locations = locationArray;
        }

        //itemList = new GameObject[locations.Length];
        for (int i = 0; i < locations.Length; i++)
        {
            Debug.Log(i + " location " + locations[i].name);
            //itemList[i] = GetListItem(i, locations);
            itemList.Add(GetListItem(i, locations));
        }
    }

    private GameObject GetListItem(int index, Location[] locations)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(parent.transform, false);
        go.GetComponent<LocationListItem>().location = locations[index];
        Debug.Log("Location in new prefab = " + locations[index].name);

        return go;
    }
}
