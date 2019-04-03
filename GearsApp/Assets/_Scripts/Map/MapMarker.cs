using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMarker : MonoBehaviour
{
    public Location MapMarkerLocation { get; set; }

    private void Start()
    {
        if(MapMarkerLocation != null)
        {

        }
    }

    public void UpdateData()
    {
        if(MapMarkerLocation != null)
            GetComponentInChildren<TextMesh>().text = MapMarkerLocation.name;
        //Add other relevant info :) 
    }

    public void TestChangeText()
    {
        GetComponentInChildren<TextMesh>().text = "Test";
    }

}
