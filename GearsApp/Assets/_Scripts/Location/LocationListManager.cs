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
    public bool displayFavoriteOnly = false;

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

    public void ToggleFavoriteList()
    {
        displayFavoriteOnly = !displayFavoriteOnly;

        if (displayFavoriteOnly)
        {
            foreach (var item in itemList)
            {
                if (item.GetComponent<LocationListItem>().location.favorite)
                {
                    item.SetActive(true);
                }
                else
                {
                    item.SetActive(false);
                }
            }

        }
        else
        {
            foreach (var item in itemList)
            {
                item.SetActive(true);
            }
        }
    }


    public void UpdateFavorites()
    {
        foreach (var item in itemList)
        {
            if (arrayType == ArrayType.Favorites)
            {
                item.SetActive(item.GetComponent<LocationListItem>().location.favorite);
            }
        }
        List<Location> list = GetFavoriteList();

        Location[] locations = GetFavoriteList().ToArray();
        bool isActive = false;
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
            else
            {

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

        for (int i = 0; i < locations.Length; i++)
        {
            Debug.Log(i + " location " + locations[i].name);
            itemList.Add(GetListItem(i, locations));
        }
    }

    private GameObject GetListItem(int index, Location[] locations)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(parent.transform, false);
        go.GetComponent<LocationListItem>().location = locations[index];
        if (arrayType == ArrayType.Favorites)
        {
            go.SetActive(go.GetComponent<LocationListItem>().location.favorite);
        }
        Debug.Log("Location in new prefab = " + locations[index].name);

        return go;
    }
}
