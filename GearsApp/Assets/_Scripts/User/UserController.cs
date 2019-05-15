using UnityEngine;
using GEARSApp;

/// <summary>
/// Singleton class. Keeps track of the current user and sends requests to database for updating user information.
/// </summary>
public class UserController : MonoBehaviour
{
    private static UserController instance;
    public User CurrentUser { get; set; }
    [SerializeField]
    private int experienceCapPerLevel = 100;

    //To load correct page when changing scene
    public string PreviousPage { get; set; }
    public string PreviousScene { get; set; }

    /// <summary>
    /// Function to retreive singleton instance
    /// </summary>
    /// <returns>Returns instance of singleton object</returns>
    public static UserController GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Is called when the script instance is being loaded. 
    /// Creates singleton object if it does not exist.
    /// </summary>
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

    /// <summary>
    /// Creates a form and starts a Coroutine with a POST request to update the users experience and level.
    /// </summary>
    public void CallUpdateUserExpAndLevel()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", CurrentUser.telephonenr);
        form.AddField("level", CurrentUser.level);
        form.AddField("experience", CurrentUser.experience);
        string path = Constants.PhpPath + "updateuser.php";

        StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler>(path, form, UpdateLevelAndExperience));
    }

    /// <summary>
    /// Creates a form and starts a Coroutine with POST request to update the users profile picture.
    /// </summary>
    /// <param name="mediaID">ID of new profile picture</param>
    public void CallUpdateUserPicture(int mediaID)
    {
        CurrentUser.media_ID = mediaID;
        WWWForm form = new WWWForm();
        form.AddField("number", CurrentUser.telephonenr);
        form.AddField("media_ID", CurrentUser.media_ID);
        string path = Constants.PhpPath + "updateuserpicture.php";

        StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler>(path, form, UpdateUserPicture));
    }

    /// <summary>
    /// Creates a form and starts a Coroutine with POST request to delete current user from database. (Cannot be undone).
    /// </summary>
    public void CallDeleteUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("number", CurrentUser.telephonenr);
        string path = Constants.PhpPath + "deleteuser.php";

        StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler>(path, form, DeleteAndLogout));
    }

    /// <summary>
    /// Updates the users experience points.
    /// </summary>
    /// <param name="experience">Experience to add to the user</param>
    public void UpdateUserExperience(int experience)
    {
        CurrentUser.experience += experience;
        if (CurrentUser.experience >= experienceCapPerLevel)
        {
            UpdateUserLevel();
        }
    }

    /// <summary>
    /// Update the users level and check if any trophies awarded for current level.
    /// </summary>
    public void UpdateUserLevel()
    {
        CurrentUser.level++;
        if (CurrentUser.experience >= experienceCapPerLevel)
            CurrentUser.experience -= experienceCapPerLevel; //resets experience bar

        CheckLevelTrophy();
    }

    /// <summary>
    /// Checks current level and if any trophies are awarded. 
    /// </summary>
    private void CheckLevelTrophy()
    {
        var trophyController = TrophyController.GetInstance();
        switch (CurrentUser.level)
        {
            case 1:
                trophyController.AddCollectedTrophyByName("Level 1");
                break;
            case 5:
                trophyController.AddCollectedTrophyByName("Level 5");
                break;
            case 10:
                trophyController.AddCollectedTrophyByName("Level 10");
                break;
        }
    }

    /// <summary>
    /// Log out the user and reset favorites.
    /// </summary>
    public void LogOut()
    {
        CurrentUser = null;
        LocationController.GetInstance().ResetFavorites();
        LoadingScreen.LoadScene(GEARSApp.Constants.RegistrationAndLoginScene);
    }
   
    /// <summary>
    /// Action Method. Ran from within a Coroutine. Checks valid response from database.
    /// </summary>
    /// <param name="handler"></param>
    private void UpdateUserPicture(PHPStatusHandler handler)
    {
        if (WebRequestController.CheckValidResponse(handler))
            return;

        //Add popup notification telling user that their picture has been updated?
    }

    /// <summary>
    /// Action Method. Ran from within Coroutine. Checks valid response from database.
    /// If response is valid, log out after deleting user from database.
    /// </summary>
    /// <param name="handler"></param>
    private void DeleteAndLogout(PHPStatusHandler handler)
    {
        if (!WebRequestController.CheckValidResponse(handler))
            return;

        LogOut();
    }

    /// <summary>
    /// Action Method. Ran from withing a Coroutine.
    /// Checks valid response from database.
    /// </summary>
    /// <param name="handler"></param>
    private void UpdateLevelAndExperience(PHPStatusHandler handler)
    {
        if (!WebRequestController.CheckValidResponse(handler))
            return;

        //Refresh UI from here?
    }
}
