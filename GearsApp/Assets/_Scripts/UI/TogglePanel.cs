using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private int backgroundBottomRect = 0;
    [SerializeField]
    private Toggle toggleMapButton;

    private RectTransform rectTransform;

    private void Start()
    {
        if(panel != null)
            rectTransform = panel.GetComponent<RectTransform>();

        backgroundBottomRect = 2400;
    }

    private void Update()
    {
        if(GetComponent<CanvasGroup>().alpha == 1 && toggleMapButton.isOn)
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, backgroundBottomRect);
        }
        else
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0);
        }
    }
}
