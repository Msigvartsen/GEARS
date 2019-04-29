using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationListItem : MonoBehaviour
{
    public Station Station { get; set; }

    private GameObject parent;
    private Button listButton;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Station " + Station.station_NR.ToString();
        listButton = GetComponentInChildren<Button>();
        listButton.onClick.AddListener(OpenStationTab);
    }

    void OpenStationTab()
    {
        StationController manager = StationController.GetInstance();
        manager.CurrentStation = Station;
        // LoadingScreen.LoadScene("Station");
    }
}
