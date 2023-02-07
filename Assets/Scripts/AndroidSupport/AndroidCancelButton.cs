using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidCancelButton : MonoBehaviour {

    public void Cancel()
    {
        FindUtils.GetPlayer().Cancel();
        FindUtils.GetPlayer().SetXSpeed(0);
    }
}
