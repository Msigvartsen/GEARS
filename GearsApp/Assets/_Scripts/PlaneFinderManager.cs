using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class PlaneFinderManager : MonoBehaviour
{
    private GameObject planeFinder;
    private GameObject groundPlane;
    private GameObject modelOnPlane;

    private HelpTextManager htm;


    private StationController stationController;
    private LocationController locationController;
    private ModelController modelController;

    private List<Station> stationsAtLocation;
    private List<Model> stationModelsAtLocation;
    private GameObject modelAtStation;

    private Station closestStation;

    // Start is called before the first frame update
    void Start()
    {
        SetInstances();


        if (transform.parent.name == "ARPanel")
        {
            GetModelsAtStations();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.name == "3DModelsPanel")
        {
            if (CheckForChildren())
            {
                // Check if the model has been placed on the ground
                if (CheckForActiveChildren())
                {
                    // Disable user input regarding placing the model
                    TurnOffInputOnGround();
                }
                else
                {
                    // Enable user to scan and place the model on the ground
                    TurnOnInputOnGround();
                }
            }
            else
            {
                htm.SetHelpText((int)Help.SELECT);
                htm.FadeInHelpText();
            }
        }

        if (transform.parent.name == "ARPanel")
        {
            closestStation = GetClosestStation();

            if (closestStation != null)
            {
                // Check if the user in close enough to the station to view its content
                if (CheckDistanceToStation(closestStation, 18))
                {
                    htm.DisableButton();
                    LoadModelAtStation(closestStation);

                    // Check if the model has been placed on the ground
                    if (CheckForActiveChildren())
                    {
                        // The user has now "Scanned" a station. Update this data to database
                        UpdateStationData(closestStation);

                        // Disable user input regarding placing the model
                        TurnOffInputOnGround();
                    }
                    else
                    {
                        // Enable user to scan and place the model on the ground
                        TurnOnInputOnGround();
                    }
                }
                else
                {
                    TurnOffInputOnGround();
                    htm.SetHelpText((int)Help.DISTANCE);
                    htm.EnableButton();
                    htm.FadeInHelpText();
                }
            }
        }
    }

    // ------------------------------------------------------------------------------------------------------------------------------------------------------------
    
    private void UpdateStationData(Station station)
    {
        for (int i = 0; i < stationController.stationList.Count; i++)
        {
            if (station.station_NR == stationController.stationList[i].station_NR && station.location_ID == stationController.stationList[i].location_ID)
            {
                stationController.CurrentStation = stationController.stationList[i];
                stationController.CallUpdateUserProgress();
            }
        }
    }

    private Station GetClosestStation()
    {
        int currentDistance = 0;
        int prevDistance = -1;
        int lowestDistance = 0;
        int indexToReturn = -1;

        for (int i = 0; i < stationsAtLocation.Count; i++)
        {
            currentDistance = (int)CalculateDistanceInMeters((float)stationsAtLocation[i].latitude, (float)stationsAtLocation[i].longitude);
            if (currentDistance < prevDistance || prevDistance == -1)
            {
                lowestDistance = currentDistance;
                indexToReturn = i;
            }

            prevDistance = currentDistance;
        }
        return stationsAtLocation[indexToReturn];
    }

    private void LoadModelAtStation(Station station)
    {
        for (int i = 0; i < stationModelsAtLocation.Count; i++)
        {
            // Load the model connected to the station
            if (stationModelsAtLocation[i].model_ID == station.model_ID)
            {
                if (modelAtStation == null)
                {
                    modelAtStation = Instantiate(Resources.Load<GameObject>("_Prefabs/" + stationModelsAtLocation[i].model_name), groundPlane.transform);

                    // Get the renderer component in either child or on current object and turn it off
                    if (modelAtStation.GetComponentsInChildren<Renderer>(true).Length > 0)
                    {
                        var rendererComponents = modelAtStation.GetComponentsInChildren<Renderer>(true);
                        foreach (var componenent in rendererComponents)
                        {
                            componenent.enabled = false;
                        }
                    }
                    else if (modelAtStation.GetComponent<Renderer>())
                    {
                        modelAtStation.GetComponent<Renderer>().enabled = false;
                    }
                }
            }
        }
    }

    private bool CheckDistanceToStation(Station station, int range)
    {
        double distance = CalculateDistanceInMeters((float)station.latitude, (float)station.longitude);

        if (distance < range)
            return true;
        else
            return false;
    }

    private void GetModelsAtStations()
    {
        // Find stations connected to selected location
        for (int i = 0; i < stationController.stationList.Count; i++)
        {
            if (stationController.stationList[i].location_ID == locationController.CurrentLocation.location_ID)
            {
                // Add relevant stations to a list
                stationsAtLocation.Add(stationController.stationList[i]);
            }
        }
        // Find the models connected to the different stations at the selected location
        for (int i = 0; i < stationsAtLocation.Count; i++)
        {
            for (int j = 0; j < modelController.modelList.Count; j++)
            {
                if (stationsAtLocation[i].model_ID == modelController.modelList[j].model_ID)
                {
                    // Add relevant models to a list
                    stationModelsAtLocation.Add(modelController.modelList[j]);
                }
            }
        }
    }

    private void SetInstances()
    {
        planeFinder = GameObject.FindGameObjectWithTag("PlaneFinder");
        groundPlane = GameObject.FindGameObjectWithTag("GroundPlane");
        //htm = GameObject.FindGameObjectWithTag("HTM").GetComponent<HelpTextManager>();

        htm = GetComponent<HelpTextManager>();

        stationController = StationController.GetInstance();
        locationController = LocationController.GetInstance();
        modelController = ModelController.GetInstance();
        stationsAtLocation = new List<Station>();
        stationModelsAtLocation = new List<Model>();
    }

    private bool CheckForChildren()
    {
        if (groundPlane.transform.childCount > 0)
            return true;
        else
            return false;
    }

    private bool CheckForActiveChildren()
    {
        if (groundPlane.transform.childCount > 0)
        {
            int activeChild = GetActiveChild();
            if (activeChild != -1)
            {
                return true;
            }
        }

        return false;
    }

    private int GetActiveChild()
    {
        int index = -1;
        // Loop through all children to see if any are active
        for (int i = 0; i < groundPlane.transform.childCount; i++)
        {
            Transform child = groundPlane.transform.GetChild(i);

            // Check if the child is enabled
            if (child.gameObject.activeSelf)
            {
                if (child.childCount > 0)
                {
                    // Check if the renderer components is enabled
                    var rendererComponents = groundPlane.transform.GetChild(i).GetComponentsInChildren<Renderer>(true);
                    foreach (var componenent in rendererComponents)
                    {
                        if (componenent.enabled)
                        {
                            index = i;
                            modelOnPlane = child.gameObject;
                        }
                    }
                }
                else
                {
                    if (child.GetComponent<Renderer>().enabled)
                    {
                        index = i;
                        modelOnPlane = child.gameObject;
                    }
                }
            }
        }
        return index;
    }

    private void TurnOffInputOnGround()
    {
        // Disable components
        planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = false;
        planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.SetActive(false);
        groundPlane.GetComponent<DefaultTrackableEventHandler>().enabled = false;
        htm.FadeOutHelpText();
    }

    private void TurnOnInputOnGround()
    {
        // Set correct help text
        if (planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.GetComponentInChildren<Renderer>().isVisible)
        {
            if (transform.parent.name == "3DModelsPanel")
                htm.SetHelpText((int)Help.PLACE);
            else if (transform.parent.name == "ARPanel")
                htm.SetHelpText((int)Help.STATION_PLACEMENT);
        }
        else
        {
            if (transform.parent.name == "3DModelsPanel")
                htm.SetHelpText((int)Help.SEARCH);
            else if (transform.parent.name == "ARPanel")
                htm.SetHelpText((int)Help.STATION_PLACEMENT);
        }

        // Enable components
        planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = true;
        planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.SetActive(true);
        groundPlane.GetComponent<DefaultTrackableEventHandler>().enabled = true;
        htm.FadeInHelpText();
    }

    private double CalculateDistanceInMeters(float lat, float longi)
    {
        float userLat = Input.location.lastData.latitude;
        float userLong = Input.location.lastData.longitude;
        int R = 6371;
        var lat_rad_1 = Mathf.Deg2Rad * userLat;
        var lat_rad_2 = Mathf.Deg2Rad * lat;
        var d_lat_rad = Mathf.Deg2Rad * (lat - userLat);
        var d_long_rad = Mathf.Deg2Rad * (longi - userLong);
        var a = Mathf.Pow(Mathf.Sin(d_lat_rad / 2), 2) + (Mathf.Pow(Mathf.Sin(d_long_rad / 2), 2) * Mathf.Cos(lat_rad_1) * Mathf.Cos(lat_rad_2));
        var c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1 - a));
        var total_dist = R * c * 1000;

        return total_dist;
    }

}
