using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class fileUtility
{
    public static string SAVE_LOCATION { get; private set; } = "...";
    public static string POTIONS_LOCATION { get; private set; } = "...";

    public static SaveFile SaveObject = new SaveFile();
    public static SaveFile _searchObject = new SaveFile();

    private static bool _isInitialized = false;
    public static Shop _shop = null;
    //public static Text _mobileDebug = null;

    /// <summary>
    /// Sets settings to ensure we're reading the right JSON.txt save file
    /// </summary>
    public static void InitializeLoadSettings()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            //using wierd APK Web search to define our file path[s]

            //borrowing code from Unity Answers user RobertCigna: https://answers.unity.com/questions/1087159/reading-text-file-on-android.html
            //still using code from 2015, including outdated WWW object type, might look into UnityWebRequest type, but don't want to break system.
            //for Android "Handheld" we need to use a URL path
            string tempPath = Path.Combine(Application.streamingAssetsPath, StateController.LoadFilePosition + ".txt");
            // Android only use WWW to read file
            WWW reader = new WWW(tempPath);   //www is obsolete, UnityWebReader is the same functionality I hope
            while (!reader.isDone) { }

            SAVE_LOCATION = Application.persistentDataPath + StateController.LoadFilePosition + ".txt";
            File.WriteAllBytes(SAVE_LOCATION, reader.bytes);

            //not using CreateAPKPath because of Pointers/Reference
            string potionTemp = Path.Combine(Application.streamingAssetsPath, "Potions.csv");
            // Android only use WWW to read file
            WWW potionReader = new WWW(potionTemp);   //www is obsolete, UnityWebReader is the same functionality I hope
            while (!potionReader.isDone) { }

            POTIONS_LOCATION = Application.persistentDataPath + "Potions.csv";
            File.WriteAllBytes(POTIONS_LOCATION, potionReader.bytes);
        }

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            Debug.Log("Desktop");
            //for windows, assume we're in editor, use the folder directory
            SAVE_LOCATION = Application.streamingAssetsPath + "/LoadFileData" + StateController.LoadFilePosition + ".txt";
            POTIONS_LOCATION = Application.streamingAssetsPath + "/Potions.csv";
        }

        _isInitialized = true;

        //after initializing which file to pull from, do initial pull
        Load();
    }

    //code adapted from CodeMonkey on YouTube: https://www.youtube.com/watch?time_continue=44&v=6uMFEM-napE&feature=emb_logo
    //mostly standard utility code, but I needed a reference to figure it out
    /// <summary>
    /// Reads JSON.txt from assets and Constructs a SaveFile Object
    /// </summary>
    public static void Load()
    {
        if (!_isInitialized) { InitializeLoadSettings(); }

        //SAVE_LOCATIOn is determined by the StateController.LoadFilePosition, which picks from our 4 predetermined locations
        if (!File.Exists(SAVE_LOCATION))
        {
            Debug.Log("Could not load from Save File Location");
            return;
        }

        string saveString = File.ReadAllText(SAVE_LOCATION);
        JsonUtility.FromJsonOverwrite(saveString, SaveObject);
        
        //recall data from Ingredient Quantity
        for (int i = 0; i < _shop.Inventory.Count; i++)
        {
            //if our saved quantity is DIFFERENT than the current reported quantity
            if(SaveObject.ingredientsQuantity[i] != _shop.Inventory[i].Quantity)
            {
                //declare new setting
                _shop.Inventory[i].SetQuantity(SaveObject.ingredientsQuantity[i]);
            }
            
            //while still using loaded recall data we can unlock the jounral pages for ingredients with quantities >-2 (-1 is unlocked infifnite, and 0+ is unlocked finite)
            //TODO Jounral Scripts
        }

        Debug.Log("Loaded Save: " + StateController.LoadFilePosition);
    }

    /// <summary>
    /// Creates JSON string out of SaveObject Data
    /// </summary>
    public static void Save()
    {
        if (!_isInitialized)
        {
            InitializeLoadSettings();
            return;
        }

        if (File.Exists(SAVE_LOCATION))
        {
            //if file exists, run code to retain current dataset
            //assuming we have a SaveObject, update its SaveTime
            SaveObject.RecentSaveTime = Time.time;

            //save each ingredient's qty to the SaveObject's int[]
            for (int i = 0; i < _shop.Inventory.Count; i++)
            {
                SaveObject.ingredientsQuantity[i] = _shop.Inventory[i].Quantity;
            }
        }
        //if Does Not Exist, then save a new SaveFile object
        else
        {
            SaveObject = new SaveFile();
        }


        string saveString = JsonUtility.ToJson(SaveObject, true);

        //if File Does Not Exist, it will create a file at location
        File.WriteAllText(SAVE_LOCATION, saveString);
    }

    /// <summary>
    /// Use Only for LoadData screen in MainMenu
    /// </summary>
    public static SaveFile SearchForSaveData(int FileToSearch)
    {
        string searchPath = "...";

        //code taken from Initializer
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            string tempPath = Path.Combine(Application.streamingAssetsPath, FileToSearch + ".txt");
            // Android only use WWW to read file
            WWW reader = new WWW(tempPath);   //www is obsolete, UnityWebReader is the same functionality I hope
            while (!reader.isDone) { }

            searchPath = Application.persistentDataPath + FileToSearch + ".txt";
            File.WriteAllBytes(searchPath, reader.bytes);
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        { searchPath = Application.streamingAssetsPath + "/LoadFileData" + FileToSearch + ".txt"; }


        //code taken from loader
        if (File.Exists(searchPath))
        {
            string saveString = File.ReadAllText(searchPath);
            JsonUtility.FromJsonOverwrite(saveString, _searchObject);
        }
        else
        {
            Debug.Log("Could not Load from File");
            _searchObject = new SaveFile();
        }

        return _searchObject;
    }
}
