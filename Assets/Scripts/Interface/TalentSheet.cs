using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentSheet : MonoBehaviour
{
    public GameObject NoTalentsText;
    public GameObject TalentsContainer;
    public GameObject TalentsSheetGrid;
    public GameObject TalentPointsText;
    public GameObject Spec1GObject;
    public GameObject Spec2GObject;
    public GameObject Spec3GObject;

    Specialisation spec1;
    Specialisation spec2;
    Specialisation spec3;
        


    void OnEnable()
    {
        if (FindUtils.GetSpellBook().activeSelf)
        {
            InterfaceUtils.ShowHideSpellBook();
        }
        if (spec1 != null)
        {
            LoadTalentsForSpec(spec1);
        } else if(spec2 != null)
        {
            LoadTalentsForSpec(spec2);
        } else if(spec3 != null)
        {
            LoadTalentsForSpec(spec3);
        }
        Interface.OpenSpellbook();
    }

    public void UpdateTalentsPointsRemainingText()
    {
        TalentPointsText.GetComponent<Text>().text = FindUtils.GetPlayer().GetTalentPoints().ToString();
    }

    void OnDisable()
    {
        Interface.CloseSpellbook();
    }

    void Start()
    {

        DeactiveAllTalentSlots();

        UpdateSpecSheet();
    }

    public void ResetTalents()
    {
        ResetSpec(spec1);
        ResetSpec(spec2);
        ResetSpec(spec3);
        ResetSlotText();
        FindUtils.GetPlayer().ResetTalentPoints();
        UpdateTalentsPointsRemainingText();
    }

    public void ResetSlotText()
    {
        foreach (Transform t in TalentsSheetGrid.transform)
        {
            t.gameObject.GetComponent<TalentSlot>().UpdateStacksText();
        }
    }

    private void ResetSpec(Specialisation spec)
    {
        if(spec != null)
        {
            foreach (KeyValuePair<int, Talent> kv in spec.GetTalentTree())
            {
                kv.Value.Reset();
            }
            spec.ResetPointsInSpec();
        }
    }

    public void SetSpec1(Specialisation spec)
    {
        spec1 = spec;
        UpdateSpecSheet();
    }
    public void SetSpec2(Specialisation spec)
    {
        spec2 = spec;
        UpdateSpecSheet();
    }
    public void SetSpec3(Specialisation spec)
    {
        spec3 = spec;
        UpdateSpecSheet();
    }
    private void UpdateSpecSheet()
    {
        if (spec1 != null || spec2 != null || spec3 != null)
        {
            LoadSpecNames();
            NoTalentsText.SetActive(false);
            TalentsContainer.SetActive(true);
        }
        else
        {
            NoTalentsText.SetActive(true);
            TalentsContainer.SetActive(false);
        }
    }

    private void LoadSpecNames()
    {
        UpdateNameForSpec(Spec1GObject, spec1);
        UpdateNameForSpec(Spec2GObject, spec2);
        UpdateNameForSpec(Spec3GObject, spec3);
    }

    private void UpdateNameForSpec(GameObject textContainer, Specialisation spec)
    {
        if (spec != null && spec.GetName() != null && spec.GetName().Length > 0)
        {
            textContainer.GetComponentInChildren<Text>().text = spec.GetName();
            textContainer.SetActive(true);
        } else
        {
            textContainer.SetActive(false);
        }
    }

    private void LoadTalentsForSpec(Specialisation spec)
    {
        DeactiveAllTalentSlots();
        foreach (KeyValuePair<int, Talent> kv in spec.GetTalentTree())
        {
            GameObject talentSlot = TalentsSheetGrid.transform.Find(kv.Key.ToString()).gameObject;
            talentSlot.GetComponent<TalentSlot>().SetTalent(spec, kv.Value);
            talentSlot.SetActive(true);
        }
    }
    
    private void DeactiveAllTalentSlots()
    {
        foreach(Transform t  in TalentsSheetGrid.transform)
        {
            t.gameObject.GetComponent<TalentSlot>().DeactivateTalent();
            t.gameObject.SetActive(false);
        }
    }

    public void SelectSpecOne()
    {
        LoadTalentsForSpec(spec1);
    }
    public void SelectSpecTwo()
    {
        LoadTalentsForSpec(spec2);
    }
    public void SelectSpecThree()
    {
        LoadTalentsForSpec(spec3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
