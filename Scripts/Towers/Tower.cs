using UnityEngine;
using System.Collections;
using System;

[SelectionBase]
public class Tower : MonoBehaviour
{
    [SerializeField] Transform turretHead;

    [SerializeField] float shootRange;
    public ParticleSystem bulletParticle;
    public ParticleSystem lvlUPParticle;

    [SerializeField] Transform enemyTarget;
    [SerializeField] Transform Tmenu;

    public TextMesh lvl_txt;
    public TextMesh lvlUP_price_txt;
    public TextMesh dmg_txt;
    public TextMesh spd_txt;


    private float _MenuCD = 5f;
    private bool _MenuON = false;
    public Waypoint baseWaypoint;
    public float Damage = 1f;
    public float CritChance = 3f; //30%
    public int lvl = 1;
    
    private void Start()
    {
        enemyTarget = null;
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
            // print(distanceToEnemy);
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
        dmg_txt.text = "DMG " + Damage;
        spd_txt.text = "SPD " + bulletParticle.emission.rateOverTimeMultiplier;
    }
}
