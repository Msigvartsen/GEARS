using UnityEngine;

/// <summary>
/// Serializable class. Retreives data from JSON via Database/PHP.
/// </summary>
[System.Serializable]
public class Trophy
{
    //Variables that keeps track of data from Database/PHP script (Comes in JSON format)
    public string trophyname;
    public string trophytype;
    public string details;

    //Variables set in Unity/C#
    public Texture2D image;
}
