using UnityEngine;
using System.Collections.Generic;

[SelectionBase]
public class Tesla_tower : MonoBehaviour
{
    [SerializeField] Transform turretHead;
    [SerializeField] Light turretHeadLightning;
    public float attackSpeed = 1f;
    [SerializeField] LightningBoltScript LightningAnimation;
    [SerializeField] LightningBoltScript LightningBolt;

    [SerializeField] float shootRange;
    public ParticleSystem lvlUPParticle;

    [SerializeField] Transform enemyTarget;
    [SerializeField] Transform Tmenu;

    public TextMesh lvl_txt;
    public TextMesh lvlUP_price_txt;
    public TextMesh dmg_txt;
    public TextMesh spd_txt;
    public TextMesh chn_txt;


    private float _MenuCD = 5f;
    private bool _MenuON = false;
    public Waypoint baseWaypoint;
    public float Damage = 1f;
    public float CritChance = 5f; //50%
    public int chainNums = 1;
    public int lvl = 1;
    
    private void Start()
    {
        enemyTarget = null;
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

        UI_Opener();
        UI_Timer();
        UI_text_updater();
    }

    private void OnMouseOver() 
    {
        UI_On();
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

    public void UI_On()
    {
        _MenuCD = 5f;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _MenuON = true;
        }
    }
    private void UI_Timer()
    {
        if (_MenuON)
        {
            _MenuCD -= Time.deltaTime;
            if (_MenuCD <= 0)
            {
                _MenuON = false; 
            }
        }
    }
    private void UI_Opener()
    {
        float _increase = Mathf.Lerp(Tmenu.transform.localScale.x, Tmenu.transform.localScale.x + 3, Time.deltaTime);
        float _decrease = Mathf.Lerp(Tmenu.transform.localScale.x, Tmenu.transform.localScale.x - 4, Time.deltaTime);
        if (_MenuON && Tmenu.transform.localScale.x <= 1)
        Tmenu.transform.localScale = new Vector3 (_increase, _increase, _increase);
        if (!_MenuON && Tmenu.transform.localScale.x >= 0)
        Tmenu.transform.localScale = new Vector3 (_decrease, _decrease, _decrease);
        if (!_MenuON && Tmenu.transform.localScale.x <= 0.1)
        Tmenu.transform.localScale = new Vector3(0,0,0);
    }
    private void UI_text_updater()
    {
        lvl_txt.text = "LVL " + lvl;
        dmg_txt.text = "DMG " + Damage;
        spd_txt.text = "SPD " + attackSpeed;
        chn_txt.text = "CHN " + chainNums;
    }
}
