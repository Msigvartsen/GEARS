using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

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

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/gears/register.php", form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("User Created Successfully");
                Debug.Log(webRequest.downloadHandler.text);
                LoadingScreen.LoadScene(1);
            }
        }
    }

    public void VerifyInputs()
    {
        submitButton.interactable = (nameField.text.Length >= 8 && passwordField.text.Length >= 8);
    }
}
