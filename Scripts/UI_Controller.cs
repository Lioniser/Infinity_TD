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
    private bool isCoinAdded;
    [SerializeField] Text coins_added;
    private float disTime;
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
    private tower Anytower;

    private float TimeToCloseUI = 10f;
    [SerializeField] basicTowerUI basicTowerUI;
    [SerializeField] teslaTowerUI teslaTowerUI;
    [SerializeField] frostTowerUI frostTowerUI;

    private void Awake() 
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        castle = FindObjectOfType<castle>();
        spawner = FindObjectOfType<EnemySpawner>();
        towerFactory = FindObjectOfType<TowerFactory>();
    }
    void Start()
    {
        points.text = pointsNum.ToString();
        coins.text =  "o " + totalCoins;
    }
    void Update()
    {
        CheckLOSE();
        CheckWIN();
        QuitLVL(false);

        Clicked();
        CoinAddBlinker();
        coinsText();  
    }

    public void AddPointForKill()
    {
        pointsNum++;
        points.text = pointsNum.ToString();
    }
    public void AddCoinForKill()
    {
        isCoinAdded = true;
        disTime = 1f;
        coins_added.color = Color.yellow;

        int CalculatedCoins = Mathf.RoundToInt(UnityEngine.Random.Range(1f,3f));
        totalCoins += CalculatedCoins;
        coins_added.text = "+" + CalculatedCoins;
        coins.text =  "o " + totalCoins;
    }
    public void AddCoin(int CoindToAdd)
    {
        isCoinAdded = true;
        disTime = 1f;
        if (CoindToAdd < 0)
        {
            coins_added.color = Color.red;
            coins_added.text = " " + CoindToAdd;
        }
        else
        {
            coins_added.color = Color.yellow;
            coins_added.text = "+" + CoindToAdd;
        }
            

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
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            
            if (hit.transform.GetComponent<Waypoint>())
            {
                Waypoint waypoint = hit.transform.GetComponent<Waypoint>();     
                if (towerFactory.towerType != "empty" && !waypoint.towerHere)
                {
                    if (isPlacerActive)
                    {
                        FindObjectOfType<Placer>().placerMovement(waypoint);
                    }
                    else
                    {
                        AddPlacer(true, towerFactory.towerType, waypoint);
                        FindObjectOfType<Placer>().placerMovement(waypoint);
                    }
                }
            }

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

            if (hit.transform.name == "Ground")
            closeAllUI();

            if (hit.transform.GetComponent<basicTower>())
            {
                basicTower _basicTower = hit.transform.GetComponent<basicTower>();
                closeOtherCharacteristics(_basicTower.tower.towerTypeParam);
                _basicTower.characteristics.Char_UI_On(TimeToCloseUI);
                _basicTower.tower.UI_On(TimeToCloseUI);
                _basicTower.Characteristic_text_updater();
                _basicTower.lvlUp();
            }

            if (hit.transform.GetComponent<teslaTower>())
            {
                teslaTower _teslaTower = hit.transform.GetComponent<teslaTower>();
                closeOtherCharacteristics(_teslaTower.tower.towerTypeParam);
                _teslaTower.characteristics.Char_UI_On(TimeToCloseUI);
                _teslaTower.tower.UI_On(TimeToCloseUI);
                _teslaTower.Characteristic_text_updater();
                _teslaTower.lvlUp();
            }

            if (hit.transform.GetComponent<frostTower>())
            {
                frostTower _frostTower = hit.transform.GetComponent<frostTower>();
                closeOtherCharacteristics(_frostTower.tower.towerTypeParam);
                _frostTower.characteristics.Char_UI_On(TimeToCloseUI);
                _frostTower.tower.UI_On(TimeToCloseUI);
                _frostTower.Characteristic_text_updater();
                _frostTower.lvlUp();
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

    private void closeOtherCharacteristics(string towerType)
    {
        if (towerType == "Tower")
        {
            teslaTowerUI._MenuCD = 0f;
            frostTowerUI._MenuCD = 0f;
        }
        if (towerType == "TeslaTower")
        {
            basicTowerUI._MenuCD = 0f;
            frostTowerUI._MenuCD = 0f;
        }
        if (towerType == "FrostTower")
        {
            basicTowerUI._MenuCD = 0f;
            teslaTowerUI._MenuCD = 0f;
        }
        if (towerType == "CloseAll")
        {
            basicTowerUI._MenuCD = 0f;
            teslaTowerUI._MenuCD = 0f;
            frostTowerUI._MenuCD = 0f;
        }
    }

    private void closeAllUI()
    {
        tower[] alltowers = FindObjectsOfType<tower>();
        foreach (tower CurrentTower in alltowers)
        {
            CurrentTower._MenuCD = 0;  
        }
        closeOtherCharacteristics("CloseAll");
    }

    private void CoinAddBlinker()
    {
        if (isCoinAdded)
        {
            disTime = disTime - Time.deltaTime;
            coins_added.color = new Color(coins_added.color.r, coins_added.color.g, coins_added.color.b, disTime);
        }
        if (disTime < 0)
        {
            isCoinAdded = false;
            disTime = 1f;
        }
    }
}
