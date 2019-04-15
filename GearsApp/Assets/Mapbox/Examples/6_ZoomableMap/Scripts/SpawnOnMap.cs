
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpawnOnMap : MonoBehaviour
{
    [SerializeField]
    AbstractMap _map;

    [SerializeField]
    float _spawnScale = 10f;

    [SerializeField]
    float _stationSize = 5f;

    [SerializeField]
    GameObject _markerPrefab;

    [SerializeField]
    GameObject _userPrefab;

    [SerializeField]
    GameObject _stationPrefab;

    LocationController locationController;
    Location[] locations;
    List<Station> stations;

    List<GameObject> _spawnedObjects;
    List<GameObject> _spawnedStations;
    GameObject userObject;

    Vector2d targetCenter;

    bool stationsSpawned = false;
    bool focusLocation = false;
    float zoomSpeed = 5f;

    void Start()
    {
        //LocationServiceNS.LocationService.CallUserPermission();
        //StartCoroutine(LocationServiceNS.LocationService.StartLocationService());
        //Invoke("SetMarkers", 2); // Temporary:  needs a delay to retreive locations first.
        SetMarkers();
    }

    private void Update()
    {
        // Update positions for markers based on map movement
        if (_spawnedObjects != null)
        {
            UpdateLocationMarkers();
        }

        // Update user marker to check for movement
        if (userObject != null)
        {
            UpdateUserMarker();
        }

        // Update positions for station markers
        if (_spawnedStations != null)
        {
            UpdateStationMarkers();
        }

        if (focusLocation)
        {
            ZoomOnLocation();
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "MapMarker")
                {
                    hit.collider.gameObject.GetComponent<MapMarker>().SpawnLocationPopupInfo();
                }

                if (hit.collider.gameObject.tag == "StationMarker")
                {
                    hit.collider.gameObject.GetComponent<StationMarker>().SpawnStationPoputInfo();
                }
            }
            else
            {

            }
        }
    }

    private void UpdateUserMarker()
    {
        Vector2d userLoc = LocationServiceNS.LocationService.GetLatitudeLongitude();
        userObject.transform.localPosition = _map.GeoToWorldPosition(userLoc, true);
        userObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
    }

    private void UpdateLocationMarkers()
    {
        for (int i = 0; i < _spawnedObjects.Count; i++)
        {
            if (GetComponent<AbstractMap>().Zoom > 4f)
            {
                _spawnedObjects[i].SetActive(true);
                GameObject spawnedObject = _spawnedObjects[i];
                var location = _spawnedObjects[i].GetComponent<MapMarker>().MapMarkerLocation;
                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(new Vector2d(location.latitude, location.longitude), true);
                spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }
            else
            {
                _spawnedObjects[i].SetActive(false);
            }
        }
    }

    private void UpdateStationMarkers()
    {
        for (int i = 0; i < _spawnedStations.Count; i++)
        {
            if (GetComponent<AbstractMap>().Zoom > 13.5f)
            {
                _spawnedStations[i].SetActive(true);
                GameObject spawnedStation = _spawnedStations[i];
                spawnedStation.transform.localPosition = _map.GeoToWorldPosition(new Vector2d(stations[i].latitude, stations[i].longitude), true);
                spawnedStation.transform.localScale = new Vector3(_stationSize, _stationSize, _stationSize);
            }
            else
            {
                _spawnedStations[i].SetActive(false);
            }
        }
    }

    private void SetMarkers()
    {
        locationController = LocationController.GetInstance();
        locations = locationController.locationList.ToArray();
        _spawnedObjects = new List<GameObject>();

        // Set up markers for each location
        foreach (Location loc in locations)
        {
            var instance = Instantiate(_markerPrefab);
            var mapMarker = instance.GetComponent<MapMarker>();
            mapMarker.MapMarkerLocation = loc;
            mapMarker.UpdateData();

            Vector2d latlong = locationController.GetLatitudeLongitudeFromLocation(loc);

            instance.transform.localPosition = _map.GeoToWorldPosition(latlong, true);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

            _spawnedObjects.Add(instance);
        }

        // Set up the user marker on the map to display users position
        if (_userPrefab != null && Input.location.isEnabledByUser)
        {
            Vector2d userLoc = LocationServiceNS.LocationService.GetLatitudeLongitude();
            userObject = SetLocationOnMap(userLoc, _userPrefab);
        }
    }

    public void SetStationMarkers(Location location)
    {
        StationController stationController = StationController.GetInstance();

        // Add markers for stations connected to selected location
        if (!stationsSpawned)
        {
            _spawnedStations = new List<GameObject>();
            stations = new List<Station>();

            foreach (var item in stationController.stationList)
            {
                if (item.location_ID == location.location_ID)
                    stations.Add(item);
            }

            foreach (var item in stations)
            {
                GameObject go = SetLocationOnMap(new Vector2d(item.latitude, item.longitude), _stationPrefab);
                var stationMarker = go.GetComponent<StationMarker>();
                stationMarker.StationMarkerStation = item;

                go.transform.localScale = new Vector3(_stationSize, _stationSize, _stationSize);
                go.GetComponentInChildren<TextMesh>().text = "Station " + item.station_NR;
                _spawnedStations.Add(go);
            }

            stationsSpawned = true;
        }
    }

    private GameObject SetLocationOnMap(Vector2d loc, GameObject prefab)
    {
        GameObject go = Instantiate(prefab);
        go.transform.localPosition = _map.GeoToWorldPosition(loc, true);
        go.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

        return go;
    }

    public void MoveCameraToLocation(Location loc)
    {
        targetCenter = new Vector2d(loc.latitude, loc.longitude);
        GetComponent<AbstractMap>().UpdateMap(targetCenter);

        SetStationMarkers(loc);

        focusLocation = true;
    }

    private void ZoomOnLocation()
    {
        float targetZoom = 16f;

        if (GetComponent<AbstractMap>().Zoom < targetZoom)
        {
            GetComponent<AbstractMap>().UpdateMap(targetCenter, GetComponent<AbstractMap>().Zoom + Time.deltaTime * zoomSpeed);
        }
        else
        {
            focusLocation = false;
        }

    }
}
