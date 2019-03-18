using UnityEngine;
using UnityEngine.UI;

public class SetProfilePicture : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(LoadProfilePicture);
    }

    public void LoadProfilePicture()
    {
        UserController uc = UserController.GetInstance();
        GameObject profilePicture = GameObject.FindGameObjectWithTag("ProfilePicture");
        string name = GetComponent<RawImage>().name;
       // uc._currentUser.media_ID = name;
        Texture2D tex = (Texture2D)GetComponent<RawImage>().texture;
        profilePicture.GetComponent<RawImage>().texture = tex;
        profilePicture.GetComponent<PopupPanel>().SetPopupPanelActive(false);
    }
}
