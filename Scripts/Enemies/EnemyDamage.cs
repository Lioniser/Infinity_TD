
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[SelectionBase]
public class EnemyDamage : MonoBehaviour
{
    public Enemy enemy;

    private float enemyHealthMax;
    private Material enemyMat;

    public GameObject statusBarObj;
    [SerializeField] GameObject healthObj;
    [SerializeField] GameObject frostedObj;

    [SerializeField] ParticleSystem hitParticle;
    [SerializeField] AudioClip hitAudio;

    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem deathParticleCastle;
    [SerializeField] AudioClip deathAudio;

    castle castle;
    UI_Controller UI;
    basicTower tower;

    [SerializeField] TextMesh textDamage;

    private void Awake() 
    {
        UI = FindObjectOfType<UI_Controller>();
        castle = FindObjectOfType<castle>();
        enemy = GetComponent<Enemy>();
    }
    private void Start()
    {
        enemyHealthMax = enemy.life;

        if (enemy.lifeRegen)
        StartCoroutine(ActiveBossRegen());
    }
    private void Update() 
    {
        statusBarObj.transform.LookAt(Camera.main.transform, Vector3.up);
        FrozenIcon();
        HealthBarUpdater();
    }
    private void OnParticleCollision(GameObject other)
    {
        basicTower _tower = other.GetComponentInParent<basicTower>();
        GetHit(_tower.damage, _tower.critChance);
    }
    public void DestroyEnemy(bool isAdd)
    {
        if (isAdd)
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            UI.AddPointForKill(enemy.pointsCost);
            UI.AddCoinForKill(enemy.coinCost);
        }
        else
            Instantiate(deathParticleCastle, transform.position, Quaternion.identity);

        AudioSource.PlayClipAtPoint(deathAudio, Camera.main.transform.position, 0.4f);
        Destroy(gameObject);
    }
    public void GetHit(float towerDamage, float towerCritChance)
    {
        if (CalculateCritical(towerDamage, towerCritChance))
        {
            enemy.life -= towerDamage * 2;
            DamageText(towerDamage*2, Color.red);
        }
        else
        {
            enemy.life -= towerDamage;
            DamageText(towerDamage, Color.yellow);
        }

        hitParticle.Stop();
        hitParticle.Play();
        GetComponent<AudioSource>().PlayOneShot(hitAudio);

        HealthBarUpdater();
        if (enemy.life <= 0)
        {
            DestroyEnemy(true);
        }
    }

    public bool CalculateCritical(float damage, float critChance)
    {
        int critRoll = Mathf.RoundToInt(UnityEngine.Random.Range(1f,100f));
        if (critChance >= critRoll)
        {
            return
            true;
        }
        else
        {
            return
            false;
        }
    } 

    private void DamageText(float damage, Color textColor)
    {
        Vector3 posToCreate = new Vector3(transform.position.x, transform.position.y+12, transform.position.z);
        TextMesh text = Instantiate(textDamage, posToCreate, Quaternion.identity);
        text.color = textColor;
        text.transform.LookAt(Camera.main.transform.position);
        text.text = damage.ToString();
        Destroy(text.gameObject, 0.6f);
    }
    private void HealthBarUpdater()
    {
        float HPpercent = enemy.life / enemyHealthMax;
        healthObj.transform.localScale = new Vector3(HPpercent, 1, 1);
    }
    private IEnumerator ActiveBossRegen()
    {
        while (true)
        {
            if (enemy.life < enemyHealthMax)
            {
                enemy.life += 5;
                DamageText(5, Color.green);
            }
            if (enemy.life > enemyHealthMax)
                enemy.life = enemyHealthMax;
            yield return new WaitForSeconds(3f);
        }
    }
    private void FrozenIcon()
    {
        EnemyMovement frozenIcon = GetComponent<EnemyMovement>();
        if (frozenIcon.isFrozen)
        {
            frostedObj.SetActive(true);
            frostedObj.transform.localScale = new Vector3(2f,2f,2f);
        }
        else
        {
            frostedObj.SetActive(false);
        }
    }

}
