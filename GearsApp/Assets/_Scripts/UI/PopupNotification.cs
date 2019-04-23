using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupNotification : MonoBehaviour
{

    [SerializeField]
    private float delay = 3;

    public void ShowPopup(string text)
    {
        GetComponent<Animator>().Play("Fade-in");
        GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
        Invoke("FadeOutDelay", delay);
    }

    private void FadeOutDelay()
    {
        GetComponent<Animator>().Play("Fade-out");
    }
}
