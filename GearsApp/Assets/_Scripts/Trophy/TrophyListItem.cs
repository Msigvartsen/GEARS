using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script attached to TrophyItem Prefab in Unity.
/// Keeps track of all information of the current trophy, and updates UI elements.
/// </summary>
public class TrophyListItem : MonoBehaviour
{
    public Trophy CurrentTrophy { get; set; }

    [SerializeField]
    private TMPro.TextMeshProUGUI trophyName;
    [SerializeField]
    private RawImage trophyImage;

    public GameObject overlayPanel;

    /// <summary>
    /// Ran before the first frame.
    /// Set the image and text for trophy.
    /// </summary>
    private void Start()
    {
        if (trophyImage != null)
            trophyImage.texture = CurrentTrophy.image;
        if (trophyName != null)
            trophyName.text = CurrentTrophy.trophyname;
    }

    /// <summary>
    /// Updates Popup window with correct details and image.
    /// </summary>
    public void TrophyDetailPopup()
    {
        GameObject popupWindow = GameObject.FindGameObjectWithTag("TrophyPopupContainer");
        popupWindow.GetComponent<Animator>().Play("Fade-in");

        if (CurrentTrophy != null)
        {
            GameObject popupInfo = GameObject.FindGameObjectWithTag("TrophyPopupInfo");
            var textComponents = popupInfo.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
            foreach (var component in textComponents)
            {
                if (component.name == "Name")
                    component.text = CurrentTrophy.trophyname;
                if (component.name == "Details")
                    component.text = CurrentTrophy.details;
            }
            popupInfo.GetComponentInChildren<RawImage>().texture = CurrentTrophy.image;
        }
    }
}
