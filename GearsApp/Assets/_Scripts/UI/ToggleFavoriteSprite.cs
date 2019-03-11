using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ToggleFavoriteSprite : MonoBehaviour
{
    private void Start()
    {
        ToggleFavorite();
    }
    
    public void ToggleFavorite()
    {
        LocationController manager = LocationController.GetInstance();
        bool isFavorite = GetComponent<Toggle>().isOn;

        if (isFavorite)
        {
            GetComponent<Image>().sprite = manager.favoriteFilled;
        }
        else
        {
            GetComponent<Image>().sprite = manager.favoriteOutline;
        }
    }
}
