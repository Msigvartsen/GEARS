using UnityEngine;
using ConstantsNS;

public class FavoriteController : MonoBehaviour
{
    [SerializeField]
    private LocationListItem listItem;
    [SerializeField]
    private GameObject imagePanel; //For filled/unfilled favorite icon
    private bool isFavorite;

    private void Start()
    {
        isFavorite = listItem.Location.favorite;
        imagePanel.SetActive(isFavorite);
    }

    public void CallAddToFavorite()
    {
        isFavorite = !isFavorite;
        imagePanel.SetActive(isFavorite);
        UserController manager = UserController.GetInstance();
        Location loc = listItem.Location;

        WWWForm form = new WWWForm();
        form.AddField("number", manager.CurrentUser.telephonenr);
        form.AddField("location_id", loc.location_ID);

        string path;
        if (isFavorite)
            path = Constants.PhpPath + "addfavorite.php";
        else
            path = Constants.PhpPath + "removefavorite.php";

        StartCoroutine(WebRequestController.PostRequest<PHPStatusHandler>(path, form, UpdateFavorite));
    }

    private void UpdateFavorite(PHPStatusHandler handler)
    {
        if (handler.statusCode == false)
        {
            Debug.Log(handler.text);
            return;
        }

        var loc = listItem.Location;
        loc.favorite = isFavorite;
        LocationController.GetInstance().UpdateLocation(loc);
    }
}

