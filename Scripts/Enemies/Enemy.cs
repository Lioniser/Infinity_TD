using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public string type;
    public string modification;
    private EnemySpawner spawner;
    public int rarity;
    [SerializeField] Renderer enemyBaseRend;
    [SerializeField] Material[] materials;
    public float life;
    public bool lifeRegen = false;
    public float speed;
    public int coinCost;
    public int pointsCost;
    private void Awake() 
    {
        spawner = FindObjectOfType<EnemySpawner>();
    }
    private void ChooseEnemyType(string _modification)
    {
        modification = _modification;
        if (type == "Monster")
        {
            if (_modification == "Basic")
            {
                // rarity = 75;
                life = 10;
                speed = 1;
                coinCost = 1;
                pointsCost = 1;
            }
            else if (_modification == "Strong")
            {
                // rarity = 14;
                enemyBaseRend.material = materials[0];
                life = 25;
                speed = 1.1f;
                coinCost = 2;
                pointsCost = 2;

            }
            else if (_modification == "Fast")
            {
                // rarity = 8;
                enemyBaseRend.material = materials[1];
                life = 10;
                speed = 0.5f;
                coinCost = 3;
                pointsCost = 3;
            }
            else if (_modification == "Gold")
            {
                // rarity = 3;   
                enemyBaseRend.material = materials[2];
                life = 15;
                speed = 1f;
                coinCost = 5;
                pointsCost = 5;
            }
        }
        else if (type == "Boss")
        {
            if (_modification == "Basic")
            {
                // rarity = 70;
                life = 50;
                speed = 1.5f;
                coinCost = 7;
                pointsCost = 5;
            }
            else if (_modification == "Strong")
            {
                // rarity = 25;
                enemyBaseRend.material = materials[1];
                life = 120;
                speed = 1.8f;
                coinCost = 12;
                pointsCost = 8;
            }
            else if (_modification == "Regenerative")
            {
                // rarity = 5;
                lifeRegen = true;
                enemyBaseRend.material = materials[0];
                life = 50;
                speed = 1.3f;
                coinCost = 15;
                pointsCost = 10;
            }
        }
    }
    public void RollMonster(string _type, int _wave)
    {
        type = _type;

        if (type == "Monster")
        {
            int rarity = Mathf.RoundToInt(Random.Range(0f,100f));
            if (rarity <= 97 && rarity >= 0)
            rarity += spawner.spawnProbability * 10;

            if (rarity > 100)
            rarity = Mathf.RoundToInt(Random.Range(76f, 97f));

            if (rarity <= 75 || _wave <= 2)
                ChooseEnemyType("Basic");
            else if (rarity > 75 && rarity <= 89 && _wave > 2)
                ChooseEnemyType("Strong");
            else if (rarity > 89 && rarity <= 97 && _wave > 3)
                ChooseEnemyType("Fast");
            else if (rarity > 97 && rarity <= 100)
                ChooseEnemyType("Gold");
        }
        else if (type == "Boss")
        {
            int rarity = Mathf.RoundToInt(Random.Range(0f,100f));
            rarity += spawner.spawnProbability * 5;

            if (rarity > 100)
            rarity = Mathf.RoundToInt(Random.Range(71f, 100f));

            if (rarity <= 70)
                ChooseEnemyType("Basic");
            else if (rarity > 70 && rarity <= 95)
                ChooseEnemyType("Strong");
            else if (rarity > 95 && rarity <= 100)
                ChooseEnemyType("Regenerative");
        }
    }
}
