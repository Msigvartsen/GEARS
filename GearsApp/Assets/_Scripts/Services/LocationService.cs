using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationService : MonoBehaviour
{
    public double _currentLatitude;
    public double _currentLongitude;
    public bool _isLocationEnabled;

    IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            _currentLatitude = 60.793911;
            _currentLongitude = 11.07599;
            _isLocationEnabled = false;
            yield break;
        }
        // Start service before querying location
        Input.location.Start();
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            _isLocationEnabled = false;
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Running)
        {
            _currentLatitude = Input.location.lastData.latitude;
            _currentLongitude = Input.location.lastData.longitude;
            _isLocationEnabled = false;

        }
        else
        {
            _currentLatitude = 60.793911;
            _currentLongitude = 11.07599;
            Debug.Log("Unable to determine device location");
            _isLocationEnabled = true;
            yield break;
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
}
