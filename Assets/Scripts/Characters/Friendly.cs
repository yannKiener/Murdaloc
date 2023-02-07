using UnityEngine;

[System.Serializable]
public class Friendly : Character
{
    void OnMouseDown()
    {
        Debug.Log("Clicked on friendly");
        Debug.Log(dialog.GetText());
        FindUtils.GetPlayer().SetTarget(this);
    }
}
