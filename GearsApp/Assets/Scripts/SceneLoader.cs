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
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 2/* && touch.phase == TouchPhase.Began*/)
            {
                LoadSceneByIndex(sceneIndex);
            }
        }
    }

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
}
