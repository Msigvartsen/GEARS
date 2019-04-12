using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserProfile : MonoBehaviour
{
    private User _currentUser;

    [SerializeField]
    private Transform loadingBar;
    [SerializeField]
    private TMPro.TextMeshProUGUI username;
    [SerializeField]
    private TMPro.TextMeshProUGUI level;
    [SerializeField]
    private TMPro.TextMeshProUGUI experience;


    private void Start()
    {
        UpdateUserProfileUI();
    }

    public void UpdateUserProfileUI()
    {
        _currentUser = UserController.GetInstance().CurrentUser;

        if (_currentUser == null)
            return;

        UpdateExperienceBar();
        UpdateTextFields();
        SetProfilePicture();
    }

    private void UpdateExperienceBar()
    {
        int xp = _currentUser.experience;
        float xpFillAmount = (float)xp / 100;
        loadingBar.GetComponent<Image>().fillAmount = xpFillAmount;
    }

    private void UpdateTextFields()
    {
        if (username != null)
            username.text = _currentUser.username;

        if (experience != null)
        {
            experience.text = _currentUser.experience.ToString();
        }
        if (level != null)
        {
            level.text = _currentUser.level.ToString();
        }
    }

    private void SetProfilePicture()
    {
        GameObject profilePicture = GameObject.FindGameObjectWithTag("ProfilePicture");

        MediaController mediaController = MediaController.GetInstance();
        if (mediaController != null && profilePicture != null)
        {
            foreach (Media media in mediaController.mediaList)
            {
                if (media.media_ID == _currentUser.media_ID)
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
