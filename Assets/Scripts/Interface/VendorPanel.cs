using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendorPanel : MonoBehaviour
{

    public GameObject sellSlotContainer;
    public Friendly vendor;

    public void Initialize(Friendly vendor)
    {
        this.vendor = vendor;
        Dictionary<Item, bool> itemList = vendor.GetSellTable();
        clearChilds(transform);
        if(itemList != null)
        {
            foreach (KeyValuePair<Item,bool> item in itemList)
            {
                GameObject sellContainer = Instantiate(sellSlotContainer,transform);
                sellContainer.GetComponentInChildren<SellSlot>().Initialize(item.Key, item.Value);
            }
        }
        transform.parent.Find("Name").GetComponent<Text>().text = vendor.GetName();
    }

    public void RefreshSelf()
    {
        if (gameObject.activeInHierarchy)
        {
            Initialize(vendor);
        }
    }

    public Friendly GetVendor()
    {
        return vendor;
    }

    void OnEnable()
    {
        Interface.OpenVendor();
    }

    void OnDisable()
    {
        Interface.CloseVendor();
    }

    private void clearChilds(Transform t)
    {
        foreach (Transform c in t)
        {
            GameObject.Destroy(c.gameObject);
        }
    }
}