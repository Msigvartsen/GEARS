using UnityEngine;

/// <summary>
/// Script to create Prefab items in Unity with different profile pictures.
/// Sets up buttons to change the picture.
/// </summary>
public class ProfilePicturesList : MonoBehaviour
{
    Media[] images;
    private string prefabName;

    /// <summary>
    /// Ran before the first frame.
    /// Finds all images from medialist and creates a prefab for each one.
    /// </summary>
    private void Start()
    {
        prefabName = "ImageListItem";
        MediaController mediaController = MediaController.GetInstance();
        images = mediaController.MediaList.ToArray();
        for(int i = 0; i < images.Length; i++)
        {
            GetListItem(images[i]);
        }
    }

    /// <summary>
    /// Creates a prefab gameobject with all Media info.
    /// </summary>
    /// <param name="media">Media to be set.</param>
    /// <returns>Returns prefab gameobject</returns>
    private GameObject GetListItem(Media media)
    {
        GameObject go = Instantiate(Resources.Load<GameObject>("_Prefabs/" + prefabName));
        go.transform.SetParent(gameObject.transform, false);
        go.GetComponent<ImageListItem>().Media = media;
        go.GetComponent<ImageListItem>().RefreshImage();

        return go;
    }
}
