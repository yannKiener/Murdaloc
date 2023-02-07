using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Friendly : Character
{

    public string DialogName;
    public List<SellTable> SellTable;
    private Dictionary<Item, bool> sellTable = new Dictionary<Item, bool>();


    new void Start()
    {
        base.Start();
        if (DialogName != null)
        {
            AddDialog(DialogName);
        }

        if(SellTable != null) { 
            foreach (SellTable st in SellTable)
            {
                sellTable.Add(Items.GetItemFromDB(st.itemName), st.hasInfinite);
            }
        }

    }

    public Dictionary<Item, bool> GetSellTable()
    {
        return sellTable;
    }

    void OnMouseDown()
    {
        FindUtils.GetPlayer().SetTarget(this);
        //check distance
        FindUtils.GetDialogBoxComponent().Initialize(this);
    }
}
