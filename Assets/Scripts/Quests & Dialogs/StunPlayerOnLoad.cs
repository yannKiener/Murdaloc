using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunPlayerOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Player player = FindUtils.GetPlayer();
        EffectsOnTime.Get("Asleep").Apply(null, player);
        Animator playerAnim = player.gameObject.GetComponent<Animator>();
        playerAnim.Play("Sleep");
    }
}
