using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ImagePanelUpdater
/// This class takes an array of images which it then will display one by one.After imageLifetime has run out,
/// the next image will appear.
/// This script is attached to the "ImagePanel" prefab in the Unity Project.
/// </summary>
public class ImagePanelUpdater : MonoBehaviour
{
    public Texture2D[] ImageList { get; set; }
    public float imageLifetime = 4.0f;

    private RawImage activeImage;
    private int imageIndex = 0;
    private int arraySize;
    private float lifetimeCountdown;
 
    /// <summary>
    /// Initialize the ImagePanel and set first image.
    /// </summary>
    void Start()
    {
        ImageList = LocationController.GetInstance().CurrentLocation.images;
        activeImage = GetComponentInChildren<RawImage>();
        arraySize = ImageList.Length;
        SetTexture();
    }

    /// <summary>
    /// Update countdown each frame. Change Image when the timer has reached 0. 
    /// </summary>
    private void Update()
    {
        lifetimeCountdown -= Time.deltaTime;
        if (lifetimeCountdown <= 0)
        {
            imageIndex++;
            SetTexture();
        }
    }
    /// <summary>
    /// Change to next image
    /// </summary>
    public void NextImage()
    {
        imageIndex++;
        SetTexture();
    }
    /// <summary>
    /// Change to previous image
    /// </summary>
    public void PreviousImage()
    {
        imageIndex--;
        SetTexture();
    }

    /// <summary>
    /// Checks if the array has elements and if the index sent is out of bounds. Sets image to next image,
    /// or if the index is out of bounds - the image is set to the first element in the array.
    /// </summary>
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
            activeImage.texture = ImageList[imageIndex];
            lifetimeCountdown = imageLifetime;
        }
    }
}
