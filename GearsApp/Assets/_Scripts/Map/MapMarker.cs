using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapMarker : MonoBehaviour
{
    public Location MapMarkerLocation { get; set; }
    private GameObject _popupWindow;

    private void Start()
    {
        //_popupWindow = GameObject.FindGameObjectWithTag("LocationPopupInfo");
        //_popupWindow.SetActive(false);

        if (MapMarkerLocation != null)
        {

        }
    }

    public void UpdateData()
    {
        if (MapMarkerLocation != null)
            GetComponentInChildren<TextMesh>().text = MapMarkerLocation.name;
        Debug.Log("MapMarkerName: :: : :  " + MapMarkerLocation.name);
        //Add other relevant info :) 
    }

    public void TestChangeText()
    {
        GetComponentInChildren<TextMesh>().text = "Test";
    }

    public void SpawnLocationPopupInfo()
    {
        _popupWindow = GameObject.FindGameObjectWithTag("LocationPopupContainer");
        _popupWindow.GetComponent<PopupPanel>().SetPopupPanelActive(true);
        if (MapMarkerLocation != null)
        {
            GameObject popupInfo = GameObject.FindGameObjectWithTag("LocationPopupInfo");
            popupInfo.GetComponentInChildren<Text>().text = MapMarkerLocation.name;
            popupInfo.GetComponentInChildren<RawImage>().texture = MapMarkerLocation.thumbnail;
        }
        Debug.Log(MapMarkerLocation.name);
        //string path = ConstantsNS.Constants.FTPLocationPath + MapMarkerLocation.name + "/Images/img.jpg";
        //Uri uri2 = new Uri(path);
        //_popupWindow.GetComponent<RawImage>().texture = MapMarkerLocation.thumbnail;
    }

}
