using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    #region Data
    private string SAVE_PATH = "";
    private readonly string SAVE_FILENAME = "/SaveFile.txt";
    [SerializeField] private User user;
    public User CurrentUser { get { return user; } }

    [SerializeField] TextAsset recipeNameText;
    [SerializeField] TextAsset recipeIngredientText;
    [SerializeField] TextAsset ingredientInfoText;

    #endregion
    #region Controller
    public UIManager UIManager { get; private set; }
    public QuestManager QuestManager { get; private set; }
    public TutorialManager TutorialManager { get; private set; }
    public AdManager AdManager { get; private set; }
    public GuestMove GuestMove;
    public Transform Pool;

    #endregion
    #region Sprite
    private Sprite[] ingredientContainerSprites;
    private Sprite[] ingredientSprites;
    private Sprite[] ingredientInPotSprites;
    private Sprite[] guestSprites;
    private Sprite[] recipeSprites;
    #endregion

    public Transform doorPosition;
    public Transform counterPosition;

    public string[] calculatedFactors;

    #region Cook
    [SerializeField] private List<Recipe> recipes = new List<Recipe>();
    [SerializeField] private List<Ingredient> ingredients = new List<Ingredient>();

    private List<Ingredient> currentRamen = new List<Ingredient>();
    private Recipe currentRecipe;

    private IngredientIcon currentIngredientIcon;
    private List<IngredientIcon> ingredientIcons = new List<IngredientIcon>();

    private List<int> userRecipeIndexes = new List<int>();
    #endregion

    public Camera mainCam { get; private set; }

    private Pot pot;
    private int[] datasOfDay = new int[(int)DataOfDay.Count];


    #region 데이터 저장
    private void FirstData()
    {
        //SAVE_PATH = Application.dataPath + "/Save";
        SAVE_PATH = Application.persistentDataPath + "/Save";
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        LoadFromJson();
    }

    private void LoadFromJson()
    {
        string json;

        if (File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            user = JsonUtility.FromJson<User>(json);
        }

        else
        {
            SaveToJson();
            LoadFromJson();

            user.SetUserTimeSpan(ReturnNowTimeSpan());
        }
    }

    public void SaveToJson()
    {
        SAVE_PATH = Application.persistentDataPath + "/Save";

        if (user == null) return;
        string json = JsonUtility.ToJson(user, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILENAME, json, System.Text.Encoding.UTF8);
    }
    #endregion


    private void Awake()
    {
        FirstData();
        InputRecipeData();
        InputIngredientData();

        GetControllerComponent();
        SearchComponent();
        ImportSprites();

        mainCam = Camera.main;
    }
    private void ImportSprites()
    {
        ingredientSprites = Resources.LoadAll<Sprite>("IngredientAsset");
        ingredientInPotSprites = Resources.LoadAll<Sprite>("IngredientInPotAsset");
        ingredientContainerSprites = Resources.LoadAll<Sprite>("IngredientContainerAsset");
        recipeSprites = Resources.LoadAll<Sprite>("Recipes_Sheet");
        guestSprites = Resources.LoadAll<Sprite>("Guests");

    }
    private void SearchComponent()
    {
        GuestMove = FindObjectOfType<GuestMove>();
        TutorialManager = FindObjectOfType<TutorialManager>();
        pot = FindObjectOfType<Pot>();

    }
    private void GetControllerComponent()
    {
        UIManager = GetComponent<UIManager>();
        QuestManager = GetComponent<QuestManager>();
        AdManager = GetComponent<AdManager>();

    }
    void Start()
    {
        SetUserIndex();
    }

    private void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Ingredient igd in ingredients)
            {
                igd.AddAmount(1);
            }

            UIManager.UpdateIngredientPanel();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (Quest quest in user.questList)
            {
                quest.AddCurrentValue(1);
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            user.AddUserMoney(10000);
            UIManager.UpdateMoneyText();
        }
    }

    public bool CheckPool(string objName)
    {
        for (int i = 0; i < Pool.childCount; i++)
        {
            if (Pool.childCount > 0 && Pool.GetChild(i).name.Contains(objName))
            {
                return true;
            }
        }
        return false;
    }

    public GameObject ReturnPoolObject(string objName)
    {
        for (int i = 0; i < Pool.childCount; i++)
        {
            if (Pool.childCount > 0 && Pool.GetChild(i).name.Contains(objName))
            {
                return Pool.GetChild(i).gameObject;
            }
        }
        return null;
    }

    public void AddCurRanem(Ingredient ingredient)
    {
        currentRamen.Add(ingredient);
    }

    public float EvaluateRamen()
    {
        List<int> checkList = new List<int>();

        for (int i = 0; i < currentRamen.Count; i++)
        {
            for (int j = 0; j < currentRecipe.GetIngredients().Count; j++)
            {
                if (checkList.Exists(x => x == j)) continue;

                if (currentRecipe.GetIngredients()[j].Contains(currentRamen[i].name))
                {
                    checkList.Add(j);
                    break;
                }
            }
        }

        if (checkList.Count == currentRamen.Count && checkList.Count == currentRecipe.GetIngredients().Count)
        {
            return 100f;
        }

        else if (Mathf.Abs(currentRamen.Count - currentRecipe.GetIngredients().Count) < 3)
        {
            return 60f;
        }

        else
        {
            return 30f;
        }
    }

    public bool IsInCurrentRamen(params string[] ingredients)
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            Ingredient igd = currentRamen.Find(x => x.name == ingredients[i]);

            if (igd == null)
            {
                return false;
            }
        }

        return true;
    }

    private void InputRecipeData()
    {
        string[] names = recipeNameText.ToString().Split('\n');
        string[] ingredients = recipeIngredientText.ToString().Split('\n');

        for (int i = 0; i < names.Length; i++)
        {
            recipes.Add(new Recipe(names[i], ingredients[i]));
        }
    }

    private void InputIngredientData()
    {
        string[] infos = ingredientInfoText.ToString().Split('\n', '\t');

        for (int i = 0, cnt = 0; i < infos.Length; i += 7, cnt++)
        {
            ingredients[cnt].SetIndex(cnt);

            ingredients[cnt].SetInfo
                (infos[i],
                infos[i + 1],
                int.Parse(infos[i + 2]),
                int.Parse(infos[i + 3]),
                float.Parse(infos[i + 4]),
                int.Parse(infos[i + 5]),
                float.Parse(infos[i + 6]));
        }
    }


    public void SetUserIndex()
    {
        userRecipeIndexes.Clear();
        int cnt = 0;
        int ingredientIndex;

        for (int i = 0; i < recipes.Count; i++)
        {
            for (int j = 0; j < recipes[i].GetIngredients().Count; j++)
            {
                ingredientIndex = ingredients.Find(x => recipes[i].GetIngredients()[j].Contains(x.name)).GetIndex();

                if (!user.GetIsIngredientsHave()[ingredientIndex])
                {
                    cnt = 0;
                    break;
                }

                else
                {
                    cnt++;

                    if (cnt == recipes[i].GetIngredients().Count)
                    {
                        userRecipeIndexes.Add(i);
                        cnt = 0;
                    }
                }
            }
        }
    }
    public void PlusIngredientInPot(Ingredient ingredient)
    {
        pot.OnIngredientPut(ingredient);
        AddCurRanem(ingredient);
    }

    public bool IsUserRecipe(int index)
    {
        return (userRecipeIndexes.Contains(index));
    }

    #region GetSet
    public int GetRandomRecipeIndex()
    {
        return userRecipeIndexes[Random.Range(0, userRecipeIndexes.Count)];
    }

    public TimeSpan ReturnNowTimeSpan()
    {
        TimeSpan span = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime());
        return span;
    }

    public List<Recipe> GetRecipes()
    {
        return recipes;
    }

    public Pot GetPot()
    {
        return pot;
    }

    public void SetCurrentRecipe(Recipe recipe)
    {
        currentRecipe = recipe;
    }

    public Recipe GetRecipe()
    {
        return currentRecipe;
    }

    public List<Ingredient> GetIngredients()
    {
        return ingredients;
    }

    public void ClearCurrentRamen()
    {
        currentRamen.Clear();
    }

    public Sprite GetIngredientSprite(int index)
    {
        return ingredientSprites[index];
    }

    public Sprite GetingredientInPotSprite(int index)
    {
        return ingredientInPotSprites[index];
    }

    public Sprite GetingredientContainerSprite(int index)
    {
        return ingredientContainerSprites[index];
    }

    public Sprite GetRecipeSprite(int index)
    {
        return recipeSprites[index];
    }

    public IngredientIcon GetCurrentIngredientIcon()
    {
        return currentIngredientIcon;
    }

    public void SetCurrentIngredientIcon(IngredientIcon icon)
    {
        currentIngredientIcon = icon;
    }

    public List<IngredientIcon> GetIngredientIcons()
    {
        return ingredientIcons;
    }

    public List<Ingredient> GetCurrentRamen()
    {
        return currentRamen;
    }

    public Sprite GetRandomGuestSprite()
    {
        return guestSprites[Random.Range(0, guestSprites.Length)];
    }
    #endregion

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void AddDatasOfDay(DataOfDay type, int value)
    {
        datasOfDay[(int)type] += value;
    }

    public int GetDataofDay(int index)
    {
        return datasOfDay[index];
    }
}