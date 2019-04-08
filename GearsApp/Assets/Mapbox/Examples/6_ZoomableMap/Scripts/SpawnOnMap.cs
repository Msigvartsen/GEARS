namespace Mapbox.Examples
{
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
            Invoke("SetMarkers", 2);
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
				    var location = _locations[i];
				    spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				    spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			    }
            }

            if (userObject != null)
            {
                Vector2d userLoc = LocationServiceNS.LocationService.GetLatitudeLongitude();
                userObject.transform.localPosition = _map.GeoToWorldPosition(userLoc, true);
                userObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }
		}

        private void SetMarkers()
        {
            locationController = LocationController.GetInstance();

            Debug.Log(locationController.locationList.Count);

            // Add markers on map for locations the user can visit
            _locations = new Vector2d[locationController.locationList.Count];
            _spawnedObjects = new List<GameObject>();
            for (int i = 0; i < locationController.locationList.Count; i++)
            {
                _locations[i] = new Vector2d(locationController.locationList[i].latitude, locationController.locationList[i].longitude);
                var instance = Instantiate(_markerPrefab);
                instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                instance.GetComponentInChildren<TextMesh>().text = locationController.locationList[i].name;
                _spawnedObjects.Add(instance);
            }

            // Set up the user marker on the map to display users position
            if (_userPrefab != null && Input.location.isEnabledByUser)
            {
                Vector2d userLoc = LocationServiceNS.LocationService.GetLatitudeLongitude();
                userObject = Instantiate(_userPrefab);
                userObject.transform.localPosition = _map.GeoToWorldPosition(userLoc, true);
                userObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }
        }

        private void SetStationMarkers(Location location)
        {
            StationController stationController = StationController.GetInstance();

            // Add markers for stations connected to selected location
            _spawnedStations = new List<GameObject>();
            for (int i = 0; i < stationController.stationList.Count; i++)
            {
                if (s)
                
            }
        }
	}
}