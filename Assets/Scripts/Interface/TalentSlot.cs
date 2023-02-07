using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalentSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Talent talent;
    private Specialisation specialisation;
    private int slotNumber = 0;


    void OnEnable()
    {
        UpdateStacksText();
    }

    void Start()
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 0.9f);
        slotNumber = int.Parse(gameObject.name);
        UpdateStacksText();
    }

    public void UpdateStacksText()
    {
        if(talent != null)
        {
            transform.GetComponentInChildren<Text>().text = talent.GetStacks() + " / " + talent.GetMaxStacks();
            if (IsTalentClickable())
            {
                transform.GetComponentInChildren<Text>().color = Color.white;
            } else
            {
                transform.GetComponentInChildren<Text>().color = Color.red;
            }
        }
    }


    private bool IsTalentClickable()
    {
        return ((slotNumber - 1) / 4) <= (specialisation.GetPointsInSpec() / 5) && LinkedTalentIsNullOrMaxed();
    }
    
    private bool LinkedTalentIsNullOrMaxed()
    {
        return talent.GetLinkedTalent() == null || (specialisation.GetTalentTree().ContainsValue(talent.GetLinkedTalent()) && talent.GetLinkedTalent().IsMaxed());
    }

    public void SetTalent(Specialisation s, Talent t)
    {
        specialisation = s;
        talent = t;
        this.GetComponent<Image>().sprite = talent.GetImage();
    }

    public void DeactivateTalent()
    {
        specialisation = null;
        talent = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(talent != null && specialisation != null && IsTalentClickable() && talent.AddOne())
        {
            specialisation.AddPointInSpec();
            if(specialisation.GetPointsInSpec() % 5 == 0 || talent.IsMaxed())
            {
                FindUtils.GetTalentSheetGrid().ResetSlotText();
            } else
            {
                UpdateStacksText();
            }
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
