using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    private Item item;
    bool hasInfinite;

    public void Initialize(Item item, bool hasInfinite)
    {
        this.hasInfinite = hasInfinite;
        this.item = item;
        GetComponent<Image>().sprite = item.GetImageAsSprite();
    }

    private void Buy()
    {
        Inventory inv = FindUtils.GetInventoryGrid();
        if (item != null)
        {
            if (inv.HasEnoughCash((int)(item.GetSellPrice() * Constants.BuyPriceMultiplier)))
            {
                if (inv.AddItem(item))
                {
                    Interface.CoinSound();
                    inv.RemoveCash((int)(item.GetSellPrice() * Constants.BuyPriceMultiplier));
                    if (!hasInfinite)
                        Destroy(this.gameObject);
                }
            }

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Buy();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
            Interface.DrawToolTip(item.GetName(), item.GetDescription(), (int)(item.GetSellPrice() * Constants.BuyPriceMultiplier));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Interface.RemoveToolTip();
    }

    void OnDisable()
    {
        Interface.RemoveToolTip();
    }
}
