using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePanelUpdater : MonoBehaviour
{

    public Texture2D[] imageList;
    public float imageLifetime = 3.0f;

    private RawImage activeImage;
    private int imageIndex = 0;
    private int arraySize;
    private float lifetimeCountdown;

    void Start()
    {
        //var currentLocation = LocationController.GetInstance().CurrentLocation;
        //Uri uri = new Uri(ConstantsNS.Constants.FTPLocationPath + currentLocation.name + "/Images/");
        //imageList = FTPHandler.DownloadAllImagesFromFTP(uri).ToArray();
        //imageList = transform.parent.GetComponent<LocationDetails>().imagePanel;

        activeImage = GetComponentInChildren<RawImage>();
        arraySize = imageList.Length;
        SetTexture();
        //activeImage.texture = imageList[0];
        //lifetimeCountdown = imageLifetime;
    }

    private void Update()
    {
        lifetimeCountdown -= Time.deltaTime;
        if (lifetimeCountdown <= 0)
        {
            imageIndex++;
            SetTexture();
        }
    }

    public void NextImage()
    {
        imageIndex++;
        SetTexture();
    }

    public void PreviousImage()
    {
        imageIndex--;
        SetTexture();
    }
    private void SetTexture()
    {
        if (arraySize > 0)
        {
            if (imageIndex < 0)
            {
                imageIndex = (arraySize - 1);
            }
            else if (imageIndex >= arraySize)
            {
                imageIndex = 0;
            }
            activeImage.texture = imageList[imageIndex];
            lifetimeCountdown = imageLifetime;
        }
    }

}
