using UnityEngine;
using GEARSApp;
using TMPro;

/// <summary>
/// Script handles user login.
/// Sends requests to database to verify successfull logins. 
/// </summary>
public class Login : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField]
    private TMP_InputField usernameField;
    [SerializeField]
    private TMP_InputField passwordField;

    [SerializeField]
    private PopupNotification popupNotification;

    /// <summary>
    /// Create a form and start a Coroutine which sends a POST request to database.
    /// Checks and verifies user / password.
    /// </summary>
    public void CallLogin()
    {
        string path = Constants.PhpPath + "login.php";
        WWWForm form = new WWWForm();
        form.AddField("user", usernameField.text);
        form.AddField("password", passwordField.text);
        StartCoroutine(WebRequestController.PostRequest<WebResponse<User>>(path, form, InitLogin));
    }

    /// <summary>
    /// Action Method.
    /// If reponse is successfull (Login succeeded), Set up all controllers before loading main scene.
    /// </summary>
    /// <param name="obj"></param>
    private void InitLogin(WebResponse<User> obj)
    {
        if(obj.handler.statusCode == false)
        {
            if (popupNotification != null)
            {
                popupNotification.ShowPopup(obj.handler.text);
            }
            return;
        }

        UserController manager = UserController.GetInstance();
        manager.CurrentUser = obj.objectList.ToArray()[0];

        LocationController.GetInstance().CallGetFavorites();
        StationController.GetInstance().CallUserProgressRequest();
        TrophyController.GetInstance().CallCollectedTrophies();
        ModelController.GetInstance().CallGetFoundModel();
        LoadingScreen.LoadScene(GEARSApp.Constants.MainScene);
    }
}


