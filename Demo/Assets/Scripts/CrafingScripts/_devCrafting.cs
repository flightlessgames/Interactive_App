using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

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

        //all values are from reading .csv, no harm in corrupting temporary Potion class values
        public int Slow, Shigh, Plow, Phigh, Mlow, Mhigh;
        public string Name;
    }

    public class Recipe :MonoBehaviour
    {
        //public for JsomUtility... not happy with this
        public string recipeName;
        public Vector3 color;
        public List<string> input;
        public Vector3 value;

        //from _decraftingInput to serializable information
        public Recipe(string n, Color c, List<Ingredients_sObj> i)
        {
            recipeName = n;
            color = new Vector3(c.r, c.g, c.b);
            input = new List<string>();
            value = Vector3.zero;

            foreach (Ingredients_sObj obj in i)
            {
                input.Add(obj.Name);
                value += obj.Values;
            }
        }
    }
    [SerializeField] private Text _displayText = null;
    [SerializeField] private RawImage _rawPotionColor = null;

    [SerializeField] private StateController _stateController = null;

    [SerializeField] private hotbarGroupController _hotSlotsController = null;
    [SerializeField] private List<craftingSlotController> _craftingSlots = new List<craftingSlotController>();

    private Vector3 _targetPotion = Vector3.zero;
    private List<Ingredients_sObj> _inputIngredients = new List<Ingredients_sObj>();

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
        _inputIngredients.Clear();

        foreach (craftingSlotController slot in _craftingSlots)
        {
            _targetPotion += slot.ScoreIngredient(); //during each craft, we're adding vector values from ingredient scores instead of "creating a new vector of combined scores"
            _inputIngredients.Add(slot.Ingredient);
        }

        ReadCSVFile();

        fileUtility.SaveObject.gold += 5;
    }

    void ReadCSVFile()  //now that we have a "targetpotion" we can compare that to our Potion.csv to read the recipe.
    {
        StreamReader strReader = new StreamReader(fileUtility.POTIONS_LOCATION);

        //code adapted from RapidGaming on YouTube: https://www.youtube.com/watch?v=xwnL4meq-j8&feature=youtu.be
        bool endOfFile = false;
        bool foundMatch = false;    //both endOfFile and foundMatch are our 2 cases to end reading

        while (!endOfFile && !foundMatch)
        {
            string data_String = strReader.ReadLine();  //.ReadLine converts a row of data into a string.

            if (data_String == null) //if a row is null then we have reached the end of our dataset //look into empty row cases, or skipped row cases to ensure we don't bug our data-entry.
            {
                endOfFile = true;
                break;
            }

            var data_values = data_String.Split(',');

            //might need to REDO or overhaul when moving onto PlayerJournal scripts. Might convert all entries into .sObjs to store player entry data.
            //feels redundant, look into consolidating into a function
            Potion readPotion = new Potion();

            //sets readPotion values based on if the strReader can parse an int from its strings.
            bool Slow = int.TryParse(data_values[0], out readPotion.Slow);
            bool Shigh = int.TryParse(data_values[1], out readPotion.Shigh);
            bool Plow = int.TryParse(data_values[2], out readPotion.Plow);
            bool Phigh = int.TryParse(data_values[3], out readPotion.Phigh);
            bool Mlow = int.TryParse(data_values[4], out readPotion.Mlow);
            bool Mhigh = int.TryParse(data_values[5], out readPotion.Mhigh);

            //feels redundant, again...
            if (readPotion.Slow > _targetPotion.x || readPotion.Shigh < _targetPotion.x)
                continue;

            if (readPotion.Plow > _targetPotion.y || readPotion.Phigh < _targetPotion.y)
                continue;

            if (readPotion.Mlow > _targetPotion.z || readPotion.Mhigh < _targetPotion.z)
                continue;

            readPotion.Name = data_values[6];

            Debug.Log("Match!\n" + readPotion.Name);
            foundMatch = true;

            _displayText.text = "Your Potion's Score was: " + _targetPotion + "\nYou made a " + readPotion.Name + " potion!";

            //create a "unique" potion sprite by generating a color (R/G/B btwn 0-1)

            //TODO (Lillianna): see if having this + ie updating _targetPotion values AS THE PLAYER ADD/REMOVES ITEMS is too much of a hassle
            float spriteColorR = (_targetPotion.x + 10) / 20;   //-10, +10 = 0, /20 = 0    //0, +10 = 10, /20 = 0.5    //10, +10 = 20, /20 = 1    //Maps values into 0-1 decimals.
            float spriteColorG = (_targetPotion.y + 10) / 20;
            float spriteColorB = (_targetPotion.z + 10) / 20;

            Color spriteColor = new Color(spriteColorR, spriteColorG, spriteColorB);
            _rawPotionColor.color = spriteColor;

            //for SaveFile and Achievements Page, updates the known recipes to this
            Recipe validRecipe = new Recipe(readPotion.Name, spriteColor, _inputIngredients);
            fileUtility.SaveObject.AddRecipe(validRecipe);
        }

        _stateController?.ChangeState((int)CraftingState.PotionResult);
    }

    public void Clear()
    {
        //after each craft, set all ingredient slots to Clear
        foreach(craftingSlotController slot in _craftingSlots)
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

            slot.GetComponent<displayIngredient>().AdjustQuanttiy();
        }
        if (destroyedSlots)
            _hotSlotsController.CountHotbar();

    }
}
