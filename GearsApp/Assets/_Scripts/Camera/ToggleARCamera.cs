using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleARCamera : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject ARCamera;
    private GameObject mapCamera;
    public GameObject[] cameras;

    private void Start()
    {
        GetCameras();
        SetCamera("MainCamera");
    }

    private void GetCameras()
    {
        cameras = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject cam = transform.GetChild(i).gameObject;
            if (cam.name == "Main Camera")
            {
                mainCamera = cam;
            }
            else if (cam.name == "Map Camera")
            {
                mapCamera = cam;
            }
            else
            {
                ARCamera = cam;
            }
               
        }
    }
    //Lag en bedre funksjon
    public void SetCamera(string cameraName)
    {
        if(cameraName == "MainCamera")
        {
            mainCamera.SetActive(true);
            ARCamera.SetActive(false);
            mapCamera.SetActive(false);
        }
        else if (cameraName == "MapCamera")
        {
            mapCamera.SetActive(true);
            ARCamera.SetActive(false);
            mainCamera.SetActive(false);
        }
        else
        {
            ARCamera.SetActive(true);
            mainCamera.SetActive(false);
            mapCamera.SetActive(false);
            
        }
    }
}
