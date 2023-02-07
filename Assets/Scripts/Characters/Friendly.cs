using UnityEngine;

[System.Serializable]
public class Friendly : Character
{
    void OnMouseDown()
    {
        FindUtils.GetPlayer().SetTarget(this);
        //check distance
        FindUtils.GetDialogBoxComponent().Initialize(this);
    }
}
