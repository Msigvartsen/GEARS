using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

/// <summary>
/// Static class for downloading files from a FTP Server.
/// </summary>
public static class FTPHandler
{
    /// <summary>
    /// Downloads one image from FTP server.
    /// </summary>
    /// <param name="serverUri">Uri path to image on server</param>
    /// <returns>Returns downloaded image as Texture2D</returns>
    public static Texture2D DownloadImageFromFTP(Uri serverUri)
    {
        byte[] data = DownloadDataAsByte(serverUri);
        //Create a new texture with size 2,2 (Size doesnt matter here, it will be overwritten by downloaded image).
        Texture2D file = new Texture2D(2, 2);
        if(data != null)
            file.LoadImage(data);

        return file;
    }

    /// <summary>
    /// Downloads all images in a folder from FTP Server.
    /// </summary>
    /// <param name="serverUri">Uri path to image on server</param>
    /// <returns></returns>
    public static List<Texture2D> DownloadAllImagesFromFTP(Uri serverUri)
    {
        List<Texture2D> images = new List<Texture2D>();

        FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(serverUri);
        listRequest.Method = WebRequestMethods.Ftp.ListDirectory;
        WebClient req = ConnectToFTPClient(serverUri);
        listRequest.Credentials = req.Credentials;
        
        List<string> lines = new List<string>();

        //Set up response and Stream.
        using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
        using (Stream listStream = listResponse.GetResponseStream())
        using (StreamReader listReader = new StreamReader(listStream))
        {
            //Read all lines and insert into string list. If item starts with "." or ".." (paths) they will be skipped.
            while (!listReader.EndOfStream)
            {
                string item = listReader.ReadLine();
                
                if(!string.IsNullOrEmpty(item) && !item.StartsWith("."))
                {
                    lines.Add(item);
                }
            }
        }
        //Loop through all string lines (containing paths to images) and download each image as Texture2D,
        //and add them to a list.
        foreach(string line in lines)
        {
            Uri fileUri = new Uri(serverUri.ToString() + line);
            Texture2D tex = DownloadImageFromFTP(fileUri);
            images.Add(tex);
        }

        return images;
        
    }

    /// <summary>
    /// Download text file from FTP Server
    /// </summary>
    /// <param name="serverUri">Uri path to image on server</param>
    /// <returns>Returns text file as string.</returns>
    public static string DownloadTextFromFTP(Uri serverUri)
    {
        byte[] data = DownloadDataAsByte(serverUri);
        if(data != null)
        {
            string result = System.Text.Encoding.UTF8.GetString(data);
            return result;
        }
        else
            return null;
    }

    /// <summary>
    /// Download data from FTP server as Bytes.
    /// </summary>
    /// <param name="serverUri">Uri path to image on server</param>
    /// <returns>Returns all data in Byte format.</returns>
    private static byte[] DownloadDataAsByte(Uri serverUri)
    {
        WebClient request = ConnectToFTPClient(serverUri);
        if (request != null)
        {
            byte[] newFileData = request.DownloadData(serverUri.ToString());
            return newFileData;
        }
        else
            return null;
    }

    /// <summary>
    /// Connection to the FTP Client server. A text asset (credentials.json) is needed containing correct
    /// log in. (Json format needs to match Credentials class).
    /// </summary>
    /// <param name="serverUri">Uri path to image on server</param>
    /// <returns>Returns request to client with status.</returns>
    private static WebClient ConnectToFTPClient(Uri serverUri)
    {
        if (serverUri.Scheme != Uri.UriSchemeFtp)
        {
            return null;
        }

        WebClient request = new WebClient();
        var json = Resources.Load <TextAsset>("_Text/credentials");
        Credentials creds = JsonUtility.FromJson<Credentials>(json.ToString());
        request.Credentials = new NetworkCredential(creds.servername, creds.password);
        return request;
    }
}

/// <summary>
/// Credential class is used to read credential file, and connect to FTP server.
/// </summary>
class Credentials
{
    public string servername;
    public string password;
}