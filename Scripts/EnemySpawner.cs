using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float spawnTime = 1;
    [SerializeField] Enemy enemyPref;
    [SerializeField] Enemy bossPref;
    [SerializeField] AudioClip enemySpawnAudioFX;
    [SerializeField] AudioClip bossSpawnAudioFX;
    [SerializeField] UI_Controller UI;
    [SerializeField] AudioSource AmbientMusic;

    public IEnumerator waveSpawner;
    private IEnumerator enemySpawner;
    public int spawnProbability = 0;
    int TotalEnemyNumber;
    int mobNumberInWave = 5;
    public int waveNum = 1;
    float waveTime = 0;
    bool isTimerOn = false;
    public bool waveActive = false;
    float fade = 1f;
    public bool start = false;
    
    private void Update() 
    {
        UI.CheckWIN();

        if (transform.childCount == 0 && start && !isTimerOn)
        {
            WaveSaver();
            isTimerOn = true;
        }

        WaveTimer();
        WaveStartedText();
    }
    private IEnumerator enemySpawn()
    {
        UI.WaveStarted.text = "Wave Started";
    
        for (int i = 1; i <= mobNumberInWave; i++)
        {
            if (waveNum <= 10)
            {
                if (i == mobNumberInWave && waveNum % 5 == 0)
                {
                    TotalEnemyNumber++;
                    GetComponent<AudioSource>().PlayOneShot(bossSpawnAudioFX);
                    Enemy boss = Instantiate(bossPref, transform.position, Quaternion.Euler(0f,90f,0f));
                    boss.RollMonster("Boss", waveNum);
                    boss.name = "Boss: #" + TotalEnemyNumber;
                    boss.transform.parent = transform;
                }
                else
                {
                    TotalEnemyNumber++;
                    GetComponent<AudioSource>().PlayOneShot(enemySpawnAudioFX);
                    Enemy enemy = Instantiate(enemyPref, transform.position, Quaternion.Euler(0f,90f,0f));
                    enemy.RollMonster("Monster", waveNum);
                    enemy.name = "Monster: #" + TotalEnemyNumber;
                    enemy.transform.parent = transform;
                }
            }
            else
            {
                int randomBossCount = Mathf.RoundToInt(Random.Range(1f, mobNumberInWave));
                if (i < randomBossCount && waveNum % 5 == 0 && waveNum < 20)
                {
                    TotalEnemyNumber++;
                    GetComponent<AudioSource>().PlayOneShot(bossSpawnAudioFX);
                    Enemy boss = Instantiate(bossPref, transform.position, Quaternion.Euler(0f,90f,0f));
                    boss.RollMonster("Boss", waveNum);
                    boss.name = "Boss: #" + TotalEnemyNumber;
                    boss.transform.parent = transform;
                }
                else if (i < randomBossCount + (waveNum - 20) && waveNum % 5 != 0 && waveNum < 20)
                {
                    TotalEnemyNumber++;
                    GetComponent<AudioSource>().PlayOneShot(bossSpawnAudioFX);
                    Enemy boss = Instantiate(bossPref, transform.position, Quaternion.Euler(0f,90f,0f));
                    boss.RollMonster("Boss", waveNum);
                    boss.name = "Boss: #" + TotalEnemyNumber;
                    boss.transform.parent = transform;
                }
                else if (i < randomBossCount + (waveNum - 10) && waveNum >= 20)
                {
                    TotalEnemyNumber++;
                    GetComponent<AudioSource>().PlayOneShot(bossSpawnAudioFX);
                    Enemy boss = Instantiate(bossPref, transform.position, Quaternion.Euler(0f,90f,0f));
                    boss.RollMonster("Boss", waveNum);
                    boss.name = "Boss: #" + TotalEnemyNumber;
                    boss.transform.parent = transform;
                }
                else
                {
                    TotalEnemyNumber++;
                    GetComponent<AudioSource>().PlayOneShot(enemySpawnAudioFX);
                    Enemy enemy = Instantiate(enemyPref, transform.position, Quaternion.Euler(0f,90f,0f));
                    enemy.RollMonster("Monster", waveNum);
                    enemy.name = "Monster: #" + TotalEnemyNumber;
                    enemy.transform.parent = transform;
                }
            }
            yield return new WaitForSeconds(spawnTime);
        }
        //Зміни у хвилях
        mobNumberInWave += 2;
        if (waveNum%4 == 0)
        {
            mobNumberInWave += 5;
        }
        if (waveNum%2 == 0)
        {
            spawnTime = spawnTime * 0.9f;;
        }
        if (waveNum%3 == 0)
        {
            mobNumberInWave -= 5;
            spawnProbability++;
        }
    }
    public IEnumerator waveSpawn()
    {
        UI.WaveNum_TXT.text = "Wave: " + waveNum;

        yield return StartCoroutine(enemySpawn());
        
        //Наступна хвиля
        waveNum++;
        waveTime = 10f * waveNum / 2;
    }

    private void WaveSaver()
    {
        
        switch (UI.mapLevel)
        {
            case 1:
            {
                if (waveNum - 1 > PlayerPrefs.GetInt("planes"))
                    PlayerPrefs.SetInt("planes", waveNum - 1);
                break;
            }
            case 2:
            {
                if (waveNum - 1 > PlayerPrefs.GetInt("lakes"))
                    PlayerPrefs.SetInt("lakes", waveNum - 1);
                break;
            } 
            case 3:
            {
                if (waveNum - 1 > PlayerPrefs.GetInt("forest"))
                    PlayerPrefs.SetInt("forest", waveNum - 1);
                break;
            } 
        }
    }
    public void StartSpawn()
    {
        if (!start && waveNum == 1)
        {
            AmbientMusic.Play();
            start = true;
            StartCoroutine(waveSpawn());   
        }
        
        if (isTimerOn)
        {
            StartCoroutine(waveSpawn());
            isTimerOn = false;
        }
    }
    private void WaveTimer()
    {
        if (isTimerOn)
        {
            UI.WaveCooldown_TXT.text = "Next in " + Mathf.Round(waveTime) + " seconds";

            if (waveTime > 0)
            {
                waveTime -= Time.deltaTime;
            }
            else if (waveTime <= 0)
            {
                StartCoroutine(waveSpawn());
                isTimerOn = false;
            }
                
        }
        else
            UI.WaveCooldown_TXT.text = null;
    }

    private void WaveStartedText()
    {
        if (UI.WaveStarted.text == "Wave Started" && UI.WaveStarted.color.a > 0)
        {
            UI.WaveStarted.color = new Color (1, 1, 1, fade);
            fade -= Time.deltaTime;
        }
        else
        {
            UI.WaveStarted.text = null;
            UI.WaveStarted.color = new Color (1, 1, 1, 1);
            fade = 1f;
        }
    }
}
