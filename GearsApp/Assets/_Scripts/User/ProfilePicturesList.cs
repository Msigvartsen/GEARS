using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePicturesList : MonoBehaviour
{
    Texture2D[] images;
    private string prefabName;

    private void Start()
    {
        prefabName = "ImageListItem";
        string path = ConstantsNS.Constants.FTPPath + "/Media/Images/";
        Uri uri = new Uri(path);
        images = FTPHandler.DownloadAllImagesFromFTP(uri).ToArray();

        for(int i = 0; i < images.Length; i++)
        {
            GetListItem(i);
        }
    }

    private GameObject GetListItem(int index)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(gameObject.transform, false);
        go.GetComponent<RawImage>().texture = images[index];
        //go.GetComponent<SetProfilePicture>().imageName = 
        return go;
    }
}
