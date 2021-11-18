[System.Serializable]
public class Livestock
{
    public string livestockName;
    public int amount;
    public int maxIngredientCount;
    private Ingredient ingredient;

    public Ingredient GetIngredient()
    {
        return ingredient;
    }

    public void SetIngredient(Ingredient ingredient)
    {
        this.ingredient = ingredient;
    }
}
