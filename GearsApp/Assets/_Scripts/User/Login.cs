using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using ConstantsNS;
using TMPro;
public class Login : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;


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
                    manager.CurrentUser = obj.objectList.ToArray()[0];

                    LocationController.GetInstance().CallGetFavorites();
                    StationController.GetInstance().CallUserProgressRequest();
                    LoadingScreen.LoadScene("Main");
                    //userProfile.UpdateUserProfileUI();
                    //UIController.GetInstance().PanelAnim("Main");
                }
                else
                {
                    Debug.Log("Error: Login Failed -> Wrong Username or Password");
                }
            }
        }
    }
}


