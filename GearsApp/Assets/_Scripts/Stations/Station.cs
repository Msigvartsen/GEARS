
/// <summary>
/// Serializable class. Retreives data from JSON via Database/PHP.
/// </summary>
[System.Serializable]
public class Station
{
    //Variables that keeps track of data from Database/PHP script (Comes in JSON format)
    public int location_ID;
    public int station_NR;
    public int target_ID;
    public double latitude;
    public double longitude;
    public int model_ID;
    public string helptext;
    public int score;

    //Variables set in Unity/C#
    public bool visited;
}
