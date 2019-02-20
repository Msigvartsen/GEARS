using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class PlaneFinderManager : MonoBehaviour
{
    private GameObject planeFinder;
    private GameObject groundPlane;
    private GameObject modelOnPlane;

    private HelpTextManager htm;

    // Start is called before the first frame update
    void Start()
    {
        planeFinder = GameObject.FindGameObjectWithTag("PlaneFinder");
        groundPlane = GameObject.FindGameObjectWithTag("GroundPlane");
        htm = GameObject.FindGameObjectWithTag("HTM").GetComponent<HelpTextManager>();
    }

    // Update is called once per frame
    void Update()
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
            htm.SetHelpText((int)Help.SELECT);
        }

    }

    public bool CheckForChildren()
    {
        if (groundPlane.transform.childCount > 0)
            return true;
        else
            return false;
    }

    public bool CheckForActiveChildren()
    {
        if (groundPlane.transform.childCount > 0)
        {
            int activeChild = FindActiveChild();
            if (activeChild != -1)
            {
                return true;
            }
        }

        return false;
    }

    public int FindActiveChild()
    {
        int index = -1;
        // Loop through all children to see if any are active
        for (int i = 0; i < groundPlane.transform.childCount; i++)
        {
            Transform child = groundPlane.transform.GetChild(i);

            // Check if the child is enabled
            if (child.gameObject.activeSelf)
            {
                if (child.childCount > 0)
                {
                    // Check if the renderer components is enabled
                    var rendererComponents = groundPlane.transform.GetChild(i).GetComponentsInChildren<Renderer>(true);
                    foreach (var componenent in rendererComponents)
                    {
                        if (componenent.enabled)
                        {
                            index = i;
                            modelOnPlane = child.gameObject;
                        }
                    }
                }
                else
                {
                    if (child.GetComponent<Renderer>().enabled)
                    {
                        index = i;
                        modelOnPlane = child.gameObject;
                    }
                }
            }
        }
        return index;
    }

    public void TurnOffInputOnGround()
    {
        // Disable components
        planeFinder.GetComponent<AnchorInputListenerBehaviour>().enabled = false;
        planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.SetActive(false);
        groundPlane.GetComponent<DefaultTrackableEventHandler>().enabled = false;
        htm.FadeOutHelpText();
    }

    public void TurnOnInputOnGround()
    {
        // Set correct help text
        if (planeFinder.GetComponent<PlaneFinderBehaviour>().PlaneIndicator.GetComponentInChildren<Renderer>().isVisible)
        {
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
}
