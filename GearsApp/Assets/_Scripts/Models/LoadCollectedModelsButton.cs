using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that loads a specific model when pressed.
/// </summary>
public class LoadCollectedModelsButton : MonoBehaviour
{
    public Model model;
    private Button button;
    private GameObject popupObject;
    private GameObject popupTitle;
    private GameObject popupConfirm;
    private Texture2D modelThumbnail;
    [SerializeField]
    private RawImage thumbnailPanel;

    /// <summary>
    /// Called the first frame.
    /// Sets instances and data values.
    /// </summary>
    void Start()
    {
        modelThumbnail = ModelController.GetInstance().GetModelThumbnail(model.model_ID);
        if (modelThumbnail != null && thumbnailPanel != null)
            thumbnailPanel.texture = modelThumbnail;

        popupObject = GameObject.FindGameObjectWithTag("CollectedModelsSelectPopup");
        popupTitle = GameObject.FindGameObjectWithTag("CollectedModelsSelectTitle");
        popupConfirm = GameObject.FindGameObjectWithTag("CollectedModelsConfirmSelection");
        button = GetComponent<Button>();
        button.onClick.AddListener(ConfirmButton);
    }

    /// <summary>
    /// Select the model and load AR view scene.
    /// </summary>
    void SelectMe()
    {
        ModelController.GetInstance().SelectedCollectibleModel = model;
        LoadingScreen.LoadScene(GEARSApp.Constants.CollectionARScene);
    }

    /// <summary>
    /// Opens a popup panel where the user has to confirm their selection.
    /// </summary>
    void ConfirmButton()
    {
        popupTitle.GetComponent<TMPro.TextMeshProUGUI>().text = model.model_name;

        if (modelThumbnail != null)
            popupTitle.transform.parent.GetComponent<RawImage>().texture = modelThumbnail;

        popupConfirm.GetComponent<Button>().onClick.AddListener(SelectMe);
        popupObject.GetComponent<Animator>().Play("Fade-in");
    }
}
