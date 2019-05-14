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
        StartCoroutine(LocationServiceNS.LocationService.StartLocationService());
    }
    private void Awake()
    {
        LocationServiceNS.LocationService.CallUserPermission();
    }
}
