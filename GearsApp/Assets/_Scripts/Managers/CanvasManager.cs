using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    private Canvas[] canvasList;
    private Canvas activeCanvas;

    private void Start ()
    {
        canvasList = GetComponentsInChildren<Canvas>();
        activeCanvas = canvasList[0];
    }

    public void ChangeCanvas (string newCanvasName)
    {
        foreach(Canvas canvas in canvasList)
        {
            if (canvas.name == newCanvasName)
            {
                activeCanvas.enabled = false;
                canvas.enabled = true;
                activeCanvas = canvas;
            }
        }
    }

    public void ChangeUIPanel ()
    {

    }
}
