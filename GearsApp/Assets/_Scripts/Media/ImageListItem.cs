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
    }

    public void RefreshImage()
    {
        if(media != null)
        {
            GetComponent<RawImage>().texture = media.image;
        }
    }

    public void UpdateProfilePicture()
    {
        UserController uc = UserController.GetInstance();
        GameObject profilePicture = GameObject.FindGameObjectWithTag("ProfilePicture");
        uc.CallUpdateUserPicture(media.media_ID);

        if(media != null)
        {
            profilePicture.GetComponent<SetProfilePicture>().RefreshImage();
            //profilePicture.GetComponent<RawImage>().texture = media.image;
        }
        profilePicture.GetComponent<PopupPanel>().SetPopupPanelActive(false);
        
    }

}
