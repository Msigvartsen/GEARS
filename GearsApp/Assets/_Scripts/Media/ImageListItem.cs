using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageListItem : MonoBehaviour
{
    public Media media;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(UpdateProfilePicture);
        GetComponent<Button>().onClick.AddListener(CloseProfilePictureWindow);
    }

    public void RefreshImage()
    {
        if (media != null)
        {
            GetComponent<RawImage>().texture = media.image;
        }
    }

    public void UpdateProfilePicture()
    {
        UserController uc = UserController.GetInstance();
        GameObject[] profilePictures = GameObject.FindGameObjectsWithTag("ProfilePicture");
        uc.CallUpdateUserPicture(media.media_ID);

        if (media != null)
        {
            foreach (var pictures in profilePictures)
            {
                pictures.GetComponent<RawImage>().texture = media.image;
                pictures.GetComponent<SetProfilePicture>().RefreshImage();
            }
        }
    }

    public void CloseProfilePictureWindow()
    {
        GameObject p = GameObject.FindGameObjectWithTag("NewProfilePictureWindow");
        p.GetComponent<Animator>().Play("Fade-out");
    }

}
