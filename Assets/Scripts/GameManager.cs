using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private User user;
    public User CurrentUser { get { return user; } }

    private string SAVE_PATH = "";
    private readonly string SAVE_FILENAME = "/SaveFile.txt";

    public Transform doorPosition;
    public Transform counterPosition;

    public UIManager UIManager { get; private set; }

    public Ingredient currentIngredient { get; private set; }

    public Sprite[] ingredientContainerSprites;
    public Sprite[] ingredientSprites;

    public Camera mainCam { get; private set; }

    public Transform Pool;


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
        //FirstData();
        UIManager = GetComponent<UIManager>();
        mainCam = Camera.main;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetCurrentIngredient(Ingredient ingredient)
    {
        currentIngredient = ingredient;
    }

    public Sprite GetIngredientContainer(int index)
    {
        return ingredientContainerSprites[index];
    }
}
