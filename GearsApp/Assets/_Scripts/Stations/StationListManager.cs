using UnityEngine;
using UnityEngine.UI;

public class StationListManager : MonoBehaviour
{
    private GameObject parent;
    private GameObject[] itemList;
    private Station[] stationArray;
    private LocationController locationController;
    private string prefabName;


    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        SetData();
    }

    /// <summary>
    /// Set instances and retrieve station models.
    /// </summary>
    private void SetData()
    {
        parent = transform.gameObject;
        locationController = LocationController.GetInstance();
        stationArray = StationController.GetInstance().StationList.ToArray();
        itemList = new GameObject[stationArray.Length];
        prefabName = "StationListItem";

        // Get the stations linked to current location
        for (int i = 0; i < stationArray.Length; i++)
        {
            if (stationArray[i].location_ID == locationController.CurrentLocation.location_ID)
            {
                itemList[i] = CreateButton(stationArray[i]);
            }
        }
    }

    /// <summary>
    /// Create button for station at selected location.
    /// </summary>
    /// <param name="station">Station connected to button.</param>
    /// <returns></returns>
    GameObject CreateButton(Station station)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(parent.transform, false);
        go.GetComponent<StationListItem>().Station = station;

        SetButtonValues(go);

        return go;
    }

    /// <summary>
    /// Update button values if the user has unlocked more stations.
    /// </summary>
    public void UpdateVisitedStations()
    {
        stationArray = StationController.GetInstance().StationList.ToArray();

        if (itemList != null)
        {
            for (int i = 0; i < stationArray.Length; i++)
            {
                if (stationArray[i].location_ID == locationController.CurrentLocation.location_ID)
                {
                    SetButtonValues(itemList[i]);
                }
            }
        }
    }

    /// <summary>
    /// Set correct button values based on user progression.
    /// </summary>
    /// <param name="ListItem">Button.</param>
    /// <param name="station">Station connected to button.</param>
    void SetButtonValues(GameObject ListItem)
    {
            ListItem.GetComponent<StationListItem>().UpdateStationStatus();
    }
}
