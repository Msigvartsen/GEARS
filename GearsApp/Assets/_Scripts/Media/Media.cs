using UnityEngine;

/// <summary>
/// Serializable class. Retreives data from JSON via Database/PHP.
/// </summary>
[System.Serializable]
public class Media
{
    //Variables that keeps track of data from Database/PHP script (Comes in JSON format)
    public int media_ID;
    public string medianame;
    public string mediatype;

    //Variables set in Unity/C#
    public Texture2D image=null;
}
