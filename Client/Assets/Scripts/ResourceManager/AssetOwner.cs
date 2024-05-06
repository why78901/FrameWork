using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
public class AssetOwner
    {
        private int m_assetHandleId = 0;
        private readonly List<AssetHandleBase> m_assetHandle = new List<AssetHandleBase>();       

        public AssetOwner()
        {
            AssetOwnerManager.Add(this);
        }

        public void AddAssetHandle(AssetHandleBase ahb)
        {
            m_assetHandle.Add(ahb);
        }
        public void RemovAssetHandle(AssetHandleBase ahb)
        {
            m_assetHandle.Remove(ahb);
        }


        public void Remove()
        {
            AssetOwnerManager.Remove(this);
        }
        #region 加载资源
        
        /// <summary>
        /// 同步加载资源，和Release方法配对使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public AssetHandle<T> LoadAsset<T>(object key)
        {
            //Debug.Log("AssetOwner.LoadAsset " + key);
            AsyncOperationHandle<T> aoh = Addressables.LoadAssetAsync<T>(key);
            aoh.WaitForCompletion();
            m_assetHandleId++;
            AssetHandle<T> ah = new AssetHandle<T>(m_assetHandleId, key, this, aoh);                   
            return ah;
        }

        /// <summary>
        /// 同步加载资源，和Release方法配对使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="completed"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public AssetHandle<T> LoadAssets<T>(object key, Action<T> completed)
        {
            AsyncOperationHandle<IList<T>> aoh = Addressables.LoadAssetsAsync(key, completed);          
            aoh.WaitForCompletion();
            m_assetHandleId++;
            AssetHandle<T> ah = new AssetHandle<T>(m_assetHandleId, key, this, aoh);
            return ah;

        }

        /// <summary>
        /// 异步加载资源，和Release方法配对使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public AssetHandle<T> LoadAssetAsync<T>(object key, Action<AssetHandle<T>> completed)
        {
            //Debug.Log("AssetOwner.LoadAssetAsync " + key);            
            AsyncOperationHandle<T> aoh = Addressables.LoadAssetAsync<T>(key);
            m_assetHandleId++;
            AssetHandle<T> ah = new AssetHandle<T>(m_assetHandleId, key, this, aoh);
            aoh.Completed += e =>
            {
                //Debug.Log("AssetOwner.LoadAsetAsync2 " + aoh.Status);     
                ah.Result = aoh.Result;
                completed?.Invoke(ah);
            };
            return ah;
        }

        /// <summary>
        /// 异步加载资源，和Release方法配对使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public AssetHandle<T> LoadAssetsAsync<T>(object key, Action<T> completed, Action<AssetHandle<T>> allCompleted)
        {
            //Debug.Log("AssetOwner.LoadAssetAsync " + key);
            AsyncOperationHandle<IList<T>> aoh = Addressables.LoadAssetsAsync<T>(key, completed);
            m_assetHandleId++;
            AssetHandle<T> ah = new AssetHandle<T>(m_assetHandleId, key, this, aoh);
            aoh.Completed += e =>
            {
                //Debug.Log("AssetOwner.LoadAsetAsync2 " + aoh.Status);     
                ah.Results = aoh.Result;
                allCompleted?.Invoke(ah);
            };
            return ah;
        }
        #endregion

        #region 实例化资源
        private AssetHandle<GameObject> InstantiateAsync(object key, AsyncOperationHandle<GameObject> aoh, Action<AssetHandle<GameObject>> completed)
        {
            m_assetHandleId++;
            AssetHandle<GameObject> ah = new AssetHandle<GameObject>(m_assetHandleId, key, this, aoh);
            aoh.Completed += e =>
            {
                //Debug.Log("AssetOwner.InstantiateAsync2 " + aoh.Status);   
                ah.Result = aoh.Result;
                completed?.Invoke(ah);
            };
            return ah;
        }

        private AssetHandle<GameObject> Instantiate(object key, AsyncOperationHandle<GameObject> aoh)
        {
            m_assetHandleId++;
            AssetHandle<GameObject> ah = new AssetHandle<GameObject>(m_assetHandleId, key, this, aoh);          
            return ah;
        }

        /// <summary>
        /// 异步实例化资源，和Release方法配对使用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public AssetHandle<GameObject> InstantiateAsync(object key, Action<AssetHandle<GameObject>> completed)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key);        
            return InstantiateAsync(key, aoh,completed);
        }
        /// <summary>
        /// 异步实例化资源，和Release方法配对使用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public AssetHandle<GameObject> InstantiateAsync(object key, Vector3 position, Quaternion rotation, Action<AssetHandle<GameObject>> completed)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, position, rotation);
            return InstantiateAsync(key,aoh, completed);
        }
        /// <summary>
        /// 异步实例化资源，和Release方法配对使用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public AssetHandle<GameObject> InstantiateAsync(object key, Transform parent, Action<AssetHandle<GameObject>> completed)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, parent);
            return InstantiateAsync(key, aoh, completed);
        }
        /// <summary>
        /// 异步实例化资源，和Release方法配对使用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public AssetHandle<GameObject> InstantiateAsync(object key, Transform parent, Vector3 position, Quaternion rotation, Action<AssetHandle<GameObject>> completed)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, position, rotation, parent);
            return InstantiateAsync(key, aoh, completed);
        }      
        /// <summary>
        /// 同步实例化资源，和Release方法配对使用
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AssetHandle<GameObject> Instantiate(object key)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key);
            aoh.WaitForCompletion();
            return Instantiate(key, aoh);
        }
        /// <summary>
        /// 同步实例化资源，和Release方法配对使用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public AssetHandle<GameObject> Instantiate(object key, Vector3 position, Quaternion rotation)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, position, rotation);
            aoh.WaitForCompletion();
            return Instantiate(key, aoh);
        }
        /// <summary>
        /// 同步实例化资源，和Release方法配对使用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public AssetHandle<GameObject> Instantiate(object key, Transform parent)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, parent);
            aoh.WaitForCompletion();
            return Instantiate(key, aoh);
        }        
        /// <summary>
        /// 同步实例化资源，和Release方法配对使用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="completed"></param>
        /// <returns></returns>
        public AssetHandle<GameObject> Instantiate(object key, Transform parent, Vector3 position, Quaternion rotation)
        {
            AsyncOperationHandle<GameObject> aoh = Addressables.InstantiateAsync(key, position, rotation, parent);
            aoh.WaitForCompletion();
            return Instantiate(key, aoh);
        }

        #endregion

        #region 释放资源
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handleId"></param>
        public void Release<T>(int handleId)
        {
            for(int i = 0; i < m_assetHandle.Count; i++)
            {
                AssetHandleBase ahb = m_assetHandle[i];
                if(handleId == ahb.Id)
                {
                    ahb.Release();
                    break;
                }
            }         
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ah"></param>
        public void Release<T>(AssetHandle<T> ah)
        {
            ah.Release();                   
        }
        /// <summary>
        /// 释放所有资源
        /// </summary>
        public void ReleaseAll()
        {      
            for(int i = m_assetHandle.Count - 1; i >= 0; i--)
            {               
                m_assetHandle[i].Release();

            }        
            m_assetHandle.Clear();
        }
       

        #endregion
    }

