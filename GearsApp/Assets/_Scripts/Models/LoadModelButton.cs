using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class LoadModelButton : MonoBehaviour
{
    public Model model;
    private Button button;
    private bool loaded = false;
    private GameObject modelToShow;

    void Start()
    {
        GetComponentInChildren<Text>().text = model.model_name;
        button = GetComponent<Button>();
        button.onClick.AddListener(CloseProfilePictureWindow);
        button.onClick.AddListener(LoadModel);
    }

    public void LoadModel()
    {
        if (modelToShow != null && modelToShow.name != model.model_name)
        {
            Destroy(modelToShow.gameObject);
            modelToShow = null;
            loaded = false;
        }

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
            print("LoadModelButton found groundplane");
            modelToShow.transform.localPosition = new Vector3(0, 0, 0);
            loaded = true;
        }
        else
        {
            // Activate the model if its selected and loaded
            modelToShow.SetActive(true);
        }
    }
    public void CloseProfilePictureWindow()
    {
        GameObject p = GameObject.FindGameObjectWithTag("NewProfilePictureWindow");
        p.GetComponent<Animator>().Play("Fade-out");

    }
}

