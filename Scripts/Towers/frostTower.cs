using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SelectionBase]
[RequireComponent(typeof(tower))]

public class frostTower : MonoBehaviour
{
    [SerializeField] ParticleSystem placeParticle;
    public ParticleSystem lvlUPParticle;
    public ParticleSystem frostEXParticle;

    UI_Controller Global_UI;
    [SerializeField] Transform enemyTarget;

    public int lvl = 1;
    private int lvlUpPrice = 4;
    public TextMesh lvl_txt;
    public TextMesh lvlUP_price_txt;
    
    public tower tower;
    public Waypoint baseWaypoint;
    

    public frostTowerUI characteristics;
    // Базові характеристики
    public float shootRange;
    public float attackSpeed = 1f;
    public float Damage = 0.1f;
    public float CritChance = 10f; //10%
    // Унікальні характеристики для вежі
    public float AreaOfEffect;
    public float FrozenMulti = 1.5f;
    public float FrozenTime = 1.5f;
    float _t = 0f;
    private void Awake() 
    {
        characteristics = FindObjectOfType<frostTowerUI>();
        tower = GetComponent<tower>();
        enemyTarget = null;
    }
    private void Start()
    {
        Instantiate(placeParticle, transform.position, Quaternion.Euler(-90f,0f,0f));
        AreaOfEffect = shootRange - 5;
        lvlUP_price_txt.text = "o" + lvlUpPrice;
    }
    private void Update()
    {
        SetTargetEnemy();
        if (enemyTarget)
        {
            fire();
        }
    }
    private void SetTargetEnemy()
    {
        EnemyDamage[] sceneEnemies = FindObjectsOfType<EnemyDamage>();
        if (sceneEnemies.Length == 0) 
        return;
        Transform closestEnemy = sceneEnemies[0].transform;
        foreach (EnemyDamage test in sceneEnemies)
        {
            closestEnemy = GetClosestEnemy(closestEnemy.transform, test.transform, gameObject);
        }
        enemyTarget = closestEnemy;
    }

    private Transform GetClosestEnemy(Transform enemyA, Transform enemyB, GameObject ObjectFrom)
    {
        var distA = Vector3.Distance(enemyA.position, ObjectFrom.transform.position);
        var distB = Vector3.Distance(enemyB.position, ObjectFrom.transform.position);
        if (distA < distB) 
        return enemyA;
        else
        return enemyB;
    }

    private void fire()
        {
            float distanceToEnemy = Vector3.Distance(enemyTarget.transform.position, transform.position);
            if (distanceToEnemy <= shootRange)
            {
                shoot();
            }
        }

    private void shoot()
    {
            List<EnemyMovement> NearEnemies = FindAllNearEnemies();

            _t += Time.deltaTime;
            if (_t >= 1/attackSpeed)
            {
                frostExplosion();
            
                for (int i = 0; i < NearEnemies.Count; i++)
                {
                    NearEnemies[i].GetComponent<EnemyDamage>().GetHit(Damage,CritChance);
                    if (NearEnemies[i].isFrozen)
                    continue;
                    NearEnemies[i].Freeze(FrozenMulti, FrozenTime);
                }
            }
    }

    private List<EnemyMovement> FindAllNearEnemies()
    {
        EnemyMovement[] Enemies = FindObjectsOfType<EnemyMovement>();
        List<EnemyMovement> nearEnemies = new List<EnemyMovement>();

        foreach (EnemyMovement enemy in Enemies)
        {
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
            if (distanceToEnemy <= shootRange)
            {
                nearEnemies.Add(enemy);
            }
        }

        return nearEnemies;
    }
    private void frostExplosion()
    {
        Vector3 newPOs = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10, gameObject.transform.position.z);
        ParticleSystem ParticlesFROST = Instantiate(frostEXParticle, newPOs, Quaternion.Euler(-90f,0f,0f));

        ParticleSystem.ShapeModule ExplsSize;
        ExplsSize = ParticlesFROST.shape;
        ExplsSize.radius = AreaOfEffect;

        _t = 0;
    }

    public void Characteristic_text_updater()
    {
        characteristics.dmg_txt.text = "Damage:		" + Damage;
        characteristics.spd_txt.text = "Attack speed:	" + attackSpeed;
        characteristics.rng_txt.text = "Range:			" + shootRange;
        characteristics.AOE_txt.text = "Area radius     " + AreaOfEffect;
        characteristics.FrozenTime_txt.text = "Frost multi			" + FrozenTime;
        characteristics.FrozenMulti_txt.text = "Frost time (sec)	" + FrozenMulti;
    }

    public void lvlUp()
    {
        UI_Controller Global_UI = FindObjectOfType<UI_Controller>();
        Transform Tmenu = GetComponent<tower>().Tmenu;
        if (Global_UI.totalCoins >= lvlUpPrice && Tmenu.transform.localScale.x >= 1)
        {
            Instantiate(lvlUPParticle.gameObject, transform.position, Quaternion.Euler(-90f,0f,0f));

            lvl++;
            lvl_txt.text = "LVL " + lvl;

            Damage = Mathf.Round((Mathf.Sqrt(lvl * 0.1f + Damage)) * 10) / 10;

            if (lvl%5 == 0)
            attackSpeed = Mathf.Round(1.1f * attackSpeed * 10) / 10;

            if (lvl%10 == 0)
            {
            shootRange += 5;
            AreaOfEffect += 5;
            }

            if (lvl%3 == 0)
            {
            FrozenMulti = Mathf.Round((FrozenMulti * 1.3f) * 10) / 10f;    
            FrozenTime = Mathf.Round((FrozenTime * 1.1f) * 10) / 10f;  
            }

            Characteristic_text_updater();
            Global_UI.AddCoin(-lvlUpPrice);
            CalculatelvlUpPrice();
        }
        else if (Global_UI.totalCoins < lvlUpPrice && Tmenu.transform.localScale.x >= 1)
        {
            Global_UI.CoinsErrorMessage("Not Enough Coins", Color.red);
        }
    }

    private void CalculatelvlUpPrice()
    {
        lvlUpPrice = Mathf.RoundToInt(Mathf.Sqrt(lvl - 1) * 10f + lvlUpPrice);
        lvlUP_price_txt.text = "o" + lvlUpPrice;
    }
}
