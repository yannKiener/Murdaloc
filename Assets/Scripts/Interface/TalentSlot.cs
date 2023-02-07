using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalentSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Talent talent;

    void Start()
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
    }

    public void UpdateStacksText()
    {
        if(talent != null)
        {
            transform.GetComponentInChildren<Text>().text = talent.GetStacks() + " / " + talent.GetMaxStacks();
        }
    }

    public void SetTalent(Talent t)
    {
        talent = t;
        this.GetComponent<Image>().sprite = talent.GetImage();
        UpdateStacksText();

    }

    public void RemoveTalent()
    {
        talent = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(talent != null)
        {
            talent.AddOne();
            UpdateStacksText();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Interface.DrawToolTip(talent.GetName(), talent.GetDescription());
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
