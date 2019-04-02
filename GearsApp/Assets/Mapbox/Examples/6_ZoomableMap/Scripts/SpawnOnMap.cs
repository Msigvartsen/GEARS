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

		List<GameObject> _spawnedObjects;

        LocationController locationController;

		void Start()
		{
            Invoke("SetMarkers", 2);
		}

		private void Update()
		{
            if (_spawnedObjects != null)
            {
			    int count = _spawnedObjects.Count;
			    for (int i = 0; i < count; i++)
			    {
				    var spawnedObject = _spawnedObjects[i];
				    var location = _locations[i];
				    spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				    spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			    }
            }
		}

        private void SetMarkers()
        {
            locationController = LocationController.GetInstance();

            Debug.Log(locationController.locationList.Count);

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
        }
	}
}