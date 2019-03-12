using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int index)
    {
        LoadingScreen.LoadScene(index);
    }
    public void LoadScene(string sceneName)
    {
        if (sceneName == "SelectLocation")
            StartCoroutine(LocationService.StartLocationService());

        LoadingScreen.LoadScene(sceneName);
    }
}
