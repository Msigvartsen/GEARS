using UnityEngine;

/// <summary>
/// Serializable class. Retreives data from JSON via Database/PHP.
/// </summary>
[System.Serializable]
public class Location
{
    //Variables that keeps track of data from Database/PHP script (Comes in JSON format)
    public int location_ID;
    public string name;
    public double latitude;
    public double longitude;
    public string information;
    public bool favorite;

    //Variables set in Unity/C#
    public Texture2D thumbnail;
    public Texture2D[] images;
}
