using UnityEngine;
using System.Collections;
using System;

[SelectionBase]
[RequireComponent(typeof(tower))]

public class basicTower : MonoBehaviour
{
    [SerializeField] Transform turretHead;
    [SerializeField] Transform enemyTarget;

    [SerializeField] float shootRange;
    public ParticleSystem bulletParticle;
    public ParticleSystem lvlUPParticle;

    public int lvl = 1;
    private int lvlUpPrice = 3;
    public TextMesh lvl_txt;
    public TextMesh lvlUP_price_txt;
    
    public Waypoint baseWaypoint;
    public tower tower;

    public basicTowerUI characteristics;
    //Базові характеристики
    public float Damage = 1f;
    public float CritChance = 3f; //30%

    private void Awake() 
    {
        characteristics = FindObjectOfType<basicTowerUI>();
        tower = GetComponent<tower>();
        enemyTarget = null;
    }
    private void Start()
    {
        lvlUP_price_txt.text = "o" + lvlUpPrice;
    }
    void Update()
    {
        SetTargetEnemy();
        if (enemyTarget)
        {
            fire();
        }
        else
        {
            shoot(false); 
            turretHead.transform.rotation = new Quaternion(0f,0f,0f,0f);
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
            closestEnemy = GetClosestEnemy(closestEnemy.transform, test.transform);
        }
        enemyTarget = closestEnemy;
    }
    private Transform GetClosestEnemy(Transform enemyA, Transform enemyB)
    {
        var distA = Vector3.Distance(enemyA.position, transform.position);
        var distB = Vector3.Distance(enemyB.position, transform.position);
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
            turretHead.LookAt(enemyTarget.transform);
            shoot(true);
            }
            else
            {
            shoot(false);
            turretHead.transform.rotation = new Quaternion(0f,0f,0f,0f);
            }
        }
    private void shoot(bool isActive)
    {
        var emission = bulletParticle.emission;
        emission.enabled = isActive;
    }

    public void Characteristic_text_updater()
    {
        characteristics.dmg_txt.text = "Damage:		" + Damage;
        characteristics.spd_txt.text = "Attack speed:	" + bulletParticle.emission.rateOverTimeMultiplier;
        characteristics.rng_txt.text = "Range:			" + shootRange;
    }

    public void lvlUp()
    {
        UI_Controller Global_UI = FindObjectOfType<UI_Controller>();
        Transform Tmenu = GetComponent<tower>().Tmenu;
        if (Input.GetKeyDown(KeyCode.Mouse0) && Global_UI.totalCoins >= lvlUpPrice && Tmenu.transform.localScale.x >= 1)
        {
            Instantiate(lvlUPParticle.gameObject, transform.position, Quaternion.Euler(-90f,0f,0f));
            lvl++;
            lvl_txt.text = "LVL " + lvl;

            Damage = Mathf.Round((Mathf.Sqrt(lvl + Damage))*10)/10;

            ParticleSystem.EmissionModule towerSpeed = bulletParticle.emission;
            towerSpeed.rateOverTimeMultiplier = Mathf.Round((Mathf.Sqrt(lvl * 0.3f + bulletParticle.emission.rateOverTimeMultiplier))*10)/10;

            Characteristic_text_updater();
            Global_UI.AddCoin(-lvlUpPrice);
            CalculatelvlUpPrice();
        }
        if (Global_UI.totalCoins < lvlUpPrice && Tmenu.transform.localScale.x >= 1)
        {
            Global_UI.coins_info.text = "Not Enought Coins";
            Global_UI.coins_info.color = Color.red;
            Global_UI.isEnoughCoins = false;
        }
    }

    private void CalculatelvlUpPrice()
    {
        lvlUpPrice = Mathf.RoundToInt(Mathf.Sqrt(lvl - 1) * 1.5f + lvlUpPrice);
        lvlUP_price_txt.text = "o" + lvlUpPrice;
    }
}
