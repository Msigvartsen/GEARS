using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePanelUpdater : MonoBehaviour
{

    public Texture[] imageList;
    public float imageLifetime = 3.0f;

    private RawImage activeImage;
    private int imageIndex = 0;
    private int arraySize;
    private float lifetimeCountdown;

    void Start()
    {
        activeImage = GetComponentInChildren<RawImage>();
        arraySize = imageList.Length;
        activeImage.texture = imageList[0];
        lifetimeCountdown = imageLifetime;
    }

    private void Update()
    {
        lifetimeCountdown -= Time.deltaTime;
        if(lifetimeCountdown <= 0)
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
