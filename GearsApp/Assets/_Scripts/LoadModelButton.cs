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
            // Create the prefab if it is not loaded
            if (!loaded)
            {
                modelToShow = Instantiate(Resources.Load<GameObject>("_Prefabs/" + model.model_name));

                // Get the renderer component in either child or on current object and turn it off
                if (modelToShow.GetComponentsInChildren<Renderer>(true).Length > 0)
                {
                    var rendererComponents = modelToShow.GetComponentsInChildren<Renderer>(true);
                    foreach (var componenent in rendererComponents)
                    {
                        componenent.enabled = false;
                    }
                }
                else if (modelToShow.GetComponent<Renderer>())
                {
                    modelToShow.GetComponent<Renderer>().enabled = false;
                }

                // Set the ground plane to be the models parent
                modelToShow.transform.parent = GameObject.FindGameObjectWithTag("GroundPlane").transform;
                modelToShow.transform.localPosition = new Vector3(0, 0, 0);
                loaded = true;
            }
            else
            {
                // Activate the model if its selected and loaded
                modelToShow.SetActive(true);
            }
        }
        else
        {
            modelToShow.SetActive(false);
        }
    }
}
