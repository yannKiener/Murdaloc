using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;


[System.Serializable]
public static class InputManager 
{

    static string interfaceFileName = "/interface.bind";
    static string playerFileName = "/player.bind";
    static string actionFileName = "/action.bind";

    static Dictionary<string, string> bindingsInterface = new Dictionary<string, string>();
    static Dictionary<string, string> bindingsPlayer = new Dictionary<string, string>();
    static Dictionary<string, string> bindingsActionBar = new Dictionary<string, string>();
    static Dictionary<string, string> bindings = new Dictionary<string, string>();

    private static void loadDefaultPlayerBindings()
    {
        bindingsPlayer = new Dictionary<string, string>
        {
            {"MoveLeft","q"},
            {"MoveRight","d"},
            {"Jump","space"},
            {"CycleTargets","tab"}
        };
    }

    private static void loadDefaultInterfaceBindings()
    {
        bindingsInterface = new Dictionary<string, string>
        {
            {"ShowHideSpellBook","v"},
            {"ShowHideInventory","b"},
            {"ShowHideCharacterSheet","c"},
            {"ShowHideQuestLog","l"},
            {"ShowHideTalentSheet","n"},
            {"Cancel","escape"}
        };
    }

    private static void loadDefaultActionBarBindings()
    {
        bindingsActionBar = new Dictionary<string, string>
        {
            {"ActionBar1","1"},
            {"ActionBar2","2"},
            {"ActionBar3","3"},
            {"ActionBar4","4"},
            {"ActionBar5","5"},
            {"ActionBar6","a"},
            {"ActionBar7","e"},
            {"ActionBar8","r"},
            {"ActionBar9","t"},
            {"ActionBar10","f"}
        };
    }

    public static bool IsButtonDown(string buttonName){
        return Input.GetKeyDown(bindings[buttonName]);
    }

    public static bool IsButtonUp(string buttonName)
    {
        return Input.GetKeyUp(bindings[buttonName]);
    }

    public static bool IsButtonPressed(string buttonName)
    {
        return Input.GetKey(bindings[buttonName]);
    }

    public static void SaveBindings()
    {
        BinaryFormatter bf = new BinaryFormatter();

        FileStream interfaceFile = File.Create(Application.persistentDataPath + interfaceFileName);
        bf.Serialize(interfaceFile, bindingsInterface);
        interfaceFile.Close();

        FileStream playerFile = File.Create(Application.persistentDataPath + playerFileName);
        bf.Serialize(playerFile, bindingsPlayer);
        playerFile.Close();

        FileStream actionFile = File.Create(Application.persistentDataPath + actionFileName);
        bf.Serialize(actionFile, bindingsActionBar);
        actionFile.Close();

        LoadBindingsToGame();
    }

    public static void LoadBindings()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + interfaceFileName))
        {
            FileStream file = File.Open(Application.persistentDataPath + interfaceFileName, FileMode.Open);
            bindingsInterface = (Dictionary<string, string>)bf.Deserialize(file);
            file.Close();
        } else
        {
            loadDefaultInterfaceBindings();
        }

        if (File.Exists(Application.persistentDataPath + playerFileName))
        {
            FileStream file = File.Open(Application.persistentDataPath + playerFileName, FileMode.Open);
            bindingsPlayer = (Dictionary<string, string>)bf.Deserialize(file);
            file.Close();
        } else
        {
            loadDefaultPlayerBindings();
        }

        if (File.Exists(Application.persistentDataPath + actionFileName))
        {
            FileStream file = File.Open(Application.persistentDataPath + actionFileName, FileMode.Open);
            bindingsActionBar = (Dictionary<string, string>)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            loadDefaultActionBarBindings();
        }
        LoadBindingsToGame();
    }

    private static void LoadBindingsToGame()
    {
        bindings = new Dictionary<string, string>();
        bindingsInterface.ToList().ForEach(x => bindings.Add(x.Key, x.Value));
        bindingsPlayer.ToList().ForEach(x => bindings.Add(x.Key, x.Value));
        bindingsActionBar.ToList().ForEach(x => bindings.Add(x.Key, x.Value));
    }

    public static void LoadDefault()
    {
        loadDefaultPlayerBindings();
        loadDefaultInterfaceBindings();
        loadDefaultActionBarBindings();
    }
}
