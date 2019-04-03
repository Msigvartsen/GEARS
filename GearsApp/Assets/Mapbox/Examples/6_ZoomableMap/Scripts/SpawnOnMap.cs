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
        Location[] locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;
        LocationController locationController;

		void Start()
		{
            
            Invoke("SetMarkers", 2); // Temporary:  needs a delay to retreive locations first. 
		}

		private void Update()
		{
            if (_spawnedObjects != null)
            {
                foreach(var obj in _spawnedObjects)
                {
                    var loc = obj.GetComponent<MapMarker>().MapMarkerLocation;
                    Vector2d latlong = locationController.GetLatitudeLongitudeFromLocation(loc);
                    obj.transform.localPosition = _map.GeoToWorldPosition(latlong, true);
                    obj.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                }
            }
		}

        private void SetMarkers()
        {
            locationController = LocationController.GetInstance();
            locations = locationController.locationList.ToArray();
            _spawnedObjects = new List<GameObject>();

            foreach(Location loc in locations)
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
        }
	}
}