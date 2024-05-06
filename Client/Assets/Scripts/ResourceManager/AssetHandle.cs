using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// 资源异步加载结果处理器
/// </summary>
/// <typeparam name="T"></typeparam>
public class AssetHandle<T> : AssetHandleBase
{
    public object Key;
    public T Result;
    public IList<object> Keys;
    public IList<T> Results;
    public AsyncOperationHandle AsyncHandle;
    public AsyncOperationHandle<IList<T>> AsyncHandles;
    private AssetOwner m_owner;
    private bool m_list = false;
    private void Create(int id,  AssetOwner owner)
    {
        Id = id;                             
        m_owner = owner;
        m_owner.AddAssetHandle(this);            
    }
    public AssetHandle(int id,object key, AssetOwner owner, AsyncOperationHandle<T> aoh)
    {
        Create(id, owner);         
        Key = key;
        AsyncHandle = aoh;
        Result = aoh.Result;        
        m_list = false;
    }

    public AssetHandle(int id, object key, AssetOwner owner, AsyncOperationHandle<IList<T>> aoh)
    {
        Create(id, owner);
        Key = key;
        AsyncHandles = aoh;
        Results = aoh.Result;          
        m_list = true;
    }      

    /// <summary>
    /// 释放资源
    /// </summary>
    public override void Release()
    {
        if(!m_list)
            Addressables.Release(AsyncHandle);            
        else
            Addressables.Release(AsyncHandles);
        m_owner.RemovAssetHandle(this);
    }
    
}
