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
    public TMP_InputField mobileField;
    public TMP_InputField nameField;
    public TMP_InputField passwordField;
    [Header("Buttons")]
    public Button submitButton;
   
    public void CallRegister()
    {
        StartCoroutine(Register());
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
                    //LoadingScreen.LoadScene("MainMenu");
                }
            }
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
