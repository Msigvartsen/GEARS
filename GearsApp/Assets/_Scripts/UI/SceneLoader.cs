using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        //string previousScene = SceneManager.GetActiveScene().name;
        //if (previousScene == "Main" || previousScene == "LocationNew")
        //{
        //    UserController.GetInstance().PreviousScene = previousScene;
        //}
        //else
        //{
        //    UserController.GetInstance().PreviousScene = "Main";
        //}

        //UserController.GetInstance().PreviousScene = SceneManager.GetActiveScene().name;
        LoadingScreen.LoadScene(sceneName);
    }

    public void PreviousPage()
    {
        string previousScene = string.Empty;

        if (UserController.GetInstance() != null)
            previousScene = UserController.GetInstance().PreviousScene;

        if (rootPanel.GetComponent<CanvasGroup>().alpha == 1)
            LoadScene(previousScene);
        else
            uiController.PanelAnimPreviousPanel();
    }

    public void ChangePanel(string nextPanel)
    {
        UserController.GetInstance().PreviousPage = nextPanel;
        LoadScene("Main");
        //LoadingScreen.LoadScene("Main");
    }
}
