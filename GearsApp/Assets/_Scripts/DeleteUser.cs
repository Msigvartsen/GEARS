using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteUser : MonoBehaviour
{
    public void CallDeleteUser()
    {
        UserManager.GetInstance().CallDeleteUser();
        GameObject gameObject = GameObject.FindGameObjectWithTag("PopupWarning");
        gameObject.SetActive(false);
    }
    public void CallLogOut()
    {
        UserManager.GetInstance().LogOut();
    }
}
