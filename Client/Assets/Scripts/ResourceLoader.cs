using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourceLoader : MonoBehaviour
{
    public static string addressResRoot = "Assets/AddressResources/";
    
    public static T Load<T>(string path) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        string url = addressResRoot + path;
        return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(url);
#endif
        string key = addressResRoot + path;
        key = key.Replace("\\", "/");
        Debug.LogError("key=" + key);
        var op = Addressables.LoadAssetAsync<T>(key);
        T go = op.WaitForCompletion();
        return go;
    }
}
