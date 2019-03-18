using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class StationTargetController : MonoBehaviour
{
    private LocationController locationController;
    private StationController stationController;

    private Location location;
    private List<Station> allStationsAtLocation;
    private bool[] visitedStations;

    private GameObject[] targets;

    // Start is called before the first frame update
    void Start()
    {
        SetAllInstances();
        FindAllTargets();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForScannedTargets();
    }

    void SetAllInstances()
    {
        // Get controllers
        locationController = LocationController.GetInstance();
        stationController = StationController.GetInstance();

        if (locationController.CurrentLocation != null)
        {
            location = locationController.CurrentLocation;
        }

        // Find all stations at the current location
        allStationsAtLocation = new List<Station>();

        for (int i = 0; i < stationController.stationList.ToArray().Length; i++)
        {
            if (stationController.stationList[i].location_ID == locationController.CurrentLocation.location_ID)
            {
                allStationsAtLocation.Add(stationController.stationList[i]);
            }
        }

        // Make a bool array for each station the user can find. Used to determine which station the user scans
        visitedStations = new bool[allStationsAtLocation.Count];
        for (int i = 0; i < visitedStations.Length; i++)
        {
            visitedStations[i] = false;
        }
    }

    void FindAllTargets()
    {
        // Locate all stations on current location
        int childCount = gameObject.transform.childCount;
        targets = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            targets[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    void CheckForScannedTargets()
    {
        bool found = false;

        if (targets != null)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                string targetName = targets[i].name;

                var renderComponents = targets[i].GetComponentInChildren<Renderer>();

                if (renderComponents.enabled)
                    found = true;
                else
                    found = false;

                // Check which station the user has scanned
                if (found)
                {
                    Debug.Log("Found " + targetName);
                    switch (targetName)
                    {
                        case "Station1":
                            visitedStations[0] = true;
                            break;
                        case "Station2":
                            visitedStations[1] = true;
                            break;
                        case "Station3":
                            visitedStations[2] = true;
                            break;
                        default:
                            break;
                    }

                    UpdateMainStationList();
                }
            }
        }
    }

    void UpdateMainStationList()
    {
        // Update the users current station
        for (int i = 0; i < stationController.stationList.Count; i++)
        {
            for (int j = 0; j < allStationsAtLocation.Count; j++)
            {
                if ((stationController.stationList[i].location_ID == allStationsAtLocation[j].location_ID)
                    && (stationController.stationList[i].station_NR == allStationsAtLocation[j].station_NR)
                    && visitedStations[j] == true)
                {
                    stationController.CurrentStation = stationController.stationList[i];
                    stationController.CallUpdateUserProgress();
                }
            }
        }

    }


}
