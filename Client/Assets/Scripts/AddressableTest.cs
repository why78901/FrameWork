using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddressableTest : MonoBehaviour
{
    private AssetOwner _ao = new AssetOwner();
    private AssetHandle<GameObject> _ah;      //得到的资源结果处理者


    private void Start()
    {
        _ah = _ao.Instantiate("Assets/AddressResources/Prefabs/Cube.prefab");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (_ah != null)
            {
                _ah.Release();
            }
        }
    }
}
