using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapInputDetector : MonoBehaviour
{
    public Camera mapCamera;
    public Text locationName;
    public GameObject map;
    [Header("Temporary solution for testing")]
    public GameObject locationCanvas;
    public GameObject locationListCanvas;
    public Camera mainCamera;
    public GameObject confirmationButton;
    [Range(0, 3f)] public float fadeSpeed = 1f;

    private GameObject selectedObject;
    private Vector2d[] markerLatLongs;
    private AbstractMap abstractMap;
    private bool showButton = false;

    // Start is called before the first frame update
    void Start()
    {
        markerLatLongs = new Vector2d[map.GetComponent<SpawnOnMap>()._locationStrings.Length];
        for (int i = 0; i < map.GetComponent<SpawnOnMap>()._locationStrings.Length; i++)
        {
            var locationString = map.GetComponent<SpawnOnMap>()._locationStrings[i];
            markerLatLongs[i] = Conversions.StringToLatLon(locationString);
        }

        abstractMap = map.GetComponent<AbstractMap>();
        confirmationButton.GetComponent<CanvasGroup>().alpha = 0;

        locationName.text = "Select a marker";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1)
            {
                // Touch to select an object
                if (touch.phase == TouchPhase.Ended)
                    SelectMarker();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            print("mouse click detected");
            SelectMarker(true);
        }

        if (showButton)
            ShowConfirmationButton();
        else
            HideConfirmationButton();

    }

    private void SelectMarker(bool mouse = false)
    {
        Vector3 position;
        if (mouse)
        {
            position = Input.mousePosition;
        }
        else
        {
            Touch touch = Input.GetTouch(0);
            position = touch.position; 
        }

        Ray ray = mapCamera.ScreenPointToRay(position);
        RaycastHit hit = new RaycastHit();

        // Raycast to see if the user clicked a marker
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.gameObject.name);

            if (hit.collider.gameObject.name == "Marker(Clone)")
            {
                selectedObject = hit.transform.gameObject;
                
                // Check which element in the abstract map which were hit
                for (int i = 0; i < map.GetComponent<SpawnOnMap>()._locationStrings.Length; i++)
                {
                    if (selectedObject.transform.localPosition == abstractMap.GeoToWorldPosition(markerLatLongs[i], true))
                    {
                        showButton = true;
                        locationName.text = markerLatLongs[i].x.ToString() + "  "  + markerLatLongs[i].y.ToString();
                        // Now loop through every latlong in the Location database entries
                        // to open the right information page according to the marker hit

                        //OpenInformationPanel();
                    }
                }
            }
        }
        else
        {
            //showButton = false;
        }
    }

    public void ShowConfirmationButton()
    {
        if (confirmationButton.GetComponent<CanvasGroup>().alpha < 1)
            confirmationButton.GetComponent<CanvasGroup>().alpha += Time.deltaTime * fadeSpeed;
    }

    public void HideConfirmationButton()
    {
        if (confirmationButton.GetComponent<CanvasGroup>().alpha > 0)
            confirmationButton.GetComponent<CanvasGroup>().alpha -= Time.deltaTime * fadeSpeed;
    }

    public void SwitchCameras()
    {
        mapCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }
}
