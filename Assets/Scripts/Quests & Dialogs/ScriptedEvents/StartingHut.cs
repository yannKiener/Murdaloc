using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StartingHut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!DialogStatus.GetStatus("PlayerWokeUp"))
        {   
            Player player = FindUtils.GetPlayer();
            EffectsOnTime.Get("Asleep").Apply(null, player);
            Animator playerAnim = player.gameObject.GetComponent<Animator>();
            playerAnim.Play("Sleep");
        } else
        {
            List<LevelChanger> levelChangersInZone = Resources.FindObjectsOfTypeAll<LevelChanger>().ToList();

            foreach (LevelChanger lvlChanger in levelChangersInZone)
            {
                lvlChanger.gameObject.SetActive(true);
            }
        }
    }
}
