using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteUser : MonoBehaviour
{
    public void CallDeleteUser()
    {
        UserController.GetInstance().CallDeleteUser();
    }
    public void CallLogOut()
    {
        UserController.GetInstance().LogOut();
    }
}
