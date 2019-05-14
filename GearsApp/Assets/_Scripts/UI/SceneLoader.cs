using UnityEngine;

/// <summary>
/// Handles logic with swapping scenes and opening correct page when a scene is reloaded.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private UIPanelController uiController;
    [SerializeField]
    private GameObject rootPanel;

    /// <summary>
    /// Loads scene based on index
    /// </summary>
    /// <param name="index">Scene Index</param>
    public void LoadScene(int index)
    {
        LoadingScreen.LoadScene(index);
    }
    /// <summary>
    /// Loads Scene based on Name
    /// </summary>
    /// <param name="sceneName">Name of Scene</param>
    public void LoadScene(string sceneName)
    {
        LoadingScreen.LoadScene(sceneName);
    }

    /// <summary>
    /// Opens previous Page. If current page is the root in current scene - go back to previous Scene.
    /// </summary>
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

    public void ChangeSceneAndSetPage(string page)
    {
        UserController.GetInstance().PreviousPage = page;
        LoadScene(GEARSApp.Constants.MainScene);
    }
}
