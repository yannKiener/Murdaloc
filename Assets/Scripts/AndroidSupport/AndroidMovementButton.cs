using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidMovementButton : MonoBehaviour {
    public bool moveLeft;
    float xSpeed = 1;

    private void Start()
    {
        if (moveLeft)
        {
            xSpeed = -1;
        }
    }

    public void MovePlayer()
    {
        FindUtils.GetPlayer().SetXSpeed(xSpeed);
    }

    public void StopMovingPlayer()
    {
        FindUtils.GetPlayer().SetXSpeed(0);
    }
}
