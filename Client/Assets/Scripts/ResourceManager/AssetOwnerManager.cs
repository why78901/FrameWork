using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源持有者管理器，提供统一的资源控制管理功能
/// </summary>
public class AssetOwnerManager
{        
    private static readonly List<AssetOwner> m_aos = new List<AssetOwner>();        

    public static AssetOwner Create()
    {
        AssetOwner ao = new AssetOwner();
        return ao;
    }

    public static void Add(AssetOwner ao)
    {
        m_aos.Add(ao);
    }

    public static void Remove(AssetOwner ao)
    {
        for(int i = m_aos.Count - 1; i >= 0; i--)
        {
            AssetOwner indexAo = m_aos[i];
            if(indexAo == ao)
            {
                indexAo.ReleaseAll();
                m_aos.RemoveAt(i);
                break;
            }    
        }
    }
        
    public static void Clear()
    {
        for(int i = 0; i < m_aos.Count; i++)
        {
            AssetOwner indexAo = m_aos[i];
            indexAo.ReleaseAll();
        }
        m_aos.Clear();
    }
}
