using UnityEngine;
using ConstantsNS;

/// <summary>
/// Class responsible for adding and removing favorites by sending requests to database.
/// Script is used on LocationListItem prefab, on the Images child. 
/// </summary>
public class FavoriteController : MonoBehaviour
{
    [SerializeField]
    private LocationListItem listItem;
    [SerializeField]
    private GameObject imagePanel; //For filled/unfilled favorite icon
    private bool isFavorite;

    /// <summary>
    /// Run at creation.
    /// </summary>
    private void Start()
    {
        isFavorite = listItem.Location.favorite;
        imagePanel.SetActive(isFavorite);
    }

    /// <summary>
    /// Toggles the favorite location and sends a request to update the database.
    /// </summary>
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

    /// <summary>
    /// Action method. This is ran during the Coroutine post using WebRequestController. 
    /// </summary>
    /// <param name="handler">Keeps track of the status from the web request.</param>
    private void UpdateFavorite(PHPStatusHandler handler)
    {
        if (!WebRequestController.CheckValidResponse(handler))
            return;

        var loc = listItem.Location;
        loc.favorite = isFavorite;
        LocationController.GetInstance().UpdateLocation(loc);
    }
}

