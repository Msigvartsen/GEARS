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

    // Start is called before the first frame update
    void Start()
    {
        models = ModelController.GetInstance().modelList.ToArray();
        GetItemList();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetItemList()
    {
        itemList = new GameObject[models.Length];

        for (int i = 0; i < models.Length; i++)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/LoadModelButton"));
            go.transform.SetParent(gameObject.transform, false);
            go.GetComponent<LoadModelButton>().model = models[i];
        }
    }
}
