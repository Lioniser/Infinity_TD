using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

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
    private int lvlUpPrice;
    public TextMeshPro lvl_txt;
    [SerializeField] private AudioClip levelUp_SND;
    public TextMesh lvlUP_price_txt;
    
    public tower tower;
    

    public frostTowerUI characteristics;
    // Базові характеристики
    public float shootRange;
    public float attackSpeed;
    public float damage;
    public float critChance = 10f; //10%
    // Унікальні характеристики для вежі
    public float areaOfEffect;
    public float frozenMulti;
    public float frozenTime;
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
        areaOfEffect = shootRange - 5;
        CalculatelvlUpPrice();
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
                    NearEnemies[i].GetComponent<EnemyDamage>().GetHit(damage,critChance);
                    if (NearEnemies[i].isFrozen)
                    continue;
                    NearEnemies[i].Freeze(frozenMulti, frozenTime);
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
        Vector3 newPOs = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 5, gameObject.transform.position.z);
        ParticleSystem ParticlesFROST = Instantiate(frostEXParticle, newPOs, Quaternion.Euler(-90f,0f,0f));

        ParticleSystem.ShapeModule ExplsSize;
        ExplsSize = ParticlesFROST.shape;
        ExplsSize.radius = areaOfEffect;

        _t = 0;
    }

    public void lvlUp()
    {
        UI_Controller Global_UI = FindObjectOfType<UI_Controller>();
        Transform Tmenu = GetComponent<tower>().Tmenu;
        if (Global_UI.totalCoins >= lvlUpPrice && Tmenu.transform.localScale.x >= 1)
        {
            Instantiate(lvlUPParticle.gameObject, transform.position, Quaternion.Euler(-90f,0f,0f));
            AudioSource.PlayClipAtPoint(levelUp_SND, Camera.main.transform.position, 0.3f);

            lvl++;
            lvl_txt.text = "LVL " + lvl;
            
            Damage();
            if (lvl%10 == 0)
            {
                shootRange += 5;
                areaOfEffect += 5;
            }
            if (lvl%3 == 0)
            {
                FrozenCalculation("multi");
                FrozenCalculation("time");
            }
            Characteristic_text_updater();

            Global_UI.AddCoin(-lvlUpPrice);
            tower.CalculateTotalTowerPrice(lvlUpPrice);
            CalculatelvlUpPrice();
        }
        else if (Global_UI.totalCoins < lvlUpPrice && Tmenu.transform.localScale.x >= 1)
        {
            Global_UI.CoinsErrorMessage("Not enought to level up", Color.red);
        }
    }

    private void CalculatelvlUpPrice()
    {
        lvlUpPrice = Mathf.RoundToInt(Mathf.Sqrt(lvl) * lvl * 3);
        lvlUP_price_txt.text = "o" + lvlUpPrice;
    }

     private float Damage()
    {
        return damage = Mathf.Round(0.1f * Mathf.Sqrt(lvl) * 10) / 10 + 0.1f;
    }
    private float AttackSpeed()
    {
        return attackSpeed = Mathf.Round(0.1f * Mathf.Sqrt(lvl) * 10) / 10 + 0.5f;
    }

    private float FrozenCalculation(string typeOfReturn)
    {
        if (typeOfReturn == "multi")
            return frozenMulti = Mathf.Round(0.4f * Mathf.Sqrt(lvl) * 10) / 10f + 0.5f;
        else if (typeOfReturn == "time")
            return frozenTime = Mathf.Round(0.3f * Mathf.Sqrt(lvl) * 10) / 10f + 1f; 
        else 
            return 0;
    }

    public void Characteristic_text_updater()
    {
        characteristics.dmg_txt.text = "Damage:		" + damage;
        characteristics.spd_txt.text = "Attack speed:	" + attackSpeed;
        characteristics.rng_txt.text = "Range:			" + shootRange;
        characteristics.AOE_txt.text = "Area radius     " + areaOfEffect;
        characteristics.FrozenTime_txt.text = "Frost multi			" + frozenMulti;
        characteristics.FrozenMulti_txt.text = "Frost time (sec)	" + frozenTime;
    }
    
}
