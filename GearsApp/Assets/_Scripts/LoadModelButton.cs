using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class LoadModelButton : MonoBehaviour
{
    public Model model;
    private Toggle button;
    private bool loaded = false;
    private GameObject modelToShow;

    void Start()
    {
        GetComponentInChildren<Text>().text = model.model_name;
        button = GetComponent<Toggle>();
        button.onValueChanged.AddListener(delegate { LoadModel(); });
        GetComponent<Toggle>().group = gameObject.transform.parent.GetComponent<ToggleGroup>();
    }

    public void LoadModel()
    {
        if (button.isOn)
        {
            if (!loaded)
            {
                modelToShow = Instantiate(Resources.Load<GameObject>("_Prefabs/" + model.model_name));
                modelToShow.transform.parent = GameObject.FindGameObjectWithTag("ModelImageTarget").transform;
                modelToShow.transform.localPosition = new Vector3(0, 0, 0);
                loaded = true;
            }
            else
            {
                modelToShow.SetActive(true);
            }

            GameObject.FindGameObjectWithTag("ModelTitleName").GetComponent<Text>().text = model.model_name;
        }
        else
        {
            modelToShow.SetActive(false);
        }
    }
}
