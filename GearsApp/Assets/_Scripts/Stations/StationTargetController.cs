using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class StationTargetController : MonoBehaviour
{
    private LocationController locationController;
    private StationController stationController;

    private Location location;
    private Station[] stations;
    private List<Station> tempList;

    private GameObject[] targets;

    // Start is called before the first frame update
    void Start()
    {
        SetAllInstances();
        FindAllTargets();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForScannedTargets();
    }

    void SetAllInstances()
    {
        locationController = LocationController.GetInstance();
        stationController = StationController.GetInstance();

        if (locationController.CurrentLocation != null)
        {
            location = locationController.CurrentLocation;
        }

        tempList = new List<Station>();

        for (int i = 0; i < stationController.stationList.ToArray().Length; i ++)
        {
            if (stationController.stationList[i].location_ID == locationController.CurrentLocation.location_ID)
            {
                tempList.Add(stationController.stationList[i]);
            }
        }

        stations = tempList.ToArray();
    }

    void FindAllTargets()
    {
        int childCount = gameObject.transform.childCount;
        targets = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            targets[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    void CheckForScannedTargets()
    {
        bool found = false;

        for (int i = 0; i < targets.Length; i++)
        {
            string targetName = targets[i].GetComponent<ImageTargetBehaviour>().ImageTarget.Name;

            var renderComponents = targets[i].GetComponentInChildren<MeshRenderer>();

            if (renderComponents.enabled)
                found = true;
            else
                found = false;

            //foreach (var components in renderComponents)
            //{
            //    if (components.enabled)
            //    {
            //        found = true;
            //        break;
            //    }
            //}

            if (found)
            {
                Debug.Log("Found " + targetName);

                switch (targetName)
                {
                    case "tarmac":
                        stations[0].visited = true;
                        break;
                    case "chips":
                        stations[1].visited = true;
                        break;
                    case "stones":
                        stations[2].visited = true;
                        break;
                    default:
                        break;
                }

                UpdateMainStationList();
            }
        }

        //foreach (var go in targets)
        //{
        //    string targetName = go.GetComponent<ImageTargetBehaviour>().ImageTarget.Name;

        //    var renderComponents = go.GetComponentsInChildren<Renderer>();

        //    foreach (var components in renderComponents)
        //    {
        //        if (components.enabled)
        //            found = true;
        //    }

        //    if (found)
        //    {
        //        Debug.Log("Found " + targetName);

        //        switch (targetName)
        //        {
        //            case "tarmac":
        //                stations[0].visited = true;
        //                break;
        //            case "chips":
        //                stations[1].visited = true;
        //                break;
        //            case "stones":
        //                stations[2].visited = true;
        //                break;
        //            default:
        //                break;
        //        }

        //        UpdateMainStationList();
        //    }
        //}
    }

    void UpdateMainStationList()
    {
        for (int i = 0; i < stationController.stationList.Count; i++)
        {
            for (int j = 0; j < stations.Length; j++)
            {
                if ((stationController.stationList[i].location_ID == stations[j].location_ID) 
                    && (stationController.stationList[i].station_NR == stations[j].station_NR) 
                    && stations[j].visited)
                {
                    stationController.stationList[i].visited = true;
                }
            }
        }

        stationController.CallUpdateUserProgress();
    }


}
