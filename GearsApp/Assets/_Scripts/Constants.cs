using Newtonsoft.Json;

namespace GEARSApp
{
    /// <summary>
    /// Static Class with Constants. Used to have an easy way to change paths in entire project.
    /// </summary>
    public static class Constants
    {
        //--PATHS--
        public static readonly string PhpPath = "https://cgtroll.com/gearsa/GEARS/PHPScripts/";
        public static readonly string FTPLocationPath = "ftp://ftp.bardrg.com/GEARS/Locations/";
        public static readonly string FTPPath = "ftp://ftp.bardrg.com/GEARS/";

        //--JSON--
        //Sets JSON parsing settings to ignore null values (No error thrown when encountering null from JSON file)
        public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        //--SCENE NAMES--
        public static readonly string LocationScene = "LocationNew";
        public static readonly string MainScene = "Main";
        public static readonly string RegistrationAndLoginScene = "RegistrationAndLogin";
        public static readonly string CollectionARScene = "CollectionAR";
        public static readonly string ARScene = "AR";
    }
}