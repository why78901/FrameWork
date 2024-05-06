using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class QuickStart : MonoBehaviour
{
    public Vector3 pos = new Vector3(5, 2, 0);
    void Start()
    {
        AudioManager.GetInstance().LoadBanks(CreateCube);
    }

    private void CreateCube()
    {
        // var pos = new Vector3(2, 2, -10);
        // var pos = new Vector3(5, 2, 0);
        // pos = Vector3.zero;
        // RuntimeManager.PlayOneShot("event:/New Event", pos);
        var eventInstant = RuntimeManager.CreateInstance("event:/sfx/ground/event/onfire");
        eventInstant.setVolume(1);
        // eventInstant.set3DAttributes(pos.To3DAttributes());
        eventInstant.start();
        eventInstant.release();

        //
        // eventInstant.getDescription(out var description);
        // description.getLength(out var length);
        // Debug.LogError("len:" + length);
        // var cube = ResourceLoader.Load<GameObject>("Prefabs/Cube.prefab");
        // if (cube)
        // {
        //     Instantiate(cube, transform);
        // }
    }

}
