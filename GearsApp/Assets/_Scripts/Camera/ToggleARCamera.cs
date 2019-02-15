using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleARCamera : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject ARCamera;
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
        }
        else
        {
            ARCamera.SetActive(true);
            mainCamera.SetActive(false);
            
        }
    }
}
