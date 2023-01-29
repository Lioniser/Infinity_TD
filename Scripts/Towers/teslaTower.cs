using UnityEngine;
using System.Collections.Generic;

[SelectionBase]
[RequireComponent(typeof(tower))]

public class teslaTower : MonoBehaviour
{
    [SerializeField] Transform turretHead;
    [SerializeField] Transform enemyTarget;

    [SerializeField] Light turretHeadLightning;
    [SerializeField] LightningBoltScript LightningAnimation;
    [SerializeField] LightningBoltScript LightningBolt;

    [SerializeField] float shootRange;
    public ParticleSystem lvlUPParticle;

    public int lvl = 1;
    private int lvlUpPrice = 5;
    public TextMesh lvl_txt;
    public TextMesh lvlUP_price_txt;

    public Waypoint baseWaypoint;
    public tower tower;

    public teslaTowerUI characteristics;
    public float Damage = 1f;
    public float CritChance = 5f; //50%
    public float attackSpeed = 1f;
    public int chainNums = 1;

    private void Awake() 
    {
        characteristics = FindObjectOfType<teslaTowerUI>();
        tower = GetComponent<tower>();
        enemyTarget = null;
    }
    private void Start()
    {
        lvlUP_price_txt.text = "o" + lvlUpPrice;
        LightningAnimation.StartObject = gameObject;
        LightningAnimation.EndObject = turretHead.gameObject;
    }
    void Update()
    {
        SetTargetEnemy();
        if (enemyTarget)
        {
            fire();
        }
        else
        shoot(false);
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
                LightningAnimation.gameObject.SetActive(true);
                turretHeadLightning.intensity += Time.deltaTime * 10f * attackSpeed;
                LightningAnimation.ChaosFactor += Time.deltaTime * attackSpeed;

                if (turretHeadLightning.intensity >= 10)
                {
                    shoot(true);
                }
            }
            else
            shoot(false);
        }

    private void shoot(bool isShooted)
    {
        if (isShooted)
        {
            lightningBoltMovementControl(chainNums);
            turretHeadLightning.intensity = 0f;
            LightningAnimation.ChaosFactor = 0f;
        }
        else
        {
            LightningAnimation.gameObject.SetActive(false);
            turretHeadLightning.intensity = 0f;
            LightningAnimation.ChaosFactor = 0f;
        }

    }

    private void lightningBoltMovementControl(int NumberOfChains)
    {
        List<EnemyDamage> sceneEnemies = new List<EnemyDamage>();

        EnemyDamage[] _sceneEnemies = FindObjectsOfType<EnemyDamage>();
        foreach (EnemyDamage _enemy in _sceneEnemies)
        {
            sceneEnemies.Add(_enemy);
        }

        GameObject startObj = turretHead.gameObject;
        GameObject endObj;

        for (int i = 0; i <= NumberOfChains; i++)
            {
                if (sceneEnemies.Count == 0)
                return;

                Transform closestEnemy = sceneEnemies[0].transform;
                foreach (EnemyDamage testedEnemy in sceneEnemies)
                {
                    closestEnemy = GetClosestEnemy(closestEnemy.transform, testedEnemy.transform, startObj);
                }
                endObj = closestEnemy.gameObject;
                sceneEnemies.Remove(closestEnemy.GetComponent<EnemyDamage>());

                float DistanceToEnemy = Vector3.Distance(startObj.transform.position, endObj.transform.position);
                if (DistanceToEnemy < shootRange)
                {
                    generateBolt(startObj, endObj);
                }
                else
                return;

                startObj = endObj;
            }

        startObj = turretHead.gameObject;
        endObj = turretHead.gameObject;
    }
    private void generateBolt(GameObject firstObj, GameObject secondObj)
    {
        secondObj.GetComponent<EnemyDamage>().getHit(Damage, CritChance);

        LightningBoltScript _LightningBolt = Instantiate(LightningBolt, firstObj.transform);
        _LightningBolt.transform.parent = transform;
        _LightningBolt.ChaosFactor = 0.3f;
        _LightningBolt.StartPosition = firstObj.transform.position;
        _LightningBolt.EndPosition = secondObj.transform.position;

        Destroy(_LightningBolt.gameObject, 0.2f);
    } 

    public void Characteristic_text_updater()
    {
        characteristics.dmg_txt.text = "Damage:		    " + Damage;
        characteristics.spd_txt.text = "Attack speed:	" + attackSpeed;
        characteristics.rng_txt.text = "Range:			" + shootRange;
        characteristics.chn_txt.text = "Chains:			" + chainNums;
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

            Damage = Mathf.Round((Mathf.Sqrt(lvl + Damage)) * 10) / 10;

            if (lvl%2 == 0)
            attackSpeed = Mathf.Round(1.1f * attackSpeed * 10) / 10;

            if (lvl%5 == 0)
            chainNums++;

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
        lvlUpPrice = Mathf.RoundToInt(Mathf.Sqrt(lvl - 1) * 5f + lvlUpPrice);
        lvlUP_price_txt.text = "o" + lvlUpPrice;
    }
}
