using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserProfile : MonoBehaviour
{
    public Transform loadingBar;
    private User _currentUser;

    [SerializeField]
    private TMPro.TextMeshProUGUI username;
    [SerializeField]
    private TMPro.TextMeshProUGUI level;
    [SerializeField]
    private TMPro.TextMeshProUGUI experience;

    public void UpdateUserProfileUI()
    {
        _currentUser = UserController.GetInstance().CurrentUser;

        if (_currentUser == null)
            return;

        username.text = _currentUser.username;

        int xp = _currentUser.experience;
        float xpFillAmount = (float)xp / 100;
        loadingBar.GetComponent<Image>().fillAmount = xpFillAmount;

        if (experience != null && level != null)
        {
            experience.text = xp.ToString();
            level.text = _currentUser.level.ToString();
        }
    }
}
