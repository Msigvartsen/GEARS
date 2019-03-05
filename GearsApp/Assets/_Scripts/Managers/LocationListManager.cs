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
        Location[] locations = new Location[0];
    
        if(arrayType == ArrayType.Favorites)
        {
            List<Location> list = new List<Location>();
            for (int i = 0; i < locationArray.Length; i++)
            {
                if(locationArray[i].favorite == true)
                {
                    Debug.Log("Adding to Favlist: " + locationArray[i].name);
                    list.Add(locationArray[i]);
                }
            }
            locations = list.ToArray();
        }
        else
        {
            locations = locationArray;
        }

        itemList = new GameObject[locations.Length];

        for (int i = 0; i < locations.Length; i++)
        {
            itemList[i] = GetListItem(i, locations);
        }
    }

    private GameObject GetListItem(int index, Location[] locations)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(parent.transform, false);
        go.GetComponent<LocationListItem>().location = locations[index];
        if(arrayType == ArrayType.Favorites)
        {
            Debug.Log("Favorite List Item index: " + index + locations[index].name);
        }
        GameObject favoriteButton = GameObject.FindGameObjectWithTag("FavoriteButton");
        favoriteButton.GetComponent<Toggle>().isOn = locations[index].favorite;

        return go;
    }
}
