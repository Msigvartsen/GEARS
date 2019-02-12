using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject loginPanel;
    [SerializeField]
    private GameObject registerPanel;

    public void Login()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }
    public void Register()
    {
        registerPanel.SetActive(true);
        loginPanel.SetActive(false);
        
    }
}

