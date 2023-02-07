using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSaveButton : MonoBehaviour {

    public void DeleteSave()
    {
        string saveName = transform.GetComponentInParent<Text>().text;

        if (File.Exists(Application.persistentDataPath +"/"+ saveName + ".murk"))
        {
            File.Delete(Application.persistentDataPath + "/" + saveName + ".murk");
            GameObject.Destroy(transform.parent.parent.gameObject);
        }
    }
}
