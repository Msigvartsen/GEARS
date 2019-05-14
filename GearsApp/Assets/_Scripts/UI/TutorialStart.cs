using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStart : MonoBehaviour
{
    [SerializeField]
    UserProfile[] level;

    // Start is called before the first frame update
    void Start()
    {
        if (UserController.GetInstance().CurrentUser.level == 0)
        {
            GetComponent<Animator>().Play("Fade-in");
        }
    }

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
