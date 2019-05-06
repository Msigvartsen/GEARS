using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ModelLoader : MonoBehaviour
{
    private GameObject[] itemList;
    private GameObject selectedModel;
    private Model[] models;
    private LocationModel[] locationModels;

    // Start is called before the first frame update
    void Start()
    {
        models = ModelController.GetInstance().modelList.ToArray();
        locationModels = ModelController.GetInstance().locationModels.ToArray();
        GetItemList();
    }

    void GetItemList()
    {
        itemList = new GameObject[models.Length];

        // Load buttons for all models connected to selected location
        for (int i = 0; i < models.Length; i++)
        {
            for (int j = 0; j < locationModels.Length; j++)
            {
                if (LocationController.GetInstance().CurrentLocation.location_ID == locationModels[j].location_ID && models[i].model_ID == locationModels[j].model_ID)
                {
                    GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/LoadModelButton"));
                    go.GetComponentInChildren<Text>().text = models[i].model_name;
                    go.transform.SetParent(gameObject.transform, false);
                    go.GetComponent<LoadModelButton>().model = models[i];
                }
            }
        }

        // Add buttons for visited stations. User can see these models at home after they have been scanned.
        for (int i = 0; i < StationController.GetInstance().stationList.Count; i++)
        {
            for (int j = 0; j < models.Length; j++)
            {
                if (StationController.GetInstance().stationList[i].visited 
                    && (LocationController.GetInstance().CurrentLocation.location_ID == StationController.GetInstance().stationList[i].location_ID) 
                    && (StationController.GetInstance().stationList[i].model_ID == models[j].model_ID))
                {
                    GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/LoadModelButton"));
                    go.GetComponentInChildren<Text>().text = "Station " + StationController.GetInstance().stationList[i].station_NR;
                    go.transform.SetParent(gameObject.transform, false);
                    go.GetComponent<LoadModelButton>().model = models[j];
                }
            }
        }
    }
}
