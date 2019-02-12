using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/gears/login.php", form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                string req = webRequest.downloadHandler.text;
                Debug.Log(req);
                if(req == "0")
                {
                    Debug.Log("Error: Login Failed -> Wrong Username or Password");
                }
                else
                {
                    Debug.Log("Hooray! Welcome");
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                }
            }
        }
    }
}
