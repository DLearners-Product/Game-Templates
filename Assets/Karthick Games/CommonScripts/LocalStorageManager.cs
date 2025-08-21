using System.Runtime.InteropServices;
using UnityEngine;

public class LocalStorageManager : MonoGenericSingleton<LocalStorageManager>
{
    [DllImport("__Internal")]
    private static extern void SaveToLocalStorage(string key, string value);

    [DllImport("__Internal")]
    private static extern string LoadFromLocalStorage(string key);

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