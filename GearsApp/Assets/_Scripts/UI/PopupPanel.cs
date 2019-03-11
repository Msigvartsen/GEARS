using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _popupPanel;

    private void Start()
    {
        _popupPanel.SetActive(false);
    }

    public void SetPopupPanelActive(bool setActive)
    {
        _popupPanel.SetActive(setActive);
    }
}
