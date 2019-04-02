using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTextComponent : MonoBehaviour
{
    public enum ArrayType { Username };
    public ArrayType arrayType;

    void Start()
    {
        if(arrayType == ArrayType.Username)
        {
            GetComponent<Text>().text = UserController.GetInstance().CurrentUser.username;
        }
    }
}
