using Mapbox.Utils;
using System.Collections;
using UnityEngine;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
namespace LocationServiceNS
{

    public static class LocationService
    {
        public static void CallUserPermission()
        {
#if PLATFORM_ANDROID
            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                Permission.RequestUserPermission(Permission.FineLocation);
            }
#endif
        }

        public static IEnumerator StartLocationService()
        {
            if (!Input.location.isEnabledByUser)
            {
                yield break;
            }
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
                Debug.Log("Connection to LocationService Timed Out");
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determine device location");
                yield break;
            }
        }

        public static void StopLocationService()
        {
            Input.location.Stop();
        }

        public static Vector2d GetLatitudeLongitude()
        {
            Vector2d latlong = new Vector2d(0, 0);
            if (Input.location.status == LocationServiceStatus.Running)
            {
                latlong = new Vector2d(Input.location.lastData.latitude, Input.location.lastData.longitude);
            }

            return latlong;
        }

        public static bool IsLocationServiceRunning()
        {
            return (Input.location.status == LocationServiceStatus.Running && Input.location.isEnabledByUser);
        }
    }
}
