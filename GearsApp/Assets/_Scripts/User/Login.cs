using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using ConstantsNS;

public class Login : MonoBehaviour
{
    [Header("Input Fields")]
    public InputField usernameField;
    public InputField passwordField;

    [Header("Buttons")]
    public Button loginButton;

    public void CallLogin()
    {
        StartCoroutine(UserLogin());
    }
    
    IEnumerator UserLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("user", usernameField.text);
        form.AddField("password", passwordField.text);

        //using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/gears/login.php", form))
        string path = Constants.PhpPath + "login.php";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(path, form))
        {
            webRequest.downloadHandler = new DownloadHandlerBuffer();
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
                if(obj.handler.statusCode == true)
                {
                    UserController manager = UserController.GetInstance();
                    manager._currentUser = obj.objectList.ToArray()[0];

                    LocationController.GetInstance().CallGetFavorites();
                    LoadingScreen.LoadScene("MainMenu");
                }
                else
                {
                    Debug.Log("Error: Login Failed -> Wrong Username or Password");
                }
            }
        }
    }
}


