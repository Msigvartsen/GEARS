using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using ConstantsNS;
using TMPro;

public class Registration : MonoBehaviour
{
    [Header("Input Fields")]
    [SerializeField]
    private TMP_InputField mobileField;
    [SerializeField]
    private TMP_InputField nameField;
    [SerializeField]
    private TMP_InputField passwordField;

    [SerializeField]
    private PopupNotification popupNotification;

    public void CallRegister()
    {
        if (Inputcheck.ValidateInput(mobileField))
        {
            StartCoroutine(Register());
        }
        else
        {
            popupNotification.ShowPopup("Mobile number needs to have 8 digits!");
        }
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("telephonenr", mobileField.text);
        form.AddField("name", nameField.text);
        form.AddField("password", passwordField.text);

        //using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/gears/register.php", form))
        string path = Constants.PhpPath + "register.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(path, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                string req = webRequest.downloadHandler.text;
                WebResponse<User> obj = JsonConvert.DeserializeObject<WebResponse<User>>(req, Constants.JsonSettings);
                Debug.Log(obj.handler.text);
                if (obj.handler.statusCode == true)
                {
                    UserController manager = UserController.GetInstance();
                    manager.CurrentUser = obj.objectList.ToArray()[0];

                    LocationController.GetInstance().CallGetFavorites();
                    LoadingScreen.LoadScene("Main");
                }
                else
                {
                    if (popupNotification != null)
                    {
                        popupNotification.ShowPopup("User Already Exist!");
                    }
                }
            }
        }
    }
}
