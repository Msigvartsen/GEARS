using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingImageFading : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup flashingCanvasGroup;
    private CanvasGroup myCanvasGroup;
    private bool fadeOut = false;

    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        myCanvasGroup = GetComponent<CanvasGroup>();
        flashingCanvasGroup = flashingCanvasGroup.GetComponent<CanvasGroup>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (flashingCanvasGroup.alpha <= 0 && Time.time > 0.2f)
            fadeOut = true;

        if (fadeOut)
        {
            myCanvasGroup.alpha -= 2f * Time.deltaTime;

            if (myCanvasGroup.alpha <= 0)
            {
                gameObject.SetActive(false);
            }
        }

    }
}
