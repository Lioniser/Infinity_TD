using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] float spawnTime = 1;
    [SerializeField] EnemyMovement enemyPref;
    [SerializeField] AudioClip enemySpawnAudioFX;
    [SerializeField] UI_Controller UI;

    public IEnumerator waveSpawner;
    int TotalEnemyNumber;
    int mobNumberInWave = 5;
    public int waveNum = 1;
    float waveTime = 0;
    public bool waveActive = false;
    float fade = 1f;
    public bool start = false;

    void Start()
    {
        enemyPref.speed = 1f;
    }
    private void Update() 
    {
        if (gameObject.transform.childCount == 0 && start)
        WaveTimer();
        else
        UI.WaveCooldown_TXT.text = null;

        WaveStartedText();
    }
    IEnumerator enemySpawn()
    {
        UI.WaveStarted.text = "Wave Started";
        waveActive = true;

        for (int i = 1; i <= mobNumberInWave; i++)
        {
            GetComponent<AudioSource>().PlayOneShot(enemySpawnAudioFX);
            EnemyMovement enemy = Instantiate(enemyPref, transform.position, Quaternion.Euler(0f,90f,0f));
            TotalEnemyNumber++;
            enemy.name = "Enemy: #" + TotalEnemyNumber;
            enemy.transform.parent = transform;
            yield return new WaitForSeconds(spawnTime);
        }

        StopCoroutine(waveSpawner);
    }
    IEnumerator waveSpawn()
    {
        for (int i = 1; i <= waveNum; i++)
        {
            UI.WaveNum_TXT.text = "Wave: " + i;

            if (i%2 == 0)
            spawnTime = Mathf.Round((Mathf.Sqrt(i) / i)*100)/100;

            yield return StartCoroutine(enemySpawn());
            //Зміни у хвилі
            mobNumberInWave = mobNumberInWave + 3;
            if (i%5 == 0)
            mobNumberInWave = mobNumberInWave + 5;
            if (i%2 == 0)
            enemyPref.speed = enemyPref.speed / 1.1f;
            //Наступна хвиля
            waveTime = 10f * i;
            waveActive = false;
            waveNum++;
            yield return new WaitForSeconds(waveTime);
        }
        
    }
    public void StartSpawn()
    {
        
        if (start && waveActive)
        return;
        else if (start && !waveActive)
        {
        StartCoroutine(waveSpawner);
        }
        else if (!start)
        {
            waveSpawner = waveSpawn();
            StartCoroutine(waveSpawner);
            start = true;
        }
    }

    private void WaveTimer()
    {
        if (waveActive)
        StartCoroutine(waveSpawner);

        if (waveTime > 0)
        {
            waveTime -= Time.deltaTime;
            UI.WaveCooldown_TXT.text = "Next in " + Mathf.Round(waveTime) + " seconds";
        }
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
