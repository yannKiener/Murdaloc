using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendorPanel : MonoBehaviour
{

    public GameObject sellSlotContainer;

    public void Initialize(Dictionary<Item, bool> itemList)
    {
        clearChilds(transform);
        if(itemList != null)
        {
            foreach (KeyValuePair<Item,bool> item in itemList)
            {
                GameObject sellContainer = Instantiate(sellSlotContainer,transform);
                sellContainer.GetComponentInChildren<SellSlot>().Initialize(item.Key, item.Value);
            }
        }
    }

    void OnEnable()
    {
        FindUtils.GetInterface().OpenVendor();
    }

    void OnDisable()
    {
        FindUtils.GetInterface().CloseVendor();
    }

    private void clearChilds(Transform t)
    {
        foreach (Transform c in t)
        {
            GameObject.Destroy(c.gameObject);
        }
    }
}