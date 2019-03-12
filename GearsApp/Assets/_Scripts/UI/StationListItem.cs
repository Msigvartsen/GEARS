using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationListItem : MonoBehaviour
{
    public Station station;

    private GameObject parent;
    private Button listButton;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Text>().text = "Station " + station.station_NR.ToString();
        listButton = GetComponentInChildren<Button>();
        listButton.onClick.AddListener(OpenStationTab);
    }

    void OpenStationTab()
    {
        StationController manager = StationController.GetInstance();
        manager.CurrentStation = station;
        // LoadingScreen.LoadScene("Station");
    }
}
