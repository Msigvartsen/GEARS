using UnityEngine;
using GEARSApp;

public class UserController : MonoBehaviour
{
    private static UserController instance;
    public User CurrentUser { get; set; }
    [SerializeField]
    private int experienceCapPerLevel = 100;

    //To load correct page when changing scene
    public string PreviousPage { get; set; }
    public string PreviousScene { get; set; }

    public static UserController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        PreviousPage = Constants.MainScene;
    }

    public void CallUpdateUserExpAndLevel()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", CurrentUser.telephonenr);
        form.AddField("level", CurrentUser.level);
        form.AddField("experience", CurrentUser.experience);
        string path = Constants.PhpPath + "updateuser.php";

        StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler>(path, form, UpdateLevelAndExperience));
    }

    public void CallUpdateUserPicture(int mediaID)
    {
        CurrentUser.media_ID = mediaID;
        WWWForm form = new WWWForm();
        form.AddField("number", CurrentUser.telephonenr);
        form.AddField("media_ID", CurrentUser.media_ID);
        string path = Constants.PhpPath + "updateuserpicture.php";

        StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler>(path, form, UpdateUserPicture));
    }

    public void CallDeleteUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", CurrentUser.telephonenr);
        string path = Constants.PhpPath + "deleteuser.php";

        StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler>(path, form, DeleteAndLogout));
    }

    public void UpdateUserExperience(int experience)
    {
        CurrentUser.experience += experience;
        if (CurrentUser.experience >= experienceCapPerLevel)
        {
            UpdateUserLevel();
        }
    }

    public void UpdateUserLevel()
    {
        CurrentUser.level++;
        if(CurrentUser.experience >= experienceCapPerLevel)
            CurrentUser.experience -= experienceCapPerLevel; //resets experience bar
    }

    public void LogOut()
    {
        CurrentUser = null;
        LocationController.GetInstance().ResetFavorites();
        LoadingScreen.LoadScene(GEARSApp.Constants.RegistrationAndLoginScene);
    }
   
    private void UpdateUserPicture(PHPStatusHandler handler)
    {
        if (WebRequestController.CheckValidResponse(handler))
            return;

        //Add popup notification telling user that their picture has been updated?
    }

    private void DeleteAndLogout(PHPStatusHandler handler)
    {
        if (!WebRequestController.CheckValidResponse(handler))
            return;

        LogOut();
    }

    private void UpdateLevelAndExperience(PHPStatusHandler handler)
    {
        if (!WebRequestController.CheckValidResponse(handler))
            return;

        //Refresh UI from here?
    }
}
