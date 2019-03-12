using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

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
        request.Credentials = new NetworkCredential("bardrg.com_gearsa", "zg5M2o8S8bDkE9iI");

        return request;
    }
}
