using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vuforia;

public class ARPlacementManager : MonoBehaviour
{
    [SerializeField]
    private GameObject planeFinder;
    [SerializeField]
    private GameObject groundPlane;
    [SerializeField]
    Toggle toggleStationSearch;

    private StationController stationController;
    private LocationController locationController;
    private ModelController modelController;
    private HelpTextManager htm;

    private List<Station> stationsAtLocation;
    private List<Model> stationModelsAtLocation;
    private GameObject modelAtStation;
    private Station closestStation;
    private GameObject shadowPlane;


    /// <summary>
    /// Set all instances and retrieve models connected to stations.
    /// </summary>
    void Start()
    {
        SetInstances();

        GetModelsAtStations();
    }

    /// <summary>
    /// Find out which mode the user has selected, to give correct instructions and feedback.
    /// </summary>
    void Update()
    {
        if (!toggleStationSearch.isOn)
        {
            HandleModelViewingAndPlacement();
        }
        else
        {
            HandleStationSearch();
        }
    }

    /// <summary>
    /// Handle station search mode.
    /// </summary>
    private void HandleStationSearch()
    {
        // Check distance to selected location first, need to find a way to set range dynamically based on which location. 25 km for testing purposes
        if ((int)CalculateDistanceInMeters((float)locationController.CurrentLocation.latitude, (float)locationController.CurrentLocation.longitude) / 1000 < 25)
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
                    if (IsChildActive())
                    {
                        // The user has now "Scanned" a station. Update this data to database
                        UpdateStationData(closestStation);

                        // Disable user input regarding placing the model
                        TurnOffInputOnGround();
                        htm.FadeOutHelpText();
                    }
                    else
                    {
                        // Enable user to scan and place the model on the ground
                        TurnOnInputOnGround();
                    }
                }
                else
                {
                    // User is too far away from closest station
                    DestroyAllChildren();
                    TurnOffInputOnGround();
                    htm.SetHelpText((int)Help.DISTANCE);
                    htm.EnableButton();
                    htm.FadeInHelpText();
                }
            }
        }
        else
        {
            // User is too far away from selected location
            TurnOffInputOnGround();
            htm.SetHelpText((int)Help.LOCATION_DISTANCE);
            htm.EnableButton();
            htm.FadeInHelpText();
        }
    }

    /// <summary>
    /// Enable and disable user interaction to position and place viewable model.
    /// </summary>
    private void HandleModelViewingAndPlacement()
    {
        htm.DisableButton();
        if (CheckForChildren())
        {
            // Check if the model has been placed on the ground
            if (IsChildActive())
            {
                // Disable user input regarding placing the model
                TurnOffInputOnGround();
                htm.FadeOutHelpText();
            }
            else
            {
                // Enable user to scan and place the model on the ground
                TurnOnInputOnGround();
            }
        }
        else
        {
            // User has not selected any models to view
            TurnOffInputOnGround();
            htm.SetHelpText((int)Help.SELECT);
            htm.FadeInHelpText();
        }
    }

    /// <summary>
    /// Update scanned station to database.
    /// </summary>
    /// <param name="station">Station that has been scanned by user.</param>
    private void UpdateStationData(Station station)
    {
        // Add experience to user and update database with userprogress
        if (!station.visited)
        {
            UserController.GetInstance().UpdateUserExperience(station.score);
            UserController.GetInstance().CallUpdateUserExpAndLevel();
            ModelController.GetInstance().CallUpdateFoundModel(station.model_ID);


            // Update database with userprogress
            for (int i = 0; i < stationController.StationList.Count; i++)
            {
                if (station.station_NR == stationController.StationList[i].station_NR && station.location_ID == stationController.StationList[i].location_ID)
                {
                    stationController.CurrentStation = stationController.StationList[i];
                    stationController.CallUpdateUserProgress();
                }
            }
        }


    }

    /// <summary>
    /// Retrieve the station nearest to users location.
    /// </summary>
    /// <returns>Returns the nearest station at current location.</returns>
    private Station GetClosestStation()
    {
        int currentDistance = 0;
        int lowestDistance = -1;
        int indexToReturn = -1;

        // Find the station closest to the users position
        for (int i = 0; i < stationsAtLocation.Count; i++)
        {
            currentDistance = (int)CalculateDistanceInMeters((float)stationsAtLocation[i].latitude, (float)stationsAtLocation[i].longitude);
            if (currentDistance < lowestDistance || lowestDistance == -1)
            {
                lowestDistance = currentDistance;
                indexToReturn = i;
            }
        }
        return stationsAtLocation[indexToReturn];
    }

    /// <summary>
    /// Load the model connected to the station.
    /// </summary>
    /// <param name="station">Model at this station will be loaded.</param>
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
                    shadowPlane = Instantiate(Resources.Load<GameObject>("_Prefabs/ShadowPlane"), modelAtStation.transform);
                    shadowPlane.GetComponent<Renderer>().enabled = false;

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

    /// <summary>
    /// Check if the user is close enough to a station.
    /// </summary>
    /// <param name="station">Station to check distance against.</param>
    /// <param name="range">Range in meters.</param>
    /// <returns>Returns true if the user is within range of station, if not return false.</returns>
    private bool CheckDistanceToStation(Station station, int range)
    {
        // Check if the distance to the station is close enough
        double distance = CalculateDistanceInMeters((float)station.latitude, (float)station.longitude);

        if (distance < range)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Retrieves all models at the stations connected to the location.
    /// </summary>
    private void GetModelsAtStations()
    {
        // Find stations connected to selected location
        for (int i = 0; i < stationController.StationList.Count; i++)
        {
            if (stationController.StationList[i].location_ID == locationController.CurrentLocation.location_ID)
            {
                // Add relevant stations to a list
                stationsAtLocation.Add(stationController.StationList[i]);
            }
        }
        // Find the models connected to the different stations at the selected location
        for (int i = 0; i < stationsAtLocation.Count; i++)
        {
            for (int j = 0; j < modelController.ModelList.Count; j++)
            {
                if (stationsAtLocation[i].model_ID == modelController.ModelList[j].model_ID)
                {
                    // Add relevant models to a list
                    stationModelsAtLocation.Add(modelController.ModelList[j]);
                }
            }
        }
    }

    /// <summary>
    /// Set all instances of controllers and initiate lists.
    /// </summary>
    private void SetInstances()
    {
        stationController = StationController.GetInstance();
        locationController = LocationController.GetInstance();
        modelController = ModelController.GetInstance();

        htm = GetComponent<HelpTextManager>();
        stationsAtLocation = new List<Station>();
        stationModelsAtLocation = new List<Model>();
    }

    /// <summary>
    /// Check if a model has selected by the user.
    /// </summary>
    /// <returns>Return true if model is selected, else false.</returns>
    private bool CheckForChildren()
    {
        if (groundPlane.transform.childCount > 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Check if the model has been placed on the ground.
    /// </summary>
    /// <returns>Returns true if model is active and visible, else false.</returns>
    private bool IsChildActive()
    {
        // Check for children
        if (groundPlane.transform.childCount > 0)
        {
            Transform child = groundPlane.transform.GetChild(0);

            // Check if the child is enabled
            if (child.gameObject.activeSelf)
            {
                if (child.childCount > 0)
                {
                    // Check if the renderer components is enabled
                    var rendererComponents = child.GetComponentsInChildren<Renderer>(true);
                    foreach (var componenent in rendererComponents)
                    {
                        if (componenent.enabled)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (child.GetComponent<Renderer>().enabled)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Turn off user input regarding model placement and surface scanning.
    /// </summary>
    private void TurnOffInputOnGround()
    {
        // Disable components
        planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = false;
        planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.SetActive(false);
        groundPlane.GetComponent<DefaultTrackableEventHandler>().enabled = false;
    }

    /// <summary>
    /// Turn on user input regarding model placement and surface scanning.
    /// </summary>
    private void TurnOnInputOnGround()
    {
        // Set correct help text
        if (planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.GetComponentInChildren<Renderer>().isVisible)
        {
            Vector3 optimalSize = groundPlane.transform.GetChild(0).GetComponentInChildren<BoxCollider>().size;
            planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.transform.GetChild(0).localScale = optimalSize;

            if (!toggleStationSearch.isOn)
                htm.SetHelpText((int)Help.PLACE);
            else
                htm.SetHelpText((int)Help.STATION_PLACEMENT);
        }
        else
        {
            if (!toggleStationSearch.isOn)
                htm.SetHelpText((int)Help.SEARCH);
            else
                htm.SetHelpText((int)Help.STATION_PLACEMENT);
        }

        // Enable components
        planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = true;
        planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.SetActive(true);
        groundPlane.GetComponent<DefaultTrackableEventHandler>().enabled = true;
        htm.FadeInHelpText();
    }

    /// <summary>
    /// Calculate distance to a point in the world from users location.
    /// </summary>
    /// <param name="lat">Target latitude.</param>
    /// <param name="longi">Target longitude.</param>
    /// <returns>Returns distance in meters.</returns>
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

    /// <summary>
    /// Remove model placed on the ground.
    /// </summary>
    public void DestroyAllChildren()
    {
        if (groundPlane.transform.childCount > 0)
        {
            for (int i = 0; i < groundPlane.transform.childCount; i++)
            {
                Destroy(groundPlane.transform.GetChild(i).gameObject);
            }
        }
    }

}
