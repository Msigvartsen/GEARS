using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the creation and updates for the List of locations in Unity.
/// Creates a list of prefabs with correct Location information.
/// </summary>
public class LocationListManager : MonoBehaviour
{
    private GameObject parent;
    private List<GameObject> itemList = new List<GameObject>();
    private Location[] locationArray;
    private string prefabName;
    private bool isInitiated = false;
    public bool DisplayFavoriteOnly { get; set; }

    /// <summary>
    /// Initializes the variables with correct values, and retreives location list.
    /// </summary>
    private void Init()
    {
        parent = transform.gameObject;
        locationArray = LocationController.GetInstance().LocationList.ToArray();
        prefabName = "LocationListItem";
        isInitiated = true;
        DisplayFavoriteOnly = false;
    }

    /// <summary>
    /// Ran at creation.
    /// Initializes and updates list of locations. 
    /// </summary>
    private void Start()
    {
        if (!isInitiated)
        {
            Init();
            CreateLocationGameObjectList();
        }
    }

    /// <summary>
    /// Toggle to show only favorites or all locations. 
    /// </summary>
    public void ToggleFavoriteList()
    {
        DisplayFavoriteOnly = !DisplayFavoriteOnly;

        if (DisplayFavoriteOnly)
        {
            SetOnlyFavoriteLocationsVisible();
        }
        else
        {
            SetAllLocationsVisible();
        }
    }

    /// <summary>
    /// Loops through item list and hides all objects that are not favorites.
    /// </summary>
    private void SetOnlyFavoriteLocationsVisible()
    {
        foreach (var item in itemList)
        {
            if (item.GetComponent<LocationListItem>().Location.favorite)
            {
                item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Loops through item list. Sets all objects to visible.
    /// </summary>
    private void SetAllLocationsVisible()
    {
        foreach (var item in itemList)
        {
            item.SetActive(true);
        }
    }

    /// <summary>
    /// Sort out a list of favorite locations from list of all locations.
    /// </summary>
    /// <returns>Returns List<Location> with favorites.</returns>
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

    /// <summary>
    /// Creates a list with LocationListItem prefab containing correct location information.
    /// </summary>
    private void CreateLocationGameObjectList()
    {
        for (int i = 0; i < locationArray.Length; i++)
        {
            itemList.Add(CreateListItem(locationArray[i]));
        }
    }

    /// <summary>
    /// Creates a prefab with current location information.
    /// </summary>
    /// <param name="location">Location to set in ListItem</param>
    /// <returns>Returns Prefab GameObject</returns>
    private GameObject CreateListItem(Location location)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(parent.transform, false);
        go.GetComponent<LocationListItem>().Location = location;
        return go;
    }
}
