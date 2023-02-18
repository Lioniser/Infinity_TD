using UnityEngine;
using System.Collections;
using System;
using TMPro;

[SelectionBase]
[RequireComponent(typeof(tower))]

public class basicTower : MonoBehaviour
{
    [SerializeField] Transform turretHead;
    [SerializeField] Transform enemyTarget;

    
    [SerializeField] ParticleSystem placeParticle;
    public ParticleSystem bulletParticle;
    [SerializeField] ParticleSystem.EmissionModule bP_emission;
    public ParticleSystem lvlUPParticle;

    public int lvl = 1;
    private int lvlUpPrice;
    public TextMeshPro lvl_txt;
    [SerializeField] private AudioClip levelUp_SND;
    public TextMesh lvlUP_price_txt;
    
    public tower tower;

    public basicTowerUI characteristics;
    //Базові характеристики
    public float damage;
    public float attackSpeed;
    public float critChance; //30%
    public float shootRange;

    private void Awake() 
    {
        characteristics = FindObjectOfType<basicTowerUI>();
        tower = GetComponent<tower>();
        enemyTarget = null;
    }
    private void Start()
    {
        Instantiate(placeParticle, transform.position, Quaternion.Euler(-90f,0f,0f));
        CalculatelvlUpPrice();
        lvlUP_price_txt.text = "o" + lvlUpPrice;

        bP_emission = bulletParticle.emission;
        bP_emission.rateOverTimeMultiplier = attackSpeed;
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

            CalculateDamage();
            CalculateAttackSpeed();
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
        lvlUpPrice = Mathf.RoundToInt(Mathf.Sqrt(lvl) * lvl * 2 + 2);
        lvlUP_price_txt.text = "o" + lvlUpPrice;
    }

    private void CalculateDamage()
    {
        damage = Mathf.Round(2 * Mathf.Sqrt(lvl) * 10) / 10 - 1f;
    }
    private void CalculateAttackSpeed()
    {
        bP_emission.rateOverTimeMultiplier = 0.8f + Mathf.Round(0.2f * Mathf.Sqrt(lvl) * 10) / 10;
    }
    // private float CritChance()
    // {
    //     return damage = Mathf.Round((Mathf.Sqrt(lvl) * 2)*10)/10;
    // }
    public void Characteristic_text_updater()
    {
        characteristics.dmg_txt.text = "Damage:		" + damage;
        characteristics.spd_txt.text = "Attack speed:	" + bulletParticle.emission.rateOverTimeMultiplier;
        characteristics.rng_txt.text = "Range:			" + shootRange;
    }
    

    
    
}
