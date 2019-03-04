using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstantsNS;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FavoriteManager : MonoBehaviour
{

    public void CallAddToFavorite()
    {
        StartCoroutine(AddToFavorite());
    }

    public void CallRemoveFromFavorite()
    {

    }

    IEnumerator AddToFavorite()
    {
        Location loc = GetComponent<LocationListItem>().location;
        bool isFavorite = GetComponentInChildren<Toggle>().isOn;
        UserManager manager = UserManager.GetInstance();

        WWWForm form = new WWWForm();
        form.AddField("number", manager._currentUser.telephonenr);
        form.AddField("location_id", loc.location_ID);

        string path;
        if (isFavorite)
            path = Constants.PhpPath + "addfavorite.php";
        else
            path = Constants.PhpPath + "removefavorite.php";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(path, form))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                string req = webRequest.downloadHandler.text;
                Debug.Log("FAVORITE: + " + req);
                LocationsManager locManager = LocationsManager.GetInstance();
                PHPStatusHandler handler = JsonConvert.DeserializeObject<PHPStatusHandler>(req, Constants.JsonSettings);

                if (handler.statusCode == true)
                {
                    loc.favorite = isFavorite;
                }
            }
        }
    }
}

