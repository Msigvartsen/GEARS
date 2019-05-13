using System.Collections.Generic;
using UnityEngine;
using GEARSApp;

/// <summary>
/// Serializable struct. Retreives data from JSON via Database/PHP.
/// </summary>
[System.Serializable]
struct Model_id
{
    public int model_ID;
}

public class ModelController : MonoBehaviour
{
    public List<Model> ModelList { get; set; }
    public List<int> FoundModels { get; set; }
    public List<LocationModel> LocationModels { get; set; }

    public Model SelectedCollectibleModel { get; set; }

    private static ModelController instance;

    public static ModelController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        if (LocationModels == null)
            LocationModels = new List<LocationModel>();

        CallRequestModels();
        CallRequestLocationModels();
    }

    public void CallRequestModels()
    {
        string path = Constants.PhpPath + "models.php";
        StartCoroutine(WebRequestController.GetRequest<WebResponse<Model>>(path, InitModelList));
    }

    public void CallRequestLocationModels()
    {
        string path = Constants.PhpPath + "locationmodels.php";
        StartCoroutine(WebRequestController.GetRequest<WebResponse<LocationModel>>(path, SetLocationModels));
    }

    public void CallUpdateFoundModel(int model_id)
    {
        WWWForm form = new WWWForm();
        form.AddField("number", UserController.GetInstance().CurrentUser.telephonenr);
        form.AddField("model_ID", model_id);
        string path = Constants.PhpPath + "updatefoundmodel.php";

        StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler>(path, form, UpdateFoundModels));
    }

    public void CallGetFoundModel()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", UserController.GetInstance().CurrentUser.telephonenr);

        string path = Constants.PhpPath + "foundmodels.php";

        StartCoroutine(WebRequestController.PostRequest<WebResponse<Model_id>>(path, form, SetFoundModelList));
    }

    private void UpdateFoundModels(PHPStatusHandler handler)
    {
        if (!WebRequestController.CheckValidResponse(handler))
            return;

        CallGetFoundModel();
    }

    private void InitModelList(WebResponse<Model> response)
    {
        if (ModelList == null)
            ModelList = new List<Model>();

        foreach (Model model in response.objectList)
        {
            ModelList.Add(model);
        }
    }

    private void SetLocationModels(WebResponse<LocationModel> response)
    {
        if (response.handler.statusCode == false)
        {
            Debug.Log(response.handler.text);
            return;
        }

        foreach (LocationModel locationModel in response.objectList)
        {
            LocationModels.Add(locationModel);
        }
    }

    private void SetFoundModelList(WebResponse<Model_id> response)
    {
        if (!WebRequestController.CheckValidResponse(response.handler))
            return;

        if (FoundModels == null)
            FoundModels = new List<int>();
        else
            FoundModels.Clear();

        foreach (Model_id foundModel in response.objectList)
        {
            FoundModels.Add(foundModel.model_ID);
        }
    }

    public Texture2D GetModelThumbnail(int modelID)
    {
        foreach(Model model in ModelList)
        {
            if(model.model_ID == modelID)
            {
                return Resources.Load<Texture2D>("_Media/ModelThumbnails/" + model.model_name);
            }
        }
        return null;
    }

}
