﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstantsNS;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FavoriteController : MonoBehaviour
{
    [SerializeField]
    private LocationListItem listItem;
    [SerializeField]
    private GameObject imagePanel; //For filled/unfilled favorite icon
    [SerializeField]
    private Toggle toggleButton;

    private void Start()
    {
        bool active = listItem.location.favorite;
        if (toggleButton != null)
            toggleButton.isOn = active;
        imagePanel.SetActive(active);
    }

    public void CallAddToFavorite()
    {
        StartCoroutine(AddToFavorite());
    }

    IEnumerator AddToFavorite()
    {
        Debug.Log("CALL FAV");
        Location loc = listItem.location;
        bool isFavorite = toggleButton.isOn;
        UserController manager = UserController.GetInstance();
        imagePanel.SetActive(isFavorite);

        WWWForm form = new WWWForm();
        form.AddField("number", manager.CurrentUser.telephonenr);
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
                imagePanel.SetActive(false);
            }
            else
            {
                string req = webRequest.downloadHandler.text;
                Debug.Log("FAVORITE: + " + req);
                LocationController locManager = LocationController.GetInstance();
                PHPStatusHandler handler = JsonConvert.DeserializeObject<PHPStatusHandler>(req, Constants.JsonSettings);

                if (handler.statusCode == true)
                {
                    loc.favorite = isFavorite;
                }
            }
        }
    }
}

