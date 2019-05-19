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
    private HelpTextManager htm;

    private List<Station> stationsAtLocation;
    private List<Model> stationModelsAtLocation;
    private GameObject modelAtStation;
    private GameObject shadowPlane;
    private GameObject smokeSpawn;


    /// <summary>
    /// Set all instances.
    /// </summary>
    void Awake()
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
            if (IsChildActive())
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
            htm.FadeInHelpText();
            htm.SetHelpText((int)Help.SELECT);
        }
    }


    /// <summary>
    /// Retrieve all neccesary instances.
    /// </summary>
    private void SetInstances()
    {
        modelController = ModelController.GetInstance();
        htm = GetComponent<HelpTextManager>();

        stationsAtLocation = new List<Station>();
        stationModelsAtLocation = new List<Model>();

        // Instantiate the selected model from previous scene
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + modelController.SelectedCollectibleModel.model_name), groundPlane.transform);
        shadowPlane = Instantiate(Resources.Load<GameObject>("_Prefabs/" + "ShadowPlane"), go.transform);
        smokeSpawn = Instantiate(Resources.Load<GameObject>("_Prefabs/" + "SmokeSpawn"), go.transform);

        shadowPlane.GetComponent<Renderer>().enabled = false;
        smokeSpawn.GetComponent<Renderer>().enabled = false;
        
        // Get the renderer component in either child or on current object and turn it off
        if (go.GetComponentsInChildren<Renderer>(true).Length > 0)
        {
            var rendererComponents = go.GetComponentsInChildren<Renderer>(true);
            foreach (var componenent in rendererComponents)
            {
                componenent.enabled = false;
                Debug.Log(componenent.enabled);
            }
        }
        else if (go.GetComponent<Renderer>())
        {
            go.GetComponent<Renderer>().enabled = false;
        }

        go.transform.localPosition = new Vector3(0, 0, 0);
        smokeSpawn.transform.localPosition = new Vector3(0, 0, 0);
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
    private bool IsChildActive()
    {
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
                            return true;
                        }
                    }
                }
                else
                {
                    if (child.GetComponent<Renderer>().enabled)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
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
            htm.SetHelpText((int)Help.PLACE);
        }
        else
        {
            htm.SetHelpText((int)Help.SCANNING);
        }

        // Enable components
        planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = true;
        planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.SetActive(true);
        groundPlane.GetComponent<DefaultTrackableEventHandler>().enabled = true;
        htm.FadeInHelpText();
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
