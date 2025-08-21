using System.Runtime.InteropServices;
using UnityEngine;

// public class LocalStorageManager : MonoGenericSingleton<LocalStorageManager>
// {
//     [DllImport("__Internal")]
//     private static extern void SaveToLocalStorage(string key, string value);

//     [DllImport("__Internal")]
//     private static extern string LoadFromLocalStorage(string key);

//     public void Save(string json)
//     {
//         SaveToLocalStorage("playerData", json);
//         Debug.Log("Saved to localStorage!");
//     }

//     public void Load()
//     {
//         string json = LoadFromLocalStorage("playerData");
//         Debug.Log("Loaded: " + json);
//     }
// }


public class LocalStorageManager : MonoGenericSingleton<LocalStorageManager>
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SaveToLocalStorage(string key, string value);

    [DllImport("__Internal")]
    private static extern string LoadFromLocalStorage(string key);
#else
    private static void SaveToLocalStorage(string key, string value)
    {
        Debug.Log("LocalStorage not available in Editor.");
    }

    private static string LoadFromLocalStorage(string key)
    {
        return null;
    }
#endif

    public void Save(string json)
    {
        SaveToLocalStorage("playerData", json);
        Debug.Log("Saved to localStorage!");
    }

    public void Load()
    {
        string json = LoadFromLocalStorage("playerData");
        Debug.Log("Loaded: " + json);
    }
}