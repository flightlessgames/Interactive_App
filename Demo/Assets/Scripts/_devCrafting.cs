using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class _devCrafting : MonoBehaviour
{
    //TODO make a not bad backend potion/gold reward
    [SerializeField] Gold currGold;
    public class Potion
    {
        public Potion(int a, int b, int c, int d, int e, int f, string name)
        {
            Slow = a;
            Shigh = b;
            Plow = c;
            Phigh = d;
            Mlow = e;
            Mhigh = f;
            Name = name;
        }
        public Potion() { }

        public int Slow, Shigh, Plow, Phigh, Mlow, Mhigh;
        public string Name;
    }

    [SerializeField] private Text _displayText = null;
    [SerializeField] private RawImage _rawPotionColor = null;
    [SerializeField] private StateController _stateController = null;
    [SerializeField] private hotbarGroupController _hotSlotsController = null;

    [SerializeField] private List<craftingSlotController> _ingredients = new List<craftingSlotController>();
    

    private Vector3 _targetPotion = Vector3.zero;
    private string _potionCsvPath = "...";

    private void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            //borrowing code from Unity Answers user RobertCigna: https://answers.unity.com/questions/1087159/reading-text-file-on-android.html
            //still using code from 2015, including outdated WWW object type, might look into UnityWebRequest type, but don't want to break system.
            //for Android "Handheld" we need to use a URL path
            string tempPath = Path.Combine(Application.streamingAssetsPath, "Potions.csv");

            // Android only use WWW to read file
            WWW reader = new WWW(tempPath);   //www is obsolete, UnityWebReader is the same functionality I hope
            while (!reader.isDone) { }

            _potionCsvPath = Application.persistentDataPath + "/db";
            File.WriteAllBytes(_potionCsvPath, reader.bytes);
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        { 
            //for windows, assume we're in editor, use the folder directory
            Debug.Log("We're on Desktop");
            _potionCsvPath = Application.streamingAssetsPath + "/Potions.csv";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CraftPotion();
        }
    }

    public void CraftPotion()  //first we combine the ingredients into a "TargetPotion"
    {
        _targetPotion = Vector3.zero; //reset the "Target" to 0 before each crafting.

        foreach (craftingSlotController slot in _ingredients)
        {
            _targetPotion += slot.ScoreIngredient(); //during each craft, we're adding vector values from ingredient scores instead of "creating a new vector of combined scores"
        }

        ReadCSVFile();

        currGold.currentGold += 5;
    }

    void ReadCSVFile()  //now that we have a "targetpotion" we can compare that to our Potion.csv to read the recipe.
    {
        Debug.Log("Attempting a read");
        StreamReader strReader = new StreamReader(_potionCsvPath);

        //code adapted from RapidGaming on YouTube: https://www.youtube.com/watch?v=xwnL4meq-j8&feature=youtu.be
        Debug.Log("We're reading");
        bool endOfFile = false;
        bool foundMatch = false;    //both endOfFile and foundMatch are our 2 cases to end reading

        while (!endOfFile && !foundMatch)
        {
            string data_String = strReader.ReadLine();  //.ReadLine converts a row of data into a string.

            if(data_String == null) //if a row is null then we have reached the end of our dataset //look into empty row cases, or skipped row cases to ensure we don't bug our data-entry.
            {
                endOfFile = true;
                break;
            }

            var data_values = data_String.Split(',');
            
            //might need to REDO or overhaul when moving onto PlayerJournal scripts. Might convert all entries into .sObjs to store player entry data.
            //feels redundant, look into consolidating into a function
            Potion readPotion = new Potion();
            bool Slow = int.TryParse(data_values[0], out readPotion.Slow);
            bool Shigh = int.TryParse(data_values[1], out readPotion.Shigh);
            bool Plow = int.TryParse(data_values[2], out readPotion.Plow);
            bool Phigh = int.TryParse(data_values[3], out readPotion.Phigh);
            bool Mlow = int.TryParse(data_values[4], out readPotion.Mlow);
            bool Mhigh = int.TryParse(data_values[5], out readPotion.Mhigh);

            //feels redundant, again...
            if (readPotion.Slow > _targetPotion.x|| readPotion.Shigh < _targetPotion.x)
                continue;

            if (readPotion.Plow > _targetPotion.y || readPotion.Phigh < _targetPotion.y)
                continue;

            if (readPotion.Mlow > _targetPotion.z || readPotion.Mhigh < _targetPotion.z)
                continue;

            Debug.Log("Match!\n" + data_values[6]);
            foundMatch = true;

            _displayText.text = "Your Potion's Score was: " + _targetPotion + "\nYou made a " + data_values[6] + " potion!";

            //create a "unique" potion sprite by generating a color (R/G/B btwn 0-1)

            //TODO (Lillianna): see if having this + ie updating _targetPotion values AS THE PLAYER ADD/REMOVES ITEMS is too much of a hassle
            float spriteColorR = (_targetPotion.x + 10) / 20;   //-10, +10 = 0, /20 = 0    //0, +10 = 10, /20 = 0.5    //10, +10 = 20, /20 = 1    //Maps values into 0-1 decimals.
            float spriteColorG = (_targetPotion.y + 10) / 20;
            float spriteColorB = (_targetPotion.z + 10) / 20;
            Color spriteColor = new Color(spriteColorR, spriteColorG, spriteColorB);
            Debug.Log("sprite color: " + spriteColor);
            _rawPotionColor.color = spriteColor;
        }
        _stateController?.ChangeState(5);
    }

    public void Clear()
    {
        Debug.Log("attempting clear");
        //after each craft, set all ingredient slots to Clear
        foreach(craftingSlotController slot in _ingredients)
        {
            slot.ClearIngredients();
        }

        bool destroyedSlots = false;
        foreach(hotbarSlotController slot in _hotSlotsController.GetComponentsInChildren<hotbarSlotController>())
        {
            //NOTE: has some issue with destroying until you hit create potion, which makes sense but could be confusing for players (they'll figure it out tho when they can't add anymore items)
            if(slot.Ingredient.IngredientData.Quantity == 0)
            {
                //first we remove from list to avoid null errors? then destroy
                Destroy(slot.gameObject);

                //if we destroy any set "flag" to destroyed, and have hotbarGroup check its new children.
                destroyedSlots = true;
            }
        }
        if (destroyedSlots)
            _hotSlotsController.CountHotbar();

    }
}
