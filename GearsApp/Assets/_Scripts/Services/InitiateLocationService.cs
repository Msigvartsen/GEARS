using UnityEngine;

/// <summary>
/// Simple script to initialize LocationService.
/// </summary>
public class InitiateLocationService : MonoBehaviour
{
    /// <summary>
    /// Ran at start.
    /// Asks for user permission and starts the location service.
    /// </summary>
    void Start()
    {
        LocationServiceNS.LocationService.CallUserPermission();
        StartCoroutine(LocationServiceNS.LocationService.StartLocationService());
    }

}
