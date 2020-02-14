using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;

public class _devCrafting : MonoBehaviour
{
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
    [SerializeField] private StateController _stateController = null;
    [SerializeField] private craftingSlotController[] _ingredients = new craftingSlotController[3];
    private Vector3 _targetPotion = Vector3.zero;

    [SerializeField] Text _debugText = null;
    private string _potionCsvPath = "...";
    UnityWebRequest _potionPath;

    private void Start()
    {
        
        //for Android "Handheld" we need to use a URL path
        Debug.Log("We're on Android");
        _potionCsvPath = Application.streamingAssetsPath + "!/assets/Potions.csv";
        _potionPath = new UnityWebRequest(_potionCsvPath);
        Debug.Log(_potionPath.url);
        _debugText.text = _potionPath.url;

        //for windows, assume we're in editor, use the folder directory
        Debug.Log("We're on Desktop");
        _potionCsvPath = Application.streamingAssetsPath + "/Potions.csv";
        Debug.Log(_potionCsvPath);
        

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

            //can decrease Quantity, but is not checking quantity before hand. 
            //TODO Check Quantity during drag+drop onto crafting zone.
            //ingredients.IngredientData.DecreaseQuantity(1);
        }

        ReadCSVFile();
    }

    void ReadCSVFile()  //now that we have a "targetpotion" we can compare that to our Potion.csv to read the recipe.
    {
        Debug.Log("Attempting a read");
        _debugText.text = "Attempting a read";
        StreamReader strReader = null;


        if(SystemInfo.deviceType == DeviceType.Desktop)
        {
             strReader = new StreamReader(_potionCsvPath);
        }
        else if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            TextAsset txtPotions = Resources.Load<TextAsset>("Potions.csv");
            if (txtPotions != null)
            {
                strReader = new StreamReader(new MemoryStream(txtPotions.bytes));
            }
        }

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

            #region compareScore v1 for Posterity
            /*int[] parsedIntValues = new int[3];
            for (int i = 0; i < 3; i++)
            {
                int parsedInt;
                bool attempt = int.TryParse(data_values[i], out parsedInt);

                if (attempt)
                    parsedIntValues[i] = parsedInt;
                else
                    Debug.Log("Found a Bad Potion Value");
                
            }

            Vector3 compareScore = new Vector3(parsedIntValues[0], parsedIntValues[1], parsedIntValues[2]);
            Debug.Log("compareScore " + compareScore);

            if (compareScore != _targetPotion)   //instead of nesting the rest of the code in an if{} we can use a !if{}
            {
                continue;
            */
            #endregion

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
            _debugText.text = "Made: " + data_values[6];
        }
        _stateController?.ChangeState(5);
    }

    public void Clear()
    {
        //after each craft, set all ingredient slots to Clear
        foreach(craftingSlotController slot in _ingredients)
        {
            slot.ClearIngredients();
        }
    }
}
