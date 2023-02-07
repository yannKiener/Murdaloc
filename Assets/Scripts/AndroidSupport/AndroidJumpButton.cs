using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidJumpButton : MonoBehaviour {

    public void StartJumping()
    {
        FindUtils.GetPlayer().StartJumping();
    }

    public void StopJumping()
    {
        FindUtils.GetPlayer().StopJumping();
    }
}
