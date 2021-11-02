using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private User user;
    public User CurrentUser { get { return user; } }
    public UIManager UIManager { get; private set; }
    public QuestManager QuestManager { get; private set; }

    private string SAVE_PATH = "";
    private readonly string SAVE_FILENAME = "/SaveFile.txt";

    public Transform doorPosition;
    public Transform counterPosition;
    public Ingredient currentIngredient { get; private set; }
    private List<Ingredient> currentRamen = new List<Ingredient>();
    private Recipe currentRecipe;

    public Sprite[] ingredientContainerSprites;
    public Sprite[] ingredientSprites;

    public Camera mainCam { get; private set; }

    public Transform Pool;

    public IngredientIcon currentIngredientIcon;
    public List<IngredientIcon> ingredientIcons = new List<IngredientIcon>();

    #region 데이터 저장
    private void FirstData()
    {
        SAVE_PATH = Application.dataPath + "/Save";
        //SAVE_PATH = Application.dataPath + "/Save";
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

    private void SaveToJson()
    {
        SAVE_PATH = Application.dataPath + "/Save";

        if (user == null) return;
        string json = JsonUtility.ToJson(user, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILENAME, json, System.Text.Encoding.UTF8);
    }
    #endregion

    private void Awake()
    {
        FirstData();
        UIManager = GetComponent<UIManager>();
        QuestManager = GetComponent<QuestManager>();
        mainCam = Camera.main;
    }

    void Start()
    {
        for (int i = 0; i < user.recipes.Count; i++)
        {
            user.recipes[i].SetList();
        }

        for (int i = 0; i < user.ingredients.Count; i++)
        {
            user.ingredients[i].SetIndex(i);
        }

        user.CheckCurrentQuest();
    }

    private void Update()
    {
        //#if DEVELOPMENT_BUILD
        CheatKey();
        //#endif
    }

    private void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Ingredient igd in user.ingredients)
            {
                igd.amount++;
            }

            UIManager.UpdateIngredientPanel();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach(Quest quest in user.questList)
            {
                quest.AddCurrentValue(1);
            }
        }
    }

    public void SetCurrentIngredient(Ingredient ingredient)
    {
        currentIngredient = ingredient;
    }

    public Sprite GetIngredientContainer(int index)
    {
        return ingredientContainerSprites[index];
    }

    public Sprite GetIngredientSprite(int index)
    {
        return ingredientSprites[index];
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

    public void AddCurRanem()
    {
        currentRamen.Add(currentIngredient);
    }

    public float EvaluateRamen(string recipeName)
    {
        Recipe recipe = user.recipes.Find(x => x.recipeName == recipeName);

        List<int> checkList = new List<int>();

        for (int i = 0; i < currentRamen.Count; i++)
        {
            for (int j = 0; j < recipe.GetIngredients().Length; j++)
            {
                if (checkList.Exists(x => x == j)) continue;

                if (currentRamen[i].name == recipe.GetIngredients()[j])
                {
                    checkList.Add(j);
                }
            }
        }

        if (!currentRamen.Exists(x => x.name == "물") || !currentRamen.Exists(x => x.name == "라면사리") || !currentRamen.Exists(x => x.name == "스프"))
        {
            return -1f;
        }

        else if (checkList.Count == currentRamen.Count && checkList.Count == recipe.GetIngredients().Length)
        {
            return 100f;
        }

        else if (Mathf.Abs(currentRamen.Count - recipe.GetIngredients().Length) < 3)
        {
            return 60f;
        }

        else
        {
            return 30f;
        }
    }

    public void SetCurrentRecipe(Recipe recipe)
    {
        currentRecipe = recipe;
    }

    public Recipe GetRecipe()
    {
        return currentRecipe;
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }

    public TimeSpan ReturnNowTimeSpan()
    {
        DateTime now = DateTime.Now;
        TimeSpan span = (now - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime());

        return span;
    }
}