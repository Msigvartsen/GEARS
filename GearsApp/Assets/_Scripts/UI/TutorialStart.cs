using UnityEngine;

/// <summary>
/// Script to open tutorial page after user has created a new user
/// </summary>
public class TutorialStart : MonoBehaviour
{
    [SerializeField]
    private UserProfile[] level;

    /// <summary>
    /// Is called before the first frame update.
    /// Fade in Tutorial Page if user is level 0.
    /// </summary>
    void Start()
    {
        if (UserController.GetInstance().CurrentUser.level == 0)
        {
            GetComponent<Animator>().Play("Fade-in");
        }
    }

    /// <summary>
    /// Level up the user after completing the tutorial page.
    /// </summary>
    public void LevelUp()
    {
        UserController.GetInstance().UpdateUserLevel();
        UserController.GetInstance().CallUpdateUserExpAndLevel();

        foreach (var item in level)
        {
            item.UpdateUserProfileUI();
        }
    }
}
