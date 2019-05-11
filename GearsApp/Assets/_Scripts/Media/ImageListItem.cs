using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ImageListItem is a script attached to ImageListItem prefab in Unity.
/// The script updates current profile picture in Unity UI and starts request to database.
/// </summary>
public class ImageListItem : MonoBehaviour
{
    public Media Media { get; set; }

    /// <summary>
    /// Add Listeners to Buttons at Start.
    /// </summary>
    private void Start()
    {
        AddButtonListeners();
    }

    /// <summary>
    /// Sets up OnClick listeners to Image button.
    /// </summary>
    private void AddButtonListeners()
    {
        GetComponent<Button>().onClick.AddListener(UpdateProfilePicture);
        GetComponent<Button>().onClick.AddListener(CloseProfilePictureWindow);
    }

    /// <summary>
    /// Refreshes/Updates Image with new texture from Current Media.
    /// </summary>
    public void RefreshImage()
    {
        if (Media != null)
            GetComponent<RawImage>().texture = Media.image;
    }

    /// <summary>
    /// Finds all gameobjects using profile picture and updates them to the new, selected profile picture.
    /// Sends a request to database to update users profile picture.
    /// </summary>
    public void UpdateProfilePicture()
    {
        UserController uc = UserController.GetInstance();
        GameObject[] profilePictures = GameObject.FindGameObjectsWithTag("ProfilePicture");
        uc.CallUpdateUserPicture(Media.media_ID);

        if (Media != null)
        {
            foreach (var pictures in profilePictures)
            {
                pictures.GetComponent<RawImage>().texture = Media.image;
                pictures.GetComponent<SetProfilePicture>().RefreshImage();
            }
        }
    }

    /// <summary>
    /// Closes profile picture popup window after selecting new picture.
    /// </summary>
    public void CloseProfilePictureWindow()
    {
        GameObject p = GameObject.FindGameObjectWithTag("NewProfilePictureWindow");
        p.GetComponent<Animator>().Play("Fade-out");
    }
}
