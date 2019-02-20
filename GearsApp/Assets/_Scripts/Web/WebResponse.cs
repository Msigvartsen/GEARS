using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class WebResponse
{
    public PHPStatusHandler handler;
    [JsonProperty("location")]
    public List<Location> locations;
}

