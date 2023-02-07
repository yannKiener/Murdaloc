using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSheet : MonoBehaviour {

    private Transform statsGrid;
    private Player player;
    private GameObject autoAttackDamage2GO;
    private GameObject autoAttackSpeed2GO;
    private GameObject autoAttackDamage2TextGO;
    private GameObject autoAttackSpeed2TextGO;
    
    private void OnEnable()
    {
        if (statsGrid == null)
        {
            statsGrid = transform.Find("StatsScrollBar").Find("StatsGrid");
        }
        if (player == null)
        {
            player = FindUtils.GetPlayer();
        }
        if (autoAttackDamage2GO == null)
        {
            autoAttackDamage2GO = statsGrid.Find("AttackDamage2").gameObject;
        }
        if (autoAttackSpeed2GO == null)
        {
            autoAttackSpeed2GO = statsGrid.Find("AttackSpeed2").gameObject;
        }
        if (autoAttackDamage2TextGO == null)
        {
            autoAttackDamage2TextGO = statsGrid.Find("AttackDamage2Text").gameObject;
        }
        if (autoAttackSpeed2TextGO == null)
        {
            autoAttackSpeed2TextGO = statsGrid.Find("AttackSpeed2Text").gameObject;
        }
        InvokeRepeating("UpdateStats", 0f, 1f);
    }

    public void OnDisable()
    {
        CancelInvoke("UpdateStats");
    }

    public void UpdateStats()
    {
        if (player != null)
        {
            Stats playerStats = FindUtils.GetPlayer().GetStats();

            UpdateTextFor("AttackDamage1",player.GetMinAutoAttack1Damage() + " - " + player.GetMaxAutoAttack1Damage());
            UpdateTextFor("AttackSpeed1", player.GetAutoAttack1Speed().ToString("0.0"));
            if (player.GetMaxAutoAttack2Damage() != 0 && player.GetAutoAttack2Speed() != 0)
            {
                autoAttackDamage2GO.SetActive(true);
                autoAttackSpeed2GO.SetActive(true);
                autoAttackDamage2TextGO.SetActive(true);
                autoAttackSpeed2TextGO.SetActive(true);
                UpdateTextFor("AttackDamage2", player.GetMinAutoAttack2Damage() + " - " + player.GetMaxAutoAttack2Damage());
                UpdateTextFor("AttackSpeed2", player.GetAutoAttack2Speed().ToString("0.0"));
            }
            else
            {
                autoAttackDamage2GO.SetActive(false);
                autoAttackSpeed2GO.SetActive(false);
                autoAttackDamage2TextGO.SetActive(false);
                autoAttackSpeed2TextGO.SetActive(false);
            }
            UpdateTextFor("Force", playerStats.Force.ToString());
            UpdateTextFor("Agility", playerStats.Agility.ToString());
            UpdateTextFor("Intelligence", playerStats.Intelligence.ToString());
            UpdateTextFor("Spirit", playerStats.Spirit.ToString());
            UpdateTextFor("Stamina", playerStats.Stamina.ToString());
            UpdateTextFor("Haste",  playerStats.Haste + "%");
            UpdateTextFor("Critical", playerStats.Critical + "%");
            UpdateTextFor("Power", playerStats.Power + "%");
        }
    }

    private void UpdateTextFor(string gameObjectName, string text)
    {
        statsGrid.Find(gameObjectName).GetComponent<Text>().text = text;
    }
}
