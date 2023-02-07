using UnityEngine;

[System.Serializable]
public class Friendly : Character
{

    public string DialogName;


    new void Start()
    {
        base.Start();
        if (DialogName != null)
        {
            AddDialog(DialogName);
        }
        
    }
    void OnMouseDown()
    {
        FindUtils.GetPlayer().SetTarget(this);
        //check distance
        FindUtils.GetDialogBoxComponent().Initialize(this);
    }
}
