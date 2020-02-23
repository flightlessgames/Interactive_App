using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class fileUtility
{
    public static string SAVE_LOCATION { get; private set; } = "...";
    public static string POTIONS_LOCATION { get; private set; } = "...";

    public static SaveFile SaveObject = new SaveFile();

    public static void InitializeLoadSettings()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            Debug.Log("We're on Handheld");
            CreateAPKPath(SAVE_LOCATION, "LoadFileData" + StateController.LoadFilePosition + ".txt");
            CreateAPKPath(POTIONS_LOCATION, "Potions.csv");
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //for windows, assume we're in editor, use the folder directory
            Debug.Log("We're on Desktop");
            SAVE_LOCATION = Application.streamingAssetsPath + "/LoadFileData" + StateController.LoadFilePosition + ".txt";
            POTIONS_LOCATION = Application.streamingAssetsPath + "/Potions.csv";
        }
    }

    public static void CreateAPKPath(string overwritePath, string targetFileName)
    {
        //borrowing code from Unity Answers user RobertCigna: https://answers.unity.com/questions/1087159/reading-text-file-on-android.html
        //still using code from 2015, including outdated WWW object type, might look into UnityWebRequest type, but don't want to break system.
        //for Android "Handheld" we need to use a URL path
        string tempPath = Path.Combine(Application.streamingAssetsPath, targetFileName);

        // Android only use WWW to read file
        WWW reader = new WWW(tempPath);   //www is obsolete, UnityWebReader is the same functionality I hope
        while (!reader.isDone) { }

        overwritePath = Application.persistentDataPath + "/db";
        File.WriteAllBytes(overwritePath, reader.bytes);
    }


    //code adapted from CodeMonkey on YouTube: https://www.youtube.com/watch?time_continue=44&v=6uMFEM-napE&feature=emb_logo
    //mostly standard utility code, but I needed a reference to figure it out
    public static void Load()
    {
        //SAVE_LOCATIOn is determined by the StateController.LoadFilePosition, which picks from our 4 predetermined locations
        if (File.Exists(SAVE_LOCATION))
        {
            string saveString = File.ReadAllText(SAVE_LOCATION);
            SaveObject = JsonUtility.FromJson<SaveFile>(saveString);
        }
        else
        {
            Save();

            //dangerous, check for Save() to write to same location as Load()
            Load();
        }
        
    }

    public static void Save()
    {
        string saveString = JsonUtility.ToJson(SaveObject);
        File.WriteAllText(SAVE_LOCATION, saveString);
    }

    public class SaveFile
    {
        //list of previous specific potions crafted, up to a limit for Achievement-History Page
        List<_devCrafting.Recipe> recentRecipes;

        //list of previous "unlocked" potion recipes "known" for Achievement-Achievements Page
        bool[] unlockedPotions;

        //all ingredient_sObj quantities
        //TODO get ALL ingredient_sObj into a List<> somehow?

        //all edited ingredient_sObj / journal_sObj datapoints (vector3)?
        //TOGO get ALL journal_sObj? or re-use ingredient_sObj list to calculate this value

        //current gold amount
        int gold;

        //time spent playing?

        public SaveFile()
        {
            recentRecipes = new List<_devCrafting.Recipe>();

            unlockedPotions = new bool[60];
            foreach (bool potionState in unlockedPotions)
            {
                potionState.Equals(false);
            }

            gold = 100;

        }

        public void AddRecipe(_devCrafting.Recipe recipe)
        {
            //adds a recipe to the list
            recentRecipes.Add(recipe);

            //retains only the last 5 recipes crafted
            if (recentRecipes.Count > 5)
            {
                recentRecipes.RemoveAt(0);
            }
        }
        
    }
}
