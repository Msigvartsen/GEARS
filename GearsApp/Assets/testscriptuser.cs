using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testscriptuser : MonoBehaviour
{
    void Start()
    {
        UserManager manager = UserManager.GetInstance();
        UserModel user = manager._currentUser;
        Debug.Log("USERNAME: " + user.username);
        GetComponent<Text>().text = user.username;
    }
}
