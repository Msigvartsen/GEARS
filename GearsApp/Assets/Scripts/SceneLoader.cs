using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int sceneIndex;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 4)
            {
                LoadSceneByIndex(sceneIndex);
            }
        }
    }

    public void LoadSceneByIndex(int index)
    {
        LoadingScreen.LoadSceneByIndex(index);
    }
}
