using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrophyListItem : MonoBehaviour
{
    public Trophy CurrentTrophy { get; set; }

    [SerializeField]
    private TMPro.TextMeshProUGUI trophyName;
    [SerializeField]
    private RawImage trophyImage;

    private void Start()
    {
        if (trophyImage != null)
            trophyImage.texture = CurrentTrophy.image;
        if (trophyName != null)
            trophyName.text = CurrentTrophy.trophyname;
    }

    public void TrophyDetailPopup()
    {
        GameObject popupWindow = GameObject.FindGameObjectWithTag("TrophyPopupContainer");
        popupWindow.GetComponent<Animator>().Play("Fade-in");

        if (CurrentTrophy != null)
        {
            GameObject popupInfo = GameObject.FindGameObjectWithTag("TrophyPopupInfo");
            var textComponents = popupInfo.GetComponentsInChildren<TMPro.TextMeshProUGUI>();//.text = CurrentTrophy.trophyname;
            foreach(var component in textComponents)
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
