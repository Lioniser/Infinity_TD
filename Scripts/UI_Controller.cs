using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


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
        QuitLVL(false);

        Clicked();
        CoinAddFader();
        CoinsTextFader();  
    }

    public void AddPointForKill(int PointsToAdd)
    {
        pointsNum += PointsToAdd;
        points.text = pointsNum.ToString();
    }
    public void AddCoinForKill(int CoinToAdd)
    {
        disTime = 1f;
        coins_added.color = Color.yellow;

        int CalculatedCoins = Mathf.RoundToInt(UnityEngine.Random.Range(CoinToAdd, CoinToAdd * 2));
        totalCoins += CalculatedCoins;
        coins_added.text = "+" + CalculatedCoins;
        coins.text =  "o " + totalCoins;
    }
    public void AddCoin(int CoindToAdd)
    {
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
        StartCoroutine(spawner.waveSpawn());
        spawner.start = true;

        endText.text = " ";
        continue_btn.gameObject.SetActive(false);
        quit_btn.gameObject.SetActive(false);
    }

    private void CheckLOSE()
    {
        if (castle.castleLife <= 0)
        {
            spawner.start = false;

            loseTimer -= Time.deltaTime;

            endText.color = Color.red;
            endText.text = "YOU LOSE";
            lose_timer.text = Mathf.RoundToInt(loseTimer).ToString();
        }
    }
    public void CheckWIN()
    {
        if (spawner.waveNum == 11 && spawner.start && spawner.transform.childCount == 0)
        {
        spawner.start = false;
    
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
                    
                    t_coins = 1f;
                }
            }

            if (hit.transform.name == "Ground")
            CloseAllUI();

            if (hit.transform.GetComponent<basicTower>())
            {
                basicTower _basicTower = hit.transform.GetComponent<basicTower>();
                CloseOtherCharacteristics(_basicTower.tower.towerTypeParam);
                _basicTower.characteristics.Char_UI_On(TimeToCloseUI);
                _basicTower.tower.UI_On(TimeToCloseUI);
                _basicTower.Characteristic_text_updater();
                _basicTower.lvlUp();
            }

            if (hit.transform.GetComponent<teslaTower>())
            {
                teslaTower _teslaTower = hit.transform.GetComponent<teslaTower>();
                CloseOtherCharacteristics(_teslaTower.tower.towerTypeParam);
                _teslaTower.characteristics.Char_UI_On(TimeToCloseUI);
                _teslaTower.tower.UI_On(TimeToCloseUI);
                _teslaTower.Characteristic_text_updater();
                _teslaTower.lvlUp();
            }

            if (hit.transform.GetComponent<frostTower>())
            {
                frostTower _frostTower = hit.transform.GetComponent<frostTower>();
                CloseOtherCharacteristics(_frostTower.tower.towerTypeParam);
                _frostTower.characteristics.Char_UI_On(TimeToCloseUI);
                _frostTower.tower.UI_On(TimeToCloseUI);
                _frostTower.Characteristic_text_updater();
                _frostTower.lvlUp();
            }
        }
    }
    public void CoinsErrorMessage(string text, Color color)
        {
            coins_info.text = text;
            coins_info.color = color;
            t_coins = 1f;
        }
    private void CoinsTextFader()
    {
        if (t_coins > 0)
        {
            t_coins -= Time.deltaTime;
            coins_info.color = new Color(coins_info.color.r, coins_info.color.g, coins_info.color.b, t_coins);
        }
        else if (t_coins < 0)
        {
            t_coins = 0f;
        }
    }
    private void CoinAddFader()
    {
        if (disTime > 0)
        {
            disTime = disTime - Time.deltaTime;
            coins_added.color = new Color(coins_added.color.r, coins_added.color.g, coins_added.color.b, disTime);
        }
        else if (disTime < 0)
        {
            disTime = 0f;
        }
    }
    private void CloseOtherCharacteristics(string towerType)
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

    private void CloseAllUI()
    {
        tower[] alltowers = FindObjectsOfType<tower>();
        foreach (tower CurrentTower in alltowers)
        {
            CurrentTower._MenuCD = 0;  
        }
        CloseOtherCharacteristics("CloseAll");
    }

    
}
