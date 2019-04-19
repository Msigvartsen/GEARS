using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Location
{
    public int location_ID;
    public string name;
    public double latitude;
    public double longitude;
    public string information;
    public bool favorite;

    public Texture2D thumbnail;
    public Texture2D[] images;
}
