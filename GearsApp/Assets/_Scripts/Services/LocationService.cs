﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

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
}