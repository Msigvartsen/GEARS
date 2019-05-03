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
}
