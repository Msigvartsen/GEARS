using System.Collections.Generic;
using Newtonsoft.Json;

/// <summary>
/// Serializable Template Class. Retreives data from JSON via Database/PHP.
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class WebResponse<T>
{
    [JsonProperty("handler")]
    public PHPStatusHandler handler;
    [JsonProperty("item")]
    public List<T> objectList;
}

