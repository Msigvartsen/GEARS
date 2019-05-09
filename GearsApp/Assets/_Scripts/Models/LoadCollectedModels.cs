using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCollectedModels : MonoBehaviour
{
    private LocationModel[] locationModels;
    private int[] collectedModelsID;
    private Model[] allModels;
    private string prefabButton = "";

    // Start is called before the first frame update
    void Start()
    {
        locationModels = ModelController.GetInstance().LocationModels.ToArray();
        collectedModelsID = ModelController.GetInstance().FoundModels.ToArray();
        allModels = ModelController.GetInstance().ModelList.ToArray();

        if (SceneManager.GetActiveScene().name == "CollectionAR")
            prefabButton = "LoadModelButton";
        else
            prefabButton = "LoadCollectedModelsButton";

        GetItemList();
    }

    void GetItemList()
    {
        int numberOfButtons = 0;

        for (int i = 0; i < allModels.Length; i++)
        {
            if (allModels[i].modeltype == "Default")
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabButton), gameObject.transform);
                go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = allModels[i].model_name;

                if (prefabButton == "LoadModelButton")
                    go.GetComponent<LoadModelButton>().model = allModels[i];
                else
                    go.GetComponent<LoadCollectedModelsButton>().model = allModels[i];


                numberOfButtons++;
            }
        }

        if (collectedModelsID != null)
        {
            for (int i = 0; i < allModels.Length; i++)
            {
                for (int j = 0; j < collectedModelsID.Length; j++)
                {
                    if (collectedModelsID[j] == allModels[i].model_ID)
                    {
                        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabButton), gameObject.transform);
                        go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = allModels[i].model_name + " *";

                        if (prefabButton == "LoadModelButton")
                            go.GetComponent<LoadModelButton>().model = allModels[i];
                        else
                            go.GetComponent<LoadCollectedModelsButton>().model = allModels[i];

                        numberOfButtons++;
                    }
                }
            }
        }

        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, (125 * numberOfButtons) + 25);
    }
}
