using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System;

public class LocationDetails : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private TMPro.TextMeshProUGUI locationNameText;
    [SerializeField]
    private TMPro.TextMeshProUGUI infoText;
    [SerializeField]
    public Texture2D[] imagePanel; //Update ImagePanel with correct images from location
    private Location currentLocation;

    private void Start()
    {
        currentLocation = LocationController.GetInstance().CurrentLocation;
        locationNameText.text = currentLocation.name;
        UpdateInfoText();
    }

    private void UpdateInfoText()
    {
        LocationController manager = LocationController.GetInstance();
        Uri uri = new Uri(ConstantsNS.Constants.FTPLocationPath + manager.CurrentLocation.name + "/Information/basicinfo.txt");
        string infotext = FTPHandler.DownloadTextFromFTP(uri);
        infoText.text = infotext;
    }

    //IEnumerator UpdateInfoText()
    //{
    //    LocationController manager = LocationController.GetInstance();

    //    GameObject gameObject = GameObject.FindGameObjectWithTag("HeaderButton");
    //    gameObject.GetComponentInChildren<Text>().text = manager.CurrentLocation.name;

    //    Transform[] children = GetComponentsInChildren<Transform>();
    //    foreach (Transform obj in children)
    //    {
    //        if (obj.name == "InfoPanel")
    //        {
    //            GameObject infoPanel = obj.gameObject;
    //            Uri uri = new Uri(ConstantsNS.Constants.FTPLocationPath + manager.CurrentLocation.name + "/Information/basicinfo.txt");
    //            string infotext = FTPHandler.DownloadTextFromFTP(uri);
    //            infoPanel.GetComponentInChildren<Text>().text = infotext;//manager.CurrentLocation.information;
    //            infoPanel.GetComponentInChildren<RawImage>().texture = manager.CurrentLocation.thumbnail;
    //            yield return infotext;
    //        }
    //    }
    //}
}
