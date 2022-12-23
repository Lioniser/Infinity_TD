
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[SelectionBase]
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] float enemyHealth;
    float enemyHealthMax;
    [SerializeField] GameObject StatusBar;
    [SerializeField] GameObject Health;
    [SerializeField] GameObject Frost;

    [SerializeField] ParticleSystem hitParticle;
    [SerializeField] AudioClip HitDamageAudioFX;

    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] AudioClip DeathEnemyAudioFX;

    [SerializeField] ParticleSystem castleDamageParticle;
    castle castle;
    UI_Controller UI;
    Tower tower;

    [SerializeField] GameObject textDamage;

    private void Start()
    {
        UI = FindObjectOfType<UI_Controller>();
        castle = FindObjectOfType<castle>();
        enemyHealthMax = enemyHealth;
    }
    private void Update() 
    {
        StatusBar.transform.LookAt(Camera.main.transform.position);
        frozenIcon();
    }
    private void OnParticleCollision(GameObject other)
    {
        Tower _tower = other.GetComponentInParent<Tower>();
        getHit(_tower.Damage, _tower.CritChance);
    }
    private void destroyEnemy(ParticleSystem particles, bool isAdd)
    {
        ParticleSystem particleToDestroy = Instantiate(particles, transform.position, Quaternion.identity);
        if (isAdd == true)
        {
        UI.AddPointForKill();
        UI.AddCoinForKill();
        }
        AudioSource.PlayClipAtPoint(DeathEnemyAudioFX, Camera.main.transform.position);
        Destroy(gameObject);
    }
    public void getHit(float TowerDamage, float TowerCritChance)
    {
        CalculateCritical(TowerDamage, TowerCritChance);

        if (CalculateCritical(TowerDamage, TowerCritChance))
        {
            enemyHealth -= TowerDamage * 2;
            damageText(TowerDamage*2, Color.red);
        }
        else
        {
            enemyHealth -= TowerDamage;
            damageText(TowerDamage, Color.yellow);
        }

        hitParticle.Stop();
        hitParticle.Play();
        GetComponent<AudioSource>().PlayOneShot(HitDamageAudioFX);

        healthBar();
        if (enemyHealth <= 0)
        {
            destroyEnemy(deathParticle, true);
        }
    }

    public bool CalculateCritical(float Damage, float CritChance)
    {
        int CritRoll = Mathf.RoundToInt(UnityEngine.Random.Range(1f,10f));
        if (CritRoll <= CritChance)
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
    public void CastleDamager()
    {
        destroyEnemy(castleDamageParticle, false);
    }

    private void damageText(float Damage, Color TextColor)
    {
        Vector3 PosToCreate = new Vector3(transform.position.x, transform.position.y+7, transform.position.z);
        GameObject text = Instantiate(textDamage.gameObject, PosToCreate, Quaternion.identity);
        text.GetComponent<TextMesh>().color = TextColor;
        text.transform.LookAt(Camera.main.transform.position);
        text.GetComponent<TextMesh>().text = Damage.ToString();
        Destroy(text, 0.4f);
    }
    private void healthBar()
    {
        float HPpercent = enemyHealth / enemyHealthMax;
        Health.transform.localScale = new Vector3(HPpercent, 1, 1);
    }
    private void frozenIcon()
    {
        EnemyMovement FrozenIcon = GetComponent<EnemyMovement>();
        if (FrozenIcon.isFrozen)
        {
        Frost.SetActive(true);
        Frost.transform.localScale = new Vector3(3,3,3);
        }
        else
        Frost.SetActive(false);
        Frost.transform.localScale = new Vector3(1f,1f,1f);
    }

}
