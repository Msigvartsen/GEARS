using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;

public static class FTPHandler
{
    public static Texture2D DownloadImageFromFTP(Uri serverUri)
    {
        byte[] data = DownloadDataAsByte(serverUri);

        Texture2D file = new Texture2D(2, 2);
        if(data != null)
            file.LoadImage(data);

        return file;
    }

    public static List<Texture2D> DownloadAllImagesFromFTP(Uri serverUri)
    {
        List<Texture2D> images = new List<Texture2D>();

        FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(serverUri);
        listRequest.Method = WebRequestMethods.Ftp.ListDirectory;
        WebClient req = ConnectToFTPClient(serverUri);
        listRequest.Credentials = req.Credentials;
        
        List<string> lines = new List<string>();

        using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
        using (Stream listStream = listResponse.GetResponseStream())
        using (StreamReader listReader = new StreamReader(listStream))
        {
            while (!listReader.EndOfStream)
            {
                string item = listReader.ReadLine();
                
                if(!string.IsNullOrEmpty(item) && !item.StartsWith("."))
                {
                    lines.Add(item);
                }
            }
        }

        foreach(string line in lines)
        {
            Uri fileUri = new Uri(serverUri.ToString() + line);
            Debug.Log("FTP FILE URLS : " + fileUri.ToString());
            Texture2D tex = DownloadImageFromFTP(fileUri);
            images.Add(tex);
        }

        return images;
        
    }

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

class Credentials
{
    public string servername;
    public string password;
}