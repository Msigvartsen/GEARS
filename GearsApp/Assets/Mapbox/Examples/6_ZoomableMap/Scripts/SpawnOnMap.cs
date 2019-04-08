
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

    Vector2d[] _locations;
    Location[] locations;

    GameObject UserMarker;

    [SerializeField]
    float _spawnScale = 100f;

    [SerializeField]
    GameObject _markerPrefab;

    [SerializeField]
    GameObject _userPrefab;

    [SerializeField]
    GameObject _stationPrefab;

    List<GameObject> _spawnedObjects;
    List<GameObject> _spawnedStations;
    GameObject userObject;
    LocationController locationController;




    void Start()
    {
        LocationServiceNS.LocationService.CallUserPermission();
        StartCoroutine(LocationServiceNS.LocationService.StartLocationService());
        Invoke("SetMarkers", 2); // Temporary:  needs a delay to retreive locations first.
    }

    private void Update()
    {
        if (_spawnedObjects != null)
        {
            // Update positions for markers based on map movement
            int count = _spawnedObjects.Count;

            for (int i = 0; i < count; i++)
            {
                var spawnedObject = _spawnedObjects[i];
                var location = _spawnedObjects[i].GetComponent<MapMarker>().MapMarkerLocation;
                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(new Vector2d(location.latitude, location.longitude), true);
                spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }
        }

        if (userObject != null)
        {
            Vector2d userLoc = LocationServiceNS.LocationService.GetLatitudeLongitude();
            userObject.transform.localPosition = _map.GeoToWorldPosition(userLoc, true);
            userObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        }

        if (_spawnedStations != null)
        {
            int count = _spawnedStations.Count;

            for (int i = 0; i < count; i++)
            {
                var spawnedStation = _spawnedStations[i];
                var location = _spawnedStations[i].GetComponent<MapMarker>().MapMarkerLocation;
                spawnedStation.transform.localPosition = _map.GeoToWorldPosition(new Vector2d(location.latitude, location.longitude), true);
                spawnedStation.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }

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
            }
            else
            {

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
            SetLocationOnMap(userLoc, _userPrefab);
        }
    }

    public void SetStationMarkers(Location location)
    {
        StationController stationController = StationController.GetInstance();

        // Add markers for stations connected to selected location
        _spawnedStations = new List<GameObject>();
        for (int i = 0; i < stationController.stationList.Count; i++)
        {
            if (stationController.stationList[i].location_ID == location.location_ID)
            {
                Vector2d stationLoc = new Vector2d(stationController.stationList[i].latitude, stationController.stationList[i].longitude);
                SetLocationOnMap(stationLoc, _stationPrefab, true);
            }
        }
    }

    private void SetLocationOnMap(Vector2d loc, GameObject prefab, bool station = false)
    {
        GameObject go = Instantiate(prefab);
        go.transform.localPosition = _map.GeoToWorldPosition(loc, true);
        go.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

        if (station)
            _spawnedStations.Add(go);
    }
}
