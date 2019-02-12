using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public int sceneIndex;

    public void LoadSceneByIndex(int index)
    {
        LoadingScreen.LoadSceneByIndex(index);
    }
}
