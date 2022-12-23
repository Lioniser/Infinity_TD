using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[SelectionBase]
public class Frost_tower : MonoBehaviour
{
    public ParticleSystem lvlUPParticle;
    public ParticleSystem frostEXParticle;

    UI_Controller Global_UI;
    [SerializeField] Transform enemyTarget;
    [SerializeField] Transform Tmenu;

    public TextMesh lvl_txt;
    public TextMesh lvlUP_price_txt;
    public TextMesh dmg_txt;
    public TextMesh spd_txt;
    public TextMesh AOE_txt;
    public TextMesh FrozenTime_txt;
    public TextMesh FrozenMulti_txt;

    private float _MenuCD = 5f;
    private bool _MenuON = false;
    public Waypoint baseWaypoint;
    float _t = 0f;

    public float shootRange;
    public float attackSpeed = 1f;
    public float Damage = 0.1f;
    public float CritChance = 1f; //10%
    public float AreaOfEffect;
    public float FrozenMulti = 1.5f;
    public float FrozenTime = 1.5f;
    public int lvl = 1;
    
    private void Start()
    {
        enemyTarget = null;
        AreaOfEffect = shootRange - 5;
    }
    void Update()
    {
        SetTargetEnemy();
        if (enemyTarget)
        {
            fire();
        }

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
                    NearEnemies[i].GetComponent<EnemyDamage>().getHit(Damage,CritChance);
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
        AOE_txt.text = "AOE " + AreaOfEffect;
        FrozenTime_txt.text = "Ft " + FrozenTime + "s";
        FrozenMulti_txt.text = "Fm " + FrozenMulti;
    }
}
