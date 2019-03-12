﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace ConstantsNS
{
    public static class Constants
    {
        public static readonly string PhpPath = "https://cgtroll.com/gearsa/GEARS/PHPScripts/";
        public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };
    }
}