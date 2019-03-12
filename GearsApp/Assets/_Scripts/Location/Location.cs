﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//REname to LocationModel later
[System.Serializable]
public class Location
{

    public int location_ID;
    public string name;
    public double latitude;
    public double longitude;
    public string information;
    public bool favorite;
}