using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Objective {
    bool IsOver();
    void Update(Hostile enemy);
    string GetDescription();
    
}
