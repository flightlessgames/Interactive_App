using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class fileUtility
{
    public static string SAVE_LOCATION { get; private set; } = "...";
    public static string POTIONS_LOCATION { get; private set; } = "...";

    public static SaveFile SaveObject = new SaveFile();

    private static bool _isInitialized = false;

    public static void InitializeLoadSettings()
    {
        Debug.Log("Initializing");

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            //using wierd APK Web search to define our file path[s]
            CreateAPKPath(SAVE_LOCATION, "LoadFileData" + StateController.LoadFilePosition + ".txt");
            CreateAPKPath(POTIONS_LOCATION, "Potions.csv");
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //for windows, assume we're in editor, use the folder directory
            SAVE_LOCATION = Application.streamingAssetsPath + "/LoadFileData" + StateController.LoadFilePosition + ".txt";
            POTIONS_LOCATION = Application.streamingAssetsPath + "/Potions.csv";
        }

        _isInitialized = true;

        //after setting paths, write to current SaveObject our previous SaveFile. Don't allow a new save to overwrite on accident
        Debug.Log("setting first Load");
        Load();
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
        Debug.Log("Load");

        if (!_isInitialized) { InitializeLoadSettings(); }

        //SAVE_LOCATIOn is determined by the StateController.LoadFilePosition, which picks from our 4 predetermined locations
        if (File.Exists(SAVE_LOCATION))
        {
            string saveString = File.ReadAllText(SAVE_LOCATION);
            Debug.Log("loaddata test, saveString: " + saveString);

            SaveObject = JsonUtility.FromJson<SaveFile>(saveString);
            /*
            Debug.Log("loaddata test, SaveObject data 'gold' = " + SaveObject.Gold);
            Debug.Log("SaveObject data 'ingredients' = " + SaveObject.UnlockedIngredients + ". Count: " + SaveObject.UnlockedIngredients.Length);
            Debug.Log("SaveObject data 'recent recipes' = " + SaveObject.RecentRecipesAsArray + ". Count: " + SaveObject.RecentRecipesAsArray.Length);
            */
        }
        else
        {
            Debug.Log("failed, creating save");
            Save();

            Debug.Log("new load");
            //dangerous, check for Save() to write to same location as Load()
            Load();
        }
        
    }

    public static void Save()
    {
        Debug.Log("Save");

        if (!_isInitialized) { InitializeLoadSettings(); }

        //SaveObject should have values at this point
        /*
        Debug.Log("savedata test, SaveObject data 'gold' = " + SaveObject.Gold);
        Debug.Log("SaveObject data 'ingredients' = " + SaveObject.UnlockedIngredients + ". Count: " + SaveObject.UnlockedIngredients.Length);
        Debug.Log("SaveObject data 'recent recipes' = " + SaveObject.RecentRecipesAsArray + ". Count: " + SaveObject.RecentRecipesAsArray.Length);
        */

        string saveString = JsonUtility.ToJson(SaveObject);
        Debug.Log("savedata test, saveString: " + saveString);

        File.WriteAllText(SAVE_LOCATION, saveString);

        Load();
    }

    [serializable]
    public class SaveFile
    {
        //list of previous specific potions crafted, up to a limit for Achievement-History Page
        public List<_devCrafting.Recipe> recentRecipes;
        public _devCrafting.Recipe[] RecentRecipesAsArray { get { return recentRecipes.ToArray(); } }

        //list of previous "unlocked" potion recipes "known" for Achievement-Achievements Page
        public bool[] unlockedIngredients;
        public bool[] UnlockedIngredients { get { return unlockedIngredients; } }

        //all ingredient_sObj quantities
        //TODO get ALL ingredient_sObj into a List<> somehow?

        //all edited ingredient_sObj / journal_sObj datapoints (vector3)?
        //TOGO get ALL journal_sObj? or re-use ingredient_sObj list to calculate this value

        //current gold amount
        public int gold;
        public int Gold { get { return gold; } }

        //time spent playing?

        public SaveFile()
        {
            //default values if no data used for constructor

            //new empty list of recipes
            recentRecipes = new List<_devCrafting.Recipe>();

            //default empty recipe
            _devCrafting.Recipe recipe = new _devCrafting.Recipe("Baby's First Recipe", Color.white, new List<Ingredients_sObj>());
            AddRecipe(recipe);

            //new array of all ingredient/bool set to false
            unlockedIngredients = new bool[60];
            foreach (bool potionState in unlockedIngredients)
            {
                potionState.Equals(false);
            }

            //start with 100g
            gold = 100;
        }

        public void AddRecipe(_devCrafting.Recipe recipe)
        {
            Debug.Log("Adding a Recipe");
            //adds a recipe to the list
            recentRecipes.Add(recipe);

            //retains only the last 5 recipes crafted
            while (recentRecipes.Count > 5)
            {
                recentRecipes.RemoveAt(0);
            }
        }

        ///<summary>
        ///Only Call using Gold_sObj.currentGold
        ///</summary>
        public void SetGoldValue(int amount)
        {
            //only call function using gold_sObj.currentGold
            Debug.Log("Changing Gold Amount");
            gold = amount;
        }
        
    }
}
