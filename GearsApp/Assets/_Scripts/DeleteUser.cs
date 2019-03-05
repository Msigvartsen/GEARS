using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteUser : MonoBehaviour
{
    public void CallDeleteUser()
    {
        UserManager.GetInstance().CallDeleteUser();
    }
}
