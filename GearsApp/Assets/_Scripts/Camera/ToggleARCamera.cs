using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleARCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject[] arCameraPanels;

    public void CheckARcamEnabled()
    {
        for(int i = 0; i < arCameraPanels.Length; i++)
        {
            if (arCameraPanels[i].activeSelf)
            {
                gameObject.SetActive(true);
                return;
            }
        }
        gameObject.SetActive(false);
    }

}
