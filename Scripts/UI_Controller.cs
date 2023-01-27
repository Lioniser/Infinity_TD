using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;


public class UI_Controller : MonoBehaviour
{
    public int totalCoins = 10;
    public int pointsNum = 0;
    public Text life;
    public Text points;
    public Text coins;
    public Text coins_info;
    private float t_coins = 1f;
    public bool isEnoughCoins;
    public Text endText;
    public Text lose_timer;
    public Button continue_btn;
    public Button quit_btn;

    public Text WaveStarted;
    public Text WaveNum_TXT;
    public Text WaveCooldown_TXT;

    public bool isPlacerActive = false;
    public Placer placerPrefab;
    private Placer _placer;

    public float loseTimer = 15f;
    private castle castle;
    private EnemySpawner spawner;
    private TowerFactory towerFactory;

    void Start()
    {
        castle = FindObjectOfType<castle>();
        spawner = FindObjectOfType<EnemySpawner>();
        towerFactory = FindObjectOfType<TowerFactory>();

        points.text = pointsNum.ToString();
        coins.text =  "o " + totalCoins;
    }
    void Update()
    {
        CheckLOSE();
        CheckWIN();
        QuitLVL(false);

        Clicked();
        coinsText();
    }

    public void AddPointForKill()
    {
        pointsNum++;
        points.text = pointsNum.ToString();
    }
    public void AddCoinForKill()
    {
        totalCoins = totalCoins + Mathf.RoundToInt(UnityEngine.Random.Range(1f,3f));
        coins.text =  "o " + totalCoins;
    }
    public void AddCoin(int CoindToAdd)
    {
        totalCoins = totalCoins + CoindToAdd;
        coins.text =  "o " + totalCoins;
    }

    public void AddPlacer(bool Add, string towerType, Waypoint waypoint)
    {
        if (Add)
        {
            if (isPlacerActive)
            return;

            else
            {
            _placer = Instantiate(placerPrefab, waypoint.transform.position, Quaternion.identity);
            _placer.currentWaypoint = waypoint;
            isPlacerActive = true;

            // _placer.towerGhost(towerType);
            }
        }
        else 
        {
            if (isPlacerActive)
            Destroy(_placer.gameObject);
            isPlacerActive = false;
        }
    }

    public void QuitLVL(bool isButton)
    {
        if (isButton)
            SceneManager.LoadScene(0);
        else
        {
            if (loseTimer <= 0 || Input.anyKeyDown && loseTimer < 15)
            SceneManager.LoadScene(0);
        }
    }
    public void ContinueLVL()
    {
        endText.text = " ";
        continue_btn.gameObject.SetActive(false);
        quit_btn.gameObject.SetActive(false);
        
        spawner.start = true;
        StartCoroutine(spawner.waveSpawner);
    }

    private void CheckLOSE()
    {
        if (castle.castleLife <= 0)
        {
            spawner.start = false;
            StopCoroutine(spawner.waveSpawner);

            loseTimer -= Time.deltaTime;

            endText.color = Color.red;
            endText.text = "YOU LOSE";
            lose_timer.text = Mathf.RoundToInt(loseTimer).ToString();
        }
    }
    private void CheckWIN()
    {
        if (spawner.waveNum == 10 && spawner.start && spawner.transform.childCount == 0)
        {
        spawner.start = false;
        StopCoroutine(spawner.waveSpawner);

        continue_btn.gameObject.SetActive(true);
        quit_btn.gameObject.SetActive(true);

        endText.color = Color.green;
        endText.text = "YOU WIN!";


        }
    }

    public void Clicked()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        Waypoint waypoint = hit.transform.GetComponent<Waypoint>();

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.transform.name == "BuildButton")
            {
                if (totalCoins >= towerFactory.towerPrice)
                {
                towerFactory.AddTower();
                AddPlacer(false, null, null);
                }
                else
                {
                coins_info.text = "Not Enought Coins";
                coins_info.color = Color.red;
                isEnoughCoins = false;
                }
            }        
            else if (towerFactory.towerType != "empty")
            {
                if (isPlacerActive)
                {
                    FindObjectOfType<Placer>().placerMovement(waypoint);
                }
                else
                {
                    if (waypoint.isPlaceble && !waypoint.towerHere)
                    {
                        AddPlacer(true, towerFactory.towerType, waypoint);
                        FindObjectOfType<Placer>().placerMovement(waypoint);
                    }
                    else 
                    {
                    Debug.Log("Тут вже насрано");
                    AddPlacer(true, towerFactory.towerType, waypoint);
                    FindObjectOfType<Placer>().placerMovement(waypoint);
                    }
                }
            }
        }
    }

    private void coinsText()
    {
        if (isEnoughCoins == false && t_coins >= 0)
        {
            t_coins -= Time.deltaTime;
            coins_info.color = new Color(coins_info.color.r, coins_info.color.g, coins_info.color.b, t_coins);
        }
        if (t_coins < 0)
        {
            t_coins = 1f;
            isEnoughCoins = true;
        }
    }
}
