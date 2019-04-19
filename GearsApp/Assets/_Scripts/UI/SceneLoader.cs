using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{

    [SerializeField]
    private UIPanelController uiController;
    [SerializeField]
    private GameObject rootPanel;

    public void LoadScene(int index)
    {
        LoadingScreen.LoadScene(index);
    }
    public void LoadScene(string sceneName)
    {
        if (sceneName == "SelectLocation")
            StartCoroutine(LocationServiceNS.LocationService.StartLocationService());

        LoadingScreen.LoadScene(sceneName);
    }

    public void PreviousPage()
    {
        if (rootPanel.GetComponent<CanvasGroup>().alpha == 1)
            LoadingScreen.LoadScene("Main");
        else
            uiController.PanelAnimPreviousPanel();
    }
}
