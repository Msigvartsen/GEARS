using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using ConstantsNS;

public class Registration : MonoBehaviour
{
    [Header("Input Fields")]
    public InputField mobileField;
    public InputField nameField;
    public InputField passwordField;
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
                Debug.Log("REQUEST FROM REG = " + req);
                WebResponse<UserModel> obj = JsonConvert.DeserializeObject<WebResponse<UserModel>>(req, Constants.JsonSettings);
                Debug.Log(obj.handler.text);
                if (obj.handler.statusCode == true)
                {
                    UserManager manager = UserManager.GetInstance();
                    manager._currentUser = obj.objectList.ToArray()[0];
                    Debug.Log(manager._currentUser.telephonenr);
                    LoadingScreen.LoadScene("MainMenu");
                }
            }
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
