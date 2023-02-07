using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfNotAndroid : MonoBehaviour {

    // Use this for initialization
    private void Awake()
    {
        if(SystemInfo.deviceType != DeviceType.Handheld)
        {
            Destroy(this.gameObject);
        }
    }
}
