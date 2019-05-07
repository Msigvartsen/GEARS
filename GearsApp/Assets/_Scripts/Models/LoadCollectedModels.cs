using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCollectedModels : MonoBehaviour
{
    private LocationModel[] locationModels;
    private int[] collectedModelsID;
    private Model[] allModels;

    // Start is called before the first frame update
    void Start()
    {
        locationModels = ModelController.GetInstance().locationModels.ToArray();
        collectedModelsID = ModelController.GetInstance().foundModels.ToArray();
        allModels = ModelController.GetInstance().modelList.ToArray();

        GetItemList();
    }

    void GetItemList()
    {
        int numberOfButtons = 0;

        for (int i = 0; i < allModels.Length; i++)
        {
            if (allModels[i].modeltype == "Default")
            {
                GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/LoadCollectedModelsButton"), gameObject.transform);
                go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = allModels[i].model_name;
                go.GetComponent<LoadCollectedModelsButton>().myModel = allModels[i];
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
                        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/LoadCollectedModelsButton"), gameObject.transform);
                        go.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = allModels[i].model_name + " *";
                        go.GetComponent<LoadCollectedModelsButton>().myModel = allModels[i];
                        numberOfButtons++;
                    }
                }
            }
        }

        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, (125 * numberOfButtons) + 25);
    }
}
