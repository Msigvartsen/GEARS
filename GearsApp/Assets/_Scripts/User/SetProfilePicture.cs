using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to update current profile picture to be rendered in Unity UI.
/// </summary>
public class SetProfilePicture : MonoBehaviour
{
    /// <summary>
    /// Ran before the first frame. Refreshed current Image.
    /// </summary>
    private void Start()
    {
        RefreshImage();
    }

    /// <summary>
    /// Updates the displayed image to match with Users current image.
    /// </summary>
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
