using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePicturesList : MonoBehaviour
{
    Media[] images;
    private string prefabName;

    private void Start()
    {
        prefabName = "ImageListItem";
        MediaController mediaController = MediaController.GetInstance();
        images = mediaController.MediaList.ToArray();
        for(int i = 0; i < images.Length; i++)
        {
            GetListItem(i, images[i]);
        }
    }

    private GameObject GetListItem(int index, Media media)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(gameObject.transform, false);
        go.GetComponent<ImageListItem>().Media = media;
        go.GetComponent<ImageListItem>().RefreshImage();
        //go.GetComponent<RawImage>().texture = images[index].image;
        //go.GetComponent<Media>() = media;
        return go;
    }
}
