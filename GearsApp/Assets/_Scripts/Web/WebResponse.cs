using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class WebResponse<T>
{
    [JsonProperty("handler")]
    public PHPStatusHandler handler;
    [JsonProperty("item")]
    public List<T> objectList;
}

