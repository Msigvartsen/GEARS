using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that creates buttons based on which models the user has created.
/// </summary>
public class LoadCollectedModels : MonoBehaviour
{
    private LocationModel[] locationModels;
    private int[] collectedModelsID;
    private Model[] allModels;
    private string prefabButton = "";

    /// <summary>
    /// Set instances and determine what type of button to create.
    /// </summary>
    void Start()
    {
        if (ModelController.GetInstance().LocationModels != null)
            locationModels = ModelController.GetInstance().LocationModels.ToArray();
        if (ModelController.GetInstance().FoundModels != null)
            collectedModelsID = ModelController.GetInstance().FoundModels.ToArray();
        if (ModelController.GetInstance().ModelList != null)
            allModels = ModelController.GetInstance().ModelList.ToArray();

        if (SceneManager.GetActiveScene().name == "CollectionAR")
            prefabButton = "LoadModelButton";
        else
            prefabButton = "LoadCollectedModelsButton";

        CreateButtons();
    }

    /// <summary>
    /// Create buttons for every default model and every model the user has collected.
    /// </summary>
    void CreateButtons()
    {
        int numberOfButtons = 0;

        for (int i = 0; i < allModels.Length; i++)
        {
            if (allModels[i].modeltype == "Default")
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabButton), gameObject.transform);
                go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = allModels[i].model_name;

                if (prefabButton == "LoadModelButton")
                    go.GetComponent<LoadModelButton>().Model = allModels[i];
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
                            go.GetComponent<LoadModelButton>().Model = allModels[i];
                        else
                            go.GetComponent<LoadCollectedModelsButton>().model = allModels[i];

                        numberOfButtons++;
                    }
                }
            }
        }

        // Set size of viewport based on number of rows of buttons. (numberOfButtons - (numberOfButtons / 2)) = total rows
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, (520 * (numberOfButtons - (numberOfButtons / 2))) + 20);
    }
}
