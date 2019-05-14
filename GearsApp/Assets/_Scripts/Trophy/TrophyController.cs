using GEARSApp;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Singleton Class. Keeps track of all Trophies in application as well as starting requests
/// to update / fetch trophies from database.
/// </summary>
public class TrophyController : MonoBehaviour
{
    private static TrophyController instance;

    public List<CollectedTrophy> CollectedTrophies { get; set; }
    public List<Trophy> TrophyList { get; set; }

    public static TrophyController GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// Creates singleton object.
    /// Starts Coroutine via WebRequestController to fetch trophies.
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
        CallRequestTrophies();
    }

    /// <summary>
    /// Starts a coroutine to WebRequestController to fetch all trophies.
    /// </summary>
    public void CallRequestTrophies()
    {
        string path = Constants.PhpPath + "trophies.php";
        StartCoroutine(WebRequestController.GetRequest<WebResponse<Trophy>>(path, InitTrophyList));
    }

    /// <summary>
    /// Creates a form before starting a coroutine to WebRequestController to get all CollectedTrophies
    /// associated with the current user.
    /// </summary>
    public void CallCollectedTrophies()
    {
        var currentUser = UserController.GetInstance().CurrentUser;

        WWWForm form = new WWWForm();
        form.AddField("number", currentUser.telephonenr);
        string path = Constants.PhpPath + "collectedtrophies.php";
        StartCoroutine(WebRequestController.PostRequest<WebResponse<CollectedTrophy>>(path, form, GetCollectedTrophies));
    }
    /// <summary>
    /// Creates a form before starting a coroutine to WebRequestController to add a trophy to the
    /// current user. 
    /// </summary>
    /// <param name="collectedTrophy">Trophy to update users collected trophies</param>
    public void CallAddCollectedTrophy(Trophy collectedTrophy)
    {
        var currentUser = UserController.GetInstance().CurrentUser;

        WWWForm form = new WWWForm();
        form.AddField("number", currentUser.telephonenr);
        form.AddField("trophyname", collectedTrophy.trophyname);
        string path = Constants.PhpPath + "addcollectedtrophy.php";

        StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler, Trophy>(path,form,
                                                                                  UpdateCollectedTrophyList,
                                                                                  collectedTrophy));
    }
    /// <summary>
    /// Action Method - Ran from within WebRequestController. 
    /// Checks the response for valid values before fetching trophy image and adding them to TrophyList.
    /// </summary>
    /// <param name="response">Response from Database Query (Parsed from JSON)</param>
    private void InitTrophyList(WebResponse<Trophy> response)
    {
        if (!WebRequestController.CheckValidResponse(response.handler))
            return;

        if (TrophyList == null)
            TrophyList = new List<Trophy>();

        foreach (Trophy trophy in response.objectList)
        {
            Texture2D tex = Resources.Load<Texture2D>("_Trophies/" + trophy.trophyname);
            trophy.image = tex;
            TrophyList.Add(trophy);
        }
    }

    /// <summary>
    /// Action Method - Ran from within WebRequestController.
    /// Checks the response for valid values before creating a new list with all collected trophies
    /// associated with the user.
    /// </summary>
    /// <param name="response">Response from Database Query (JSON format)</param>
    private void GetCollectedTrophies(WebResponse<CollectedTrophy> response)
    {
        if (!WebRequestController.CheckValidResponse(response.handler))
            return;

        //Create new list, or clear it (in case of duplicates)
        if (CollectedTrophies == null)
            CollectedTrophies = new List<CollectedTrophy>();
        else
            CollectedTrophies.Clear();

        foreach (CollectedTrophy collectedtrophy in response.objectList)
            CollectedTrophies.Add(collectedtrophy);
    }

    /// <summary>
    /// Action Method - Ran from within WebRequestController.
    /// Checks the response for valid values before accessing Trophy List Manager script,
    /// and adding and updating new collected trophies to UI.
    /// </summary>
    /// <param name="handler">Status from request</param>
    /// <param name="collectedTrophy">Trophy the user has collected</param>
    private void UpdateCollectedTrophyList(PHPStatusHandler handler, Trophy collectedTrophy)
    {
        if (!WebRequestController.CheckValidResponse(handler))
            return;

        GameObject gameObject = GameObject.FindGameObjectWithTag("TrophyListManager");
        var go = gameObject.GetComponent<TrophyListManager>();
        CreateNewCollectedTrophy(collectedTrophy);
        go.UpdateTrophyList(collectedTrophy.trophyname);
    }

    /// <summary>
    /// Created a new Collected Trophy with current user telephonenumber and name of trophy.
    /// This is added to the users CollectedTrophy list and updating UI elements.
    /// </summary>
    /// <param name="collectedTrophy">Trophy the user has collected.</param>
    private void CreateNewCollectedTrophy(Trophy collectedTrophy)
    {
        int number = UserController.GetInstance().CurrentUser.telephonenr;
        string trophyname = collectedTrophy.trophyname;

        CollectedTrophy newCollectedTrophy = new CollectedTrophy
        {
            telephonenr = number,
            trophyname = trophyname
        };

        foreach(var trophy in CollectedTrophies)
        {
            //Return empty is trophy is already collected.
            if (newCollectedTrophy.trophyname == trophy.trophyname)
                return;
        }
        CollectedTrophies.Add(newCollectedTrophy);
    }

    public void AddCollectedTrophyByName(string trophyName)
    {
        foreach(Trophy trophy in TrophyList)
        {
            if(trophy.trophyname == trophyName)
            {
                CallAddCollectedTrophy(trophy);
            }
        }
    }
}
