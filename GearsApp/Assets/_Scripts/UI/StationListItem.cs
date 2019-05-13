using UnityEngine;
using UnityEngine.UI;

public class StationListItem : MonoBehaviour
{
    public Station Station { get; set; }

    private GameObject parent;
    private Button listButton;
    [Header("Image Panels")]
    [SerializeField]
    private RawImage lockImageGameObject;
    [SerializeField]
    private RawImage thumbnail;

    [Header("Textures")]
    [SerializeField]
    private Texture2D lockedImage;
    [SerializeField]
    private Texture2D unlockedImage;

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Station " + Station.station_NR.ToString();
        //thumbnail.texture = GetStationThumbnail();
        listButton = GetComponentInChildren<Button>();
        listButton.onClick.AddListener(OpenStationTab);
    }

    void OpenStationTab()
    {
        StationController manager = StationController.GetInstance();
        manager.CurrentStation = Station;
        // LoadingScreen.LoadScene("Station");
    }

    public void UpdateStationStatus()
    {
        if (!Station.visited)
        {
            GetComponentInChildren<Button>().enabled = false;
            GetComponentInChildren<CanvasGroup>().alpha = 0.5f;
            GetComponentInChildren<Text>().text = "Locked";

            if (lockedImage != null)
                lockImageGameObject.texture = lockedImage;
        }
        else
        {
            GetComponentInChildren<Button>().enabled = true;
            GetComponentInChildren<CanvasGroup>().alpha = 1f;
            GetComponentInChildren<Text>().text = "Unlocked";

            if (unlockedImage != null)
                lockImageGameObject.texture = unlockedImage;
        }
    }
}
