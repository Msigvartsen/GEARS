using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking; 

[System.Serializable]
public class Location
{
    public int id;
    public string name;
    public double latitude;
    public double longitude;
    public string information;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Location CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<Location>(jsonString);
    }
}
