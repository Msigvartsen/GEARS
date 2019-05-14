using UnityEngine;
using UnityEngine.UI;

public class LoadModelButton : MonoBehaviour
{
    public Model Model { get; set; }

    private Button button;
    private bool loaded = false;
    private GameObject modelToShow;
    private GameObject groundPlane;
    private GameObject shadowPlane;
    private GameObject smokeSpawn;
    [SerializeField]
    private RawImage thumbnail;

    void Start()
    {
        SetButtonThumbnail();
        button = GetComponent<Button>();
        button.onClick.AddListener(CloseProfilePictureWindow);
        button.onClick.AddListener(LoadModel);
        groundPlane = GameObject.FindGameObjectWithTag("GroundPlane");
    }

    /// <summary>
    /// Load model connected to the button.
    /// </summary>
    public void LoadModel()
    {
        if (groundPlane.transform.childCount > 0)
        {
            Debug.Log("DESTROY EXISTING MODEL");
            if (groundPlane.transform.GetChild(0).name != Model.model_name)
            {
                Destroy(groundPlane.transform.GetChild(0).gameObject);
                loaded = false;
            }
        }

        // Create the prefab if it is not loaded
        if (!loaded)
        {
            modelToShow = Instantiate(Resources.Load<GameObject>("_Prefabs/" + Model.model_name), groundPlane.transform);
            shadowPlane = Instantiate(Resources.Load<GameObject>("_Prefabs/" + "ShadowPlane"), modelToShow.transform);
            smokeSpawn = Instantiate(Resources.Load<GameObject>("_Prefabs/" + "SmokeSpawn"), modelToShow.transform);

            shadowPlane.GetComponent<Renderer>().enabled = false;
            smokeSpawn.GetComponent<Renderer>().enabled = false;

            // Get the renderer component in either child or on current object and turn it off
            if (modelToShow.GetComponentsInChildren<Renderer>(true).Length > 0)
            {
                var rendererComponents = modelToShow.GetComponentsInChildren<Renderer>(true);
                foreach (var componenent in rendererComponents)
                {
                    componenent.enabled = false;
                }
            }
            else if (modelToShow.GetComponent<Renderer>())
            {
                modelToShow.GetComponent<Renderer>().enabled = false;
            }

            modelToShow.transform.localPosition = new Vector3(0, 0, 0);
            smokeSpawn.transform.localPosition = new Vector3(0, 0, 0);
            loaded = true;
        }
        else
        {
            // Activate the model if its selected and loaded
            modelToShow.SetActive(true);
            shadowPlane.SetActive(true);
            smokeSpawn.SetActive(true);
        }
    }

    public void CloseProfilePictureWindow()
    {
        GameObject p = GameObject.FindGameObjectWithTag("NewProfilePictureWindow");
        p.GetComponent<Animator>().Play("Fade-out");

    }

    private void SetButtonThumbnail()
    {
        Texture2D img = ModelController.GetInstance().GetModelThumbnail(Model.model_ID);
        if (img != null)
            thumbnail.texture = img;
    }
}

