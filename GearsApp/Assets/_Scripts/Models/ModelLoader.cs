using UnityEngine;

public class ModelLoader : MonoBehaviour
{
    private GameObject selectedModel;
    private Model[] models;
    private LocationModel[] locationModels;

    /// <summary>
    /// Get models from database.
    /// </summary>
    void Start()
    {
        models = ModelController.GetInstance().ModelList.ToArray();
        locationModels = ModelController.GetInstance().LocationModels.ToArray();
        CreateButtons();
    }

    /// <summary>
    /// Create buttons for models the user can see at the selected location.
    /// </summary>
    void CreateButtons()
    {
        int numberOfButtons = 0;

        // Load buttons for all models connected to selected location
        for (int i = 0; i < models.Length; i++)
        {
            for (int j = 0; j < locationModels.Length; j++)
            {
                if (LocationController.GetInstance().CurrentLocation.location_ID == locationModels[j].location_ID && models[i].model_ID == locationModels[j].model_ID)
                {
                    GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/LoadModelButton"));
                    go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = models[i].model_name;
                    go.transform.SetParent(gameObject.transform, false);
                    go.GetComponent<LoadModelButton>().Model = models[i];
                    numberOfButtons++;
                }
            }
        }

        // Add buttons for visited stations. User can see these models at home after they have been scanned.
        for (int i = 0; i < StationController.GetInstance().StationList.Count; i++)
        {
            for (int j = 0; j < models.Length; j++)
            {
                if (StationController.GetInstance().StationList[i].visited 
                    && (LocationController.GetInstance().CurrentLocation.location_ID == StationController.GetInstance().StationList[i].location_ID) 
                    && (StationController.GetInstance().StationList[i].model_ID == models[j].model_ID))
                {
                    GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/LoadModelButton"));
                    go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Station " + StationController.GetInstance().StationList[i].station_NR;
                    go.transform.SetParent(gameObject.transform, false);
                    go.GetComponent<LoadModelButton>().Model = models[j];
                    numberOfButtons++;
                }
            }
        }

        if (numberOfButtons > 5)
            GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, 125 * numberOfButtons);
    }
}
