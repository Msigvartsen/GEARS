using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System;

public class LocationDetails : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(UpdateInfoText());
    }

    IEnumerator UpdateInfoText()
    {
        LocationController manager = LocationController.GetInstance();

        GameObject gameObject = GameObject.FindGameObjectWithTag("HeaderButton");
        gameObject.GetComponentInChildren<Text>().text = manager.CurrentLocation.name;

        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform obj in children)
        {
            if (obj.name == "InfoPanel")
            {
                GameObject infoPanel = obj.gameObject;
                Uri uri = new Uri(ConstantsNS.Constants.FTPLocationPath + manager.CurrentLocation.name + "/Information/basicinfo.txt");
                string infotext = FTPHandler.DownloadTextFromFTP(uri);
                infoPanel.GetComponentInChildren<Text>().text = infotext;//manager.CurrentLocation.information;
                string path = ConstantsNS.Constants.FTPLocationPath + manager.CurrentLocation.name + "/Images/hinn.jpg";
                Uri uri2 = new Uri(path);
                infoPanel.GetComponentInChildren<RawImage>().texture = FTPHandler.DownloadImageFromFTP(uri2);
                yield return infotext;
            }
        }
    }
}
