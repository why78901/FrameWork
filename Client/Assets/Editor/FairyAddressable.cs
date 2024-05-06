using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.Windows;

public class FairyAddressable : Editor
{
    #if ADDRESSABLES_ENABLED
    
    [MenuItem("AssetAddress/删除无效组")]
    private static void RemoveInvalidGroup()
    {
        Debug.Log("====================清理空group======================");
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetGroup defaultLocalGroup = settings.FindGroup("Default Local Group");

        if (defaultLocalGroup == null)
        {
            Debug.LogError("没有找到默认的本地配置组");
            return;
        }
        settings.DefaultGroup = defaultLocalGroup;
        
        List<AddressableAssetGroup> groups = settings.groups;
        List<AddressableAssetGroup> deleteGroups = new List<AddressableAssetGroup>();
        for (int i = 0; i < groups.Count; i++)
        {
            AddressableAssetGroup group = groups[i];
            if (group == null)
            {
                //Debug.Log("删除空引用:");
                deleteGroups.Add(group);
                continue;
            }

            if (group.Schemas.Count < 1)
            {
                deleteGroups.Add(group);
                continue;
            }
            
            if(group.Name == "Built In Data" || group.Name == "Default Local Group")
                continue;

            if (group.entries.Count < 1)
            {
                //Debug.Log("删除空Group:" + group.Name);
                deleteGroups.Add(group);
            }
            else
            {
                foreach (var entry in group.entries)
                {
                    if (!File.Exists(entry.AssetPath))
                    {
                        //Debug.LogError("清理无效的路径===" + group.Name+" ----------  "+ entry.AssetPath);
                        deleteGroups.Add(group);
                    }
                }
            }
        }

        for (int i = 0; i < deleteGroups.Count; i++)
        {
            AddressableAssetGroup group = deleteGroups[i];
            Debug.LogError("删除Group===" + group.Name);
            settings.RemoveGroup(deleteGroups[i]);
        }
        
        Debug.LogError("====================清理完毕======================");
        
        settings.DefaultGroup = defaultLocalGroup;
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
    }
    
    [MenuItem("AssetAddress/设置资源组")]
    public static void AssetAddressSetAddressable()
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetGroup defaultLocalGroup = settings.FindGroup("Default Local Group");

        if (defaultLocalGroup == null)
        {
            Debug.LogError("没有找到默认的本地配置组");
            return;
        }
        settings.DefaultGroup = defaultLocalGroup;
        
        string path = "Assets/AddressResources";
        bool valid = AssetDatabase.IsValidFolder(path);
        if (valid)
        {
            string[] files = AssetDatabase.FindAssets("", new string[] { path });
            for (int i = 0; i < files.Length; i++)
            {
                string guid = files[i];
                string filePath = AssetDatabase.GUIDToAssetPath(guid);
                
                // if(filePath.Contains("NGUI"))
                //     continue;

                if (!string.IsNullOrEmpty(filePath))
                {
                    string groupName = filePath.Replace("/", "-");
                    AddressableAssetGroup group = settings.FindGroup(groupName);

                    System.Type type = AssetDatabase.GetMainAssetTypeAtPath(filePath);
                    if (type == typeof(DefaultAsset))
                    {
                        Debug.Log("跳过文件夹：" + filePath);
                        continue;
                    }

                    if(group != null)
                        continue;
                    
                    
                    Debug.Log("生成组----》" + groupName);
                    group = settings.CreateGroup(groupName, true, false, false, null, typeof(GameObject));
                    AddressableAssetEntry entry =
                        settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(filePath), group);
                    entry.address = filePath;
                }
            }
        }
        
        settings.DefaultGroup = defaultLocalGroup;
        AssetDatabase.SaveAssets();
        EditorUtility.ClearProgressBar();
        Debug.Log("====================共计 group {0}个======================".Replace("{0}",settings.groups.Count+""));
    }

    [MenuItem("AssetAddress/初始编译")]
    public static void AssetAddressBuildNew()
    {
        bool isBuild = EditorUtility.DisplayDialog("Warning", "该功能为初始编译，如热更新资源 请使用热更新编译菜单！ 谨慎使用 后果自负！！！", "编译", "取消");
        if (!isBuild)
            return;

        AddressablesPlayerBuildResult buildResult;
        AddressableAssetSettings.BuildPlayerContent(out buildResult);
        
        if (buildResult != null)
        {
            if(string.IsNullOrEmpty(buildResult.Error))
            {
                Debug.Log("构建成功.");
                EditorWindow.focusedWindow.ShowNotification(new GUIContent("构建成功"));
            }
            else
            {
                Debug.LogError("构建失败 \n" + buildResult.Error);
                EditorWindow.focusedWindow.ShowNotification(new GUIContent("构建失败 \n" + buildResult.Error));
            }
        }
        else
        {
            Debug.LogError("构建失败");
            EditorWindow.focusedWindow.ShowNotification(new GUIContent("构建失败"));
        }
    }

    [MenuItem("AssetAddress/热更新编译")]
    public static bool AssetAddressBuildUpdate()
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        string assetPath = ContentUpdateScript.GetContentStateDataPath(true);
        if (!string.IsNullOrEmpty(assetPath))
        {
            EditorUtility.DisplayProgressBar("打包更新资源.", "该过程生成最新的补丁资源.", 0);
            ContentUpdateScript.BuildContentUpdate(settings, assetPath);
            EditorUtility.ClearProgressBar();
        }
        else
        {
            Debug.LogError("需要更新AA资源方可打包");
            return false;
        }
        
        Debug.Log("热更新编译完成");
        return true;
    }
    #endif
    
    
}