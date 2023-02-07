using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableBehaviour : MonoBehaviour
{
    bool checkDistance = false;

    /*
     * Use this to handle click without condition 
     * <returns> boolean to show message on click "It's too far" or not  </returns> 
     */
    public abstract bool OnClick();

    /*
     * Use this to handle click with condition : player is close enough 
     * this will run a check regulary : if player gets far away from object, will run 
     * <seealso>"OnPlayerFarOrDead</seealso>
     */
    public abstract void OnClickPlayerCloseEnough();

    /*
     * This will run if player gets far away from object.
     */
    public abstract void OnPlayerFarOrDead();

    /*
     * This is used in place of regular "Update()"
     */
    public abstract void OnUpdate();

    void OnMouseDown()
    {
        bool showMessage = OnClick();
        if (FindUtils.IsPlayerNearAndAlive(transform, Constants.InteractDistance, showMessage))
        {
            checkDistance = true;
            OnClickPlayerCloseEnough();
        }
    }


    void Update()
    {
        OnUpdate();
        if (checkDistance)
        {
            InvokeRepeating("DoSomethingIfPlayerFarOrDead", 1f, 1f);
            checkDistance = false;
        }
    }

    void DoSomethingIfPlayerFarOrDead()
    {
        if (!FindUtils.IsPlayerNearAndAlive(transform, Constants.InteractDistance, false))
        {
            CancelInvoke();
            OnPlayerFarOrDead();
        }
    }
}
