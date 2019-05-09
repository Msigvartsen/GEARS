using ConstantsNS;
using System.Collections.Generic;
using UnityEngine;

public class TrophyController : MonoBehaviour
{
    private static TrophyController instance;

    public List<CollectedTrophy> CollectedTrophies { get; set; }
    public List<Trophy> TrophyList { get; set; }

    public static TrophyController GetInstance()
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
        CallRequestTrophies();
    }

    public void CallRequestTrophies()
    {
        string path = Constants.PhpPath + "trophies.php";
        StartCoroutine(WebRequestController.GetRequest<WebResponse<Trophy>>(path, InitTrophyList));
    }
    public void CallCollectedTrophies()
    {
        var currentUser = UserController.GetInstance().CurrentUser;

        WWWForm form = new WWWForm();
        form.AddField("number", currentUser.telephonenr);
        string path = Constants.PhpPath + "collectedtrophies.php";
        StartCoroutine(WebRequestController.PostRequest<WebResponse<CollectedTrophy>>(path, form, GetCollectedTrophies));
    }

    private void InitTrophyList(WebResponse<Trophy> response)
    {
        if (!WebRequestController.CheckResponse(response.handler))
            return;

        if (TrophyList == null)
            TrophyList = new List<Trophy>();

        Debug.Log(response.handler.text);

        foreach (Trophy trophy in response.objectList)
        {
            Texture2D tex = Resources.Load<Texture2D>("_Trophies/" + trophy.trophyname);
            trophy.image = tex;
            TrophyList.Add(trophy);
        }
    }

    private void GetCollectedTrophies(WebResponse<CollectedTrophy> response)
    {
        if (!WebRequestController.CheckResponse(response.handler))
            return;

        if (CollectedTrophies == null)
            CollectedTrophies = new List<CollectedTrophy>();
        else
            CollectedTrophies.Clear();

        foreach (CollectedTrophy collectedtrophy in response.objectList)
            CollectedTrophies.Add(collectedtrophy);
    }

    public void CallAddCollectedTrophy(Trophy collectedTrophy)
    {
        var currentUser = UserController.GetInstance().CurrentUser;

        WWWForm form = new WWWForm();
        form.AddField("number", currentUser.telephonenr);
        form.AddField("trophyname", collectedTrophy.trophyname);
        string path = Constants.PhpPath + "addcollectedtrophy.php";

        StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler, Trophy>(path,
                                                                                  form,
                                                                                  UpdateCollectedTrophyList,
                                                                                  collectedTrophy));

    }

    private void UpdateCollectedTrophyList(PHPStatusHandler handler, Trophy collectedTrophy)
    {
        if (!WebRequestController.CheckResponse(handler))
            return;

        GameObject gameObject = GameObject.FindGameObjectWithTag("TrophyListManager");
        var go = gameObject.GetComponent<TrophyListManager>();
        CreateNewCollectedTrophy(collectedTrophy);
        go.UpdateTrophyList(collectedTrophy.trophyname);

    }
    private void CreateNewCollectedTrophy(Trophy collectedTrophy)
    {
        int number = UserController.GetInstance().CurrentUser.telephonenr;
        string trophyname = collectedTrophy.trophyname;

        CollectedTrophies.Add(new CollectedTrophy
        {
            telephonenr = number,
            trophyname = trophyname
        });
    }
}
