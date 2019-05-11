
/// <summary>
/// Serializable class. Retreives data from JSON via Database/PHP.
/// </summary>
[System.Serializable]
public class Model
{
    //Variables that keeps track of data from Database/PHP script (Comes in JSON format)
    public int model_ID;
    public string model_name;
    public string modeltype;
    public int score;
}
