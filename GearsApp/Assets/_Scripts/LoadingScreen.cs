using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance = null;

    [Header("Resources")]
    public CanvasGroup canvasAlpha;
    public Text status;
    public static string prefabName = "ScreenLoader";


    [Header("Images")]
    public Image loadingImageObject;
    [Range(0.1f, 3.0f)] public float imageRadialSpeed = 1f;

    [Header("Settings")]
    public string locationNameText;
    [Range(0.1f, 4.0f)] public float fadingAnimationSpeed = 2f;

    private AsyncOperation loadingProcess;


    // Start is called before the first frame update
    void Start()
    {
        loadingImageObject.type = Image.Type.Filled;
    }

    public static void LoadScene(int index)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName)).GetComponent<LoadingScreen>();
            DontDestroyOnLoad(instance.gameObject);
        }

        instance.gameObject.SetActive(true);
        instance.loadingProcess = SceneManager.LoadSceneAsync(index);
        instance.loadingProcess.allowSceneActivation = true;
    }

    public static void LoadScene(string sceneName)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName)).GetComponent<LoadingScreen>();
            DontDestroyOnLoad(instance.gameObject);
        }

        instance.gameObject.SetActive(true);
        instance.loadingProcess = SceneManager.LoadSceneAsync(sceneName);
        instance.loadingProcess.allowSceneActivation = true;
    }



    void Awake()
    {
        canvasAlpha.alpha = 0f;
        loadingImageObject.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            status.text = Mathf.Round(loadingProcess.progress * 100f).ToString() + "%";
        }
        catch
        {
            Debug.Log("Cannot update status.text.");
        }


        if (loadingProcess.isDone)
        {
            canvasAlpha.alpha -= fadingAnimationSpeed * Time.deltaTime;

            if (canvasAlpha.alpha <= 0)
                gameObject.SetActive(false);

        }
        else
        {
            canvasAlpha.alpha = 1f;
            loadingImageObject.fillAmount += imageRadialSpeed * Time.deltaTime; ;

            if (loadingImageObject.fillAmount >= 1)
                loadingProcess.allowSceneActivation = true;
        }
    }
}
