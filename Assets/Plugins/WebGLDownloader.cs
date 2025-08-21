using UnityEngine;
using System.Runtime.InteropServices;

public static class WebGLDownloader
{
    #if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void DownloadFile(string json, string filename);
    #endif

    public static void SaveJSON(string jsonData, string filename)
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        DownloadFile(jsonData, filename);
        #else
        // For editor/standalone testing
        string path = System.IO.Path.Combine(Application.persistentDataPath, filename);
        System.IO.File.WriteAllText(path, jsonData);
        Debug.Log("Saved locally at: " + path);
        #endif
    }
}
