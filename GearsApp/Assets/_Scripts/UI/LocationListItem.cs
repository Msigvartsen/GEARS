using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationListItem : MonoBehaviour
{
    private GameObject parent;
    private Button listButton;
    public Location location;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        GetComponentInChildren<Text>().text = location.name;
        listButton = GetComponentInChildren<Button>();
        listButton.onClick.AddListener(OpenLocationTab);
    }

    private void OpenLocationTab()
    {
        LocationController manager = LocationController.GetInstance();
        manager.CurrentLocation = location;
        LoadingScreen.LoadScene("Location");
    }
}
