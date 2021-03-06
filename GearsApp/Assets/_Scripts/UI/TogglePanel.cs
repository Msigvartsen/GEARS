﻿using UnityEngine;

/// <summary>
/// Class TogglePanel.
/// Hides background when Map tab is active to display Map.
/// Implements the <see cref="UnityEngine.MonoBehaviour" />
/// </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class TogglePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    private int backgroundBottomRect = 0;
    [SerializeField]
    private GameObject mapPanel;

    private RectTransform rectTransform;

    private void Start()
    {
        if(panel != null)
            rectTransform = panel.GetComponent<RectTransform>();

        backgroundBottomRect = 2350;
    }

    private void Update()
    {
        if(GetComponent<CanvasGroup>().alpha == 1 && mapPanel.activeSelf)
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, backgroundBottomRect);
        }
        else
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, 0);
        }
    }
}
