using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadCollectedModelsButton : MonoBehaviour
{
    public Model model;
    private Button button;
    private GameObject popupObject;
    private GameObject popupTitle;
    private GameObject popupConfirm;

    // Start is called before the first frame update
    void Start()
    {
        popupObject = GameObject.FindGameObjectWithTag("CollectedModelsSelectPopup");
        popupTitle = GameObject.FindGameObjectWithTag("CollectedModelsSelectTitle");
        popupConfirm = GameObject.FindGameObjectWithTag("CollectedModelsConfirmSelection");
        button = GetComponent<Button>();
        button.onClick.AddListener(ConfirmButton);
    }

    void SelectMe()
    {
        ModelController.GetInstance().SelectedCollectibleModel = model;
        LoadingScreen.LoadScene(ConstantsNS.Constants.CollectionARScene);
    }

    void ConfirmButton()
    {
        popupTitle.GetComponent<TMPro.TextMeshProUGUI>().text = model.model_name;
        popupConfirm.GetComponent<Button>().onClick.AddListener(SelectMe);
        popupObject.GetComponent<Animator>().Play("Fade-in");
    }
}
