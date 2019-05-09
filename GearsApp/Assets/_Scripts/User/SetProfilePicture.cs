using UnityEngine;
using UnityEngine.UI;

public class SetProfilePicture : MonoBehaviour
{
    private void Start()
    {
        RefreshImage();
    }

    public void RefreshImage()
    {
        MediaController mediaController = MediaController.GetInstance();
        User user = UserController.GetInstance().CurrentUser;

        foreach (Media media in mediaController.MediaList)
        {
            if (media.media_ID == user.media_ID)
            {
                GetComponent<RawImage>().texture = media.image;
            }
        }
    }
}
