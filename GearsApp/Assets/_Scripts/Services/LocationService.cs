using Mapbox.Utils;
using System.Collections;
using UnityEngine;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
namespace LocationServiceNS
{
    /// <summary>
    /// Static class that handles Location Services.
    /// </summary>
    public static class LocationService
    {
        /// <summary>
        /// Requests permission from the user to access geolocation via android.
        /// This needs to be active for the geolocation to work.
        /// </summary>
        public static void CallUserPermission()
        {
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
            }
#endif
        }

        /// <summary>
        /// Try to start location service. Connection retries for 20 seconds before ending.
        /// Will try to match desired accuracy. If this fails it will use a default value. Location each updateDistanceInMeters.
        /// </summary>
        /// <returns></returns>
        public static IEnumerator StartLocationService(float desiredAccuracyInMeters = 1, float updateDistanceInMeters = 5)
        {
            if (!Input.location.isEnabledByUser)
            {
                yield break;
            }
            Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);

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
                Debug.Log("Connection to LocationService Timed Out");
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determine device location");
                yield break;
            }
        }

        /// <summary>
        /// Stop Location Service from Running.
        /// </summary>
        public static void StopLocationService()
        {
            Input.location.Stop();
        }

        /// <summary>
        /// Get longitude and latitude from mobile location
        /// </summary>
        /// <returns>Vector2d containing latitude and longitude to the users mobile</returns>
        public static Vector2d GetLatitudeLongitude()
        {
            Vector2d latlong = new Vector2d(0, 0);
            if (Input.location.status == LocationServiceStatus.Running)
            {
                latlong = new Vector2d(Input.location.lastData.latitude, Input.location.lastData.longitude);
            }
            return latlong;
        }

        /// <summary>
        /// Checks if the location service is enabled and running
        /// </summary>
        /// <returns>Returns true if location service is active</returns>
        public static bool IsLocationServiceRunning()
        {
            return (Input.location.status == LocationServiceStatus.Running && Input.location.isEnabledByUser);
        }
    }
}
