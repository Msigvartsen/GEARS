using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script updates UI elements in Unity with correct information and images for the current user.
/// </summary>
public class UserProfile : MonoBehaviour
{
    private User currentUser;

    [SerializeField]
    private Transform loadingBar;
    [SerializeField]
    private TMPro.TextMeshProUGUI username;
    [SerializeField]
    private TMPro.TextMeshProUGUI level;
    [SerializeField]
    private TMPro.TextMeshProUGUI experience;

    /// <summary>
    /// Ran before the first frame. Initialized user profile UI.
    /// </summary>
    private void Start()
    {
        UpdateUserProfileUI();
    }

    /// <summary>
    /// Gets current user and updates UI in Unity with correct images/text.
    /// </summary>
    public void UpdateUserProfileUI()
    {
        currentUser = UserController.GetInstance().CurrentUser;

        if (currentUser == null)
            return;

        UpdateExperienceBar();
        UpdateTextFields();
        SetProfilePicture();
    }

    /// <summary>
    /// Updates radial XP-Bar in Unity.
    /// </summary>
    private void UpdateExperienceBar()
    {
        int xp = currentUser.experience;
        float xpFillAmount = (float)xp / 100;
        loadingBar.GetComponent<Image>().fillAmount = xpFillAmount;
    }

    /// <summary>
    /// Updates all text fields (Level, experience and username)
    /// </summary>
    private void UpdateTextFields()
    {
        if (username != null)
            username.text = currentUser.username;

        if (experience != null)
        {
            experience.text = currentUser.experience.ToString();
        }
        if (level != null)
        {
            level.text = currentUser.level.ToString();
        }
    }

    /// <summary>
    /// Find all profile picture UIs.
    /// Update user picture when Current users media_id is equal to image in media list.
    /// </summary>
    private void SetProfilePicture()
    {
        GameObject profilePicture = GameObject.FindGameObjectWithTag("ProfilePicture");

        MediaController mediaController = MediaController.GetInstance();
        if (mediaController != null && profilePicture != null)
        {
            foreach (Media media in mediaController.MediaList)
            {
                if (media.media_ID == currentUser.media_ID)
                {
                    profilePicture.GetComponent<RawImage>().texture = media.image;
                }
            }
        }
        else
        {
            Debug.Log("MediaController or profilePicture == null in UserProfile.cs - SetProfilePicture");
        }
    }
}
