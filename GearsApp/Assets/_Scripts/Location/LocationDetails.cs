using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationDetails : MonoBehaviour
{
    private void Start()
    {
        LocationController manager = LocationController.GetInstance();

        GameObject gameObject = GameObject.FindGameObjectWithTag("HeaderButton");
        gameObject.GetComponentInChildren<Text>().text = manager.CurrentLocation.name;

        Transform[] children = GetComponentsInChildren<Transform>();
        foreach(Transform obj in children)
        {
            if(obj.name == "InfoPanel")
            {
                GameObject infoPanel = obj.gameObject;
                infoPanel.GetComponentInChildren<Text>().text = manager.CurrentLocation.information;
            }
        }
    }
}
