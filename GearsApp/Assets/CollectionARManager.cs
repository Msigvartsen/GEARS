using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class CollectionARManager : MonoBehaviour
{
    [SerializeField]
    private GameObject planeFinder;
    [SerializeField]
    private GameObject groundPlane;

    private ModelController modelController;
    private HelpTextManager htm;

    private List<Station> stationsAtLocation;
    private List<Model> stationModelsAtLocation;
    private GameObject modelAtStation;
    private GameObject shadowPlane;


    // Start is called before the first frame update
    void Start()
    {
        SetInstances();
    }

    // Update is called once per frame
    void Update()
    {
        HandleModelViewingAndPlacement();
    }

    private void HandleModelViewingAndPlacement()
    {
        htm.DisableButton();
        if (CheckForChildren())
        {
            // Check if the model has been placed on the ground
            if (CheckForActiveChildren())
            {
                // Disable user input regarding placing the model
                TurnOffInputOnGround();
                htm.FadeOutHelpText();
            }
            else
            {
                // Enable user to scan and place the model on the ground
                TurnOnInputOnGround();
            }
        }
        else
        {
            // User has not selected any models to view
            TurnOffInputOnGround();
            htm.SetHelpText((int)Help.SELECT);
            htm.FadeInHelpText();
        }
    }


    private void SetInstances()
    {
        modelController = ModelController.GetInstance();

        htm = GetComponent<HelpTextManager>();
        stationsAtLocation = new List<Station>();
        stationModelsAtLocation = new List<Model>();

        // Instantiate the selected model from previous scene
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + modelController.SelectedCollectibleModel.model_name), groundPlane.transform);
        go.transform.localPosition = new Vector3(0, 0, 0);
        shadowPlane = Instantiate(Resources.Load<GameObject>("_Prefabs/" + "ShadowPlane"), go.transform);
        shadowPlane.GetComponent<Renderer>().enabled = false;

        // Get the renderer component in either child or on current object and turn it off
        if (go.GetComponentsInChildren<Renderer>(true).Length > 0)
        {
            var rendererComponents = go.GetComponentsInChildren<Renderer>(true);
            foreach (var componenent in rendererComponents)
            {
                componenent.enabled = false;
            }
        }
        else if (go.GetComponent<Renderer>())
        {
            go.GetComponent<Renderer>().enabled = false;
        }

        go.transform.localPosition = new Vector3(0, 0, 0);

    }

    private bool CheckForChildren()
    {
        if (groundPlane.transform.childCount > 0)
            return true;
        else
            return false;
    }

    private bool CheckForActiveChildren()
    {
        if (groundPlane.transform.childCount > 0)
        {
            int activeChild = GetActiveChild();
            if (activeChild != -1)
            {
                return true;
            }
        }

        return false;
    }

    private int GetActiveChild()
    {
        int index = -1;

        // Check for children
        if (groundPlane.transform.childCount > 0)
        {
            Transform child = groundPlane.transform.GetChild(0);

            // Check if the child is enabled
            if (child.gameObject.activeSelf)
            {
                if (child.childCount > 0)
                {
                    // Check if the renderer components is enabled
                    var rendererComponents = child.GetComponentsInChildren<Renderer>(true);
                    foreach (var componenent in rendererComponents)
                    {
                        if (componenent.enabled)
                        {
                            index = 0;
                        }
                    }
                }
                else
                {
                    if (child.GetComponent<Renderer>().enabled)
                    {
                        index = 0;
                    }
                }
            }
        }

        return index;
    }

    private void TurnOffInputOnGround()
    {
        // Disable components
        planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = false;
        planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.SetActive(false);
        groundPlane.GetComponent<DefaultTrackableEventHandler>().enabled = false;
    }

    private void TurnOnInputOnGround()
    {
        // Set correct help text
        if (planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.GetComponentInChildren<Renderer>().isVisible)
        {
            Vector3 optimalSize = groundPlane.transform.GetChild(0).GetComponentInChildren<BoxCollider>().size;
            planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.transform.GetChild(0).localScale = optimalSize;

            htm.SetHelpText((int)Help.PLACE);
        }
        else
        {
            htm.SetHelpText((int)Help.SEARCH);
        }

        // Enable components
        planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = true;
        planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.SetActive(true);
        groundPlane.GetComponent<DefaultTrackableEventHandler>().enabled = true;
        htm.FadeInHelpText();
    }

    public void DestroyAllChildren()
    {
        if (groundPlane.transform.childCount > 0)
        {
            for (int i = 0; i < groundPlane.transform.childCount; i++)
            {
                Destroy(groundPlane.transform.GetChild(i).gameObject);
            }
        }
    }

}
