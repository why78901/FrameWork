using System;
using FMODUnity;
using UnityEngine;
using System.IO;

public class AudioManager
{
    private static AudioManager instance;

    public static AudioManager GetInstance()
    {
        if (instance == null)
        {
            instance = new AudioManager();
        }
        return instance;
    }
    
    public void LoadBanks(Action callback)
    {
        // Debug.LogError("persistentDataPath" + Application.persistentDataPath);
        string[] arr = { "Master.bytes", "Master.strings.bytes", "music.bytes", "sfx.bytes", "ui.bytes"};
        // string[] arr = { "Master.bytes", "Master.strings.bytes"};
        int len = arr.Length;
        for (int i = 0; i < len; i++)
        {
            //根据路径拿到文件信息
            FileInfo file = new FileInfo(arr[i]);
            string assetPath = $"Audio/FMODBanks/{file.Name}";
            TextAsset asset = ResourceLoader.Load<TextAsset>(assetPath);
            if (asset == null)
            {
                UnityEngine.Debug.LogError($"Dont Found bank assetPath: {assetPath}");
                return;
            }

            RuntimeManager.LoadBank(asset);
        }
        Debug.LogError("LoadBank Finish");
        callback?.Invoke();
    }
}