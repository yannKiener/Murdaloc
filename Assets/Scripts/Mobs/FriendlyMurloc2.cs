using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMurloc2 : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Friendly mob = gameObject.AddComponent<Friendly>();
        mob.Initialize("Friendly Murloc2", 1, false);
        mob.AddDialog("FriendlyMurloc2");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
