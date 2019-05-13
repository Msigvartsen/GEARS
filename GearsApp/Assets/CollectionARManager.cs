using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

/// <summary>
/// Handle AR placement on the ground when viewing your collected items.
/// </summary>
public class CollectionARManager : MonoBehaviour
{
    [SerializeField]
    private GameObject planeFinder;
    [SerializeField]
    private GameObject groundPlane;

    private ModelController modelController;

    private List<Station> stationsAtLocation;
    private List<Model> stationModelsAtLocation;
    private GameObject modelAtStation;
    private GameObject shadowPlane;


    /// <summary>
    /// Set all instances.
    /// </summary>
    void Start()
    {
        SetInstances();
    }

    /// <summary>
    /// Handle viewing and placement of models, checks every frame.
    /// </summary>
    void Update()
    {
        HandleModelViewingAndPlacement();
    }

    /// <summary>
    /// Handles viewing and placement of the model that the user has selected.
    /// </summary>
    private void HandleModelViewingAndPlacement()
    {
        if (CheckForChildren())
        {
            // Check if the model has been placed on the ground
            if (CheckForActiveChildren())
            {
                // Disable user input regarding placing the model
                TurnOffInputOnGround();
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
        }
    }


    /// <summary>
    /// Retrieve all neccesary instances.
    /// </summary>
    private void SetInstances()
    {
        modelController = ModelController.GetInstance();

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

    /// <summary>
    /// Check if a model has been selected.
    /// </summary>
    /// <returns>Returns true if a model has been selected, if not false.</returns>
    private bool CheckForChildren()
    {
        if (groundPlane.transform.childCount > 0)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Check if the model has been placed on the ground.
    /// </summary>
    /// <returns>Returns true if the model has been placed, if not false.</returns>
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

    /// <summary>
    /// Get the model that is active on the ground.
    /// </summary>
    /// <returns>Returns the index of the child.</returns>
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

    /// <summary>
    /// User can not place more models on the ground. Stop scanning for planes and surfaces.
    /// </summary>
    private void TurnOffInputOnGround()
    {
        // Disable components
        planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = false;
        planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.SetActive(false);
        groundPlane.GetComponent<DefaultTrackableEventHandler>().enabled = false;
    }

    /// <summary>
    /// User is able to place a model on the ground. Start scanning for planes and surfaces.
    /// </summary>
    private void TurnOnInputOnGround()
    {
        // Set correct help text
        if (planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.GetComponentInChildren<Renderer>().isVisible)
        {
            Vector3 optimalSize = groundPlane.transform.GetChild(0).GetComponentInChildren<BoxCollider>().size;
            planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.transform.GetChild(0).localScale = optimalSize;
        }

        // Enable components
        planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = true;
        planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.SetActive(true);
        groundPlane.GetComponent<DefaultTrackableEventHandler>().enabled = true;
    }

    /// <summary>
    /// Destroy all models placed on the ground.
    /// </summary>
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
