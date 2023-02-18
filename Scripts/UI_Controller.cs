using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class UI_Controller : MonoBehaviour
{
    public int mapLevel = 0;
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

    [SerializeField] GameObject unlockedObj;
    [SerializeField] Text unlockedTXT;
    private float t_unlocked = 0f;
    bool isUnlocked = false;

    [SerializeField] Button basicTowerButton, teslaTowerButton, frostTowerButton, startWaveButton, continueButton, quitButton, menuButton;

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
    private tower currentTower;

    public Image ChosedTowerImage;
    private float TimeToCloseUI = 10f;
    [SerializeField] basicTowerUI basicTowerUI;
    public Text basicTowerPrice_txt;
    [SerializeField] teslaTowerUI teslaTowerUI;
    public Text teslaTowerPrice_txt;
    [SerializeField] frostTowerUI frostTowerUI;
    public Text frostTowerPrice_txt;



    private void Awake() 
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        mapLevel = PlayerPrefs.GetInt("level");

        castle = FindObjectOfType<castle>();
        spawner = FindObjectOfType<EnemySpawner>();
        towerFactory = FindObjectOfType<TowerFactory>();
    }
    void Start()
    {
        CheckTowerAvailable();
        points.text = pointsNum.ToString();
        coins.text =  "o " + totalCoins;

        basicTowerButton.onClick.AddListener(delegate {towerFactory.chooseTower("BasicTower");});
        teslaTowerButton.onClick.AddListener(delegate {towerFactory.chooseTower("TeslaTower");});
        frostTowerButton.onClick.AddListener(delegate {towerFactory.chooseTower("FrostTower");});
        startWaveButton.onClick.AddListener(delegate {spawner.StartSpawn();});
        continueButton.onClick.AddListener(delegate {ContinueLVL();});
        quitButton.onClick.AddListener(delegate {QuitLVL(true);});
        menuButton.onClick.AddListener(delegate {QuitLVL(true);});
    }
    void Update()
    {
        CheckLOSE();
        QuitLVL(false);

        Clicked();
        CoinAddFader();
        CoinsTextFader();  
        UnlockTextFader();
    }

    public void AddPointForKill(int PointsToAdd)
    {
        pointsNum += PointsToAdd;
        points.text = pointsNum.ToString();
        if (pointsNum > PlayerPrefs.GetInt("points"))
            PlayerPrefs.SetInt("points", pointsNum);
    }
    public void AddCoinForKill(int CoinToAdd)
    {
        disTime = 1f;
        coins_added.color = Color.yellow;

        int CalculatedCoins = Mathf.RoundToInt(Mathf.Round(UnityEngine.Random.Range(CoinToAdd, CoinToAdd * 2 + 1)));
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

        isUnlocked = false;
        endText.text = " ";
        continueButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
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
        if (spawner.waveNum == 11 && spawner.start && spawner.transform.childCount == 0 && !spawner.isSpawnStarted)
        {
        spawner.start = false;

        if (mapLevel == 1 && PlayerPrefs.GetInt("teslaTowerLock") == 0)
        {
            isUnlocked = true;
            unlockedTXT.text = "Tesla tower unlocked";
            PlayerPrefs.SetInt("teslaTowerLock", 1);
        }
        if (mapLevel == 2 && PlayerPrefs.GetInt("frostTowerLock") == 0)
        {
            isUnlocked = true;
            unlockedTXT.text = "Frost tower unlocked";
            PlayerPrefs.SetInt("frostTowerLock", 1);
        }
    
        continueButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);

        endText.color = Color.green;
        endText.text = "YOU WIN!";
        }
    }

    private void CheckTowerAvailable()
    {
        if (PlayerPrefs.GetInt("teslaTowerLock") == 1)
            teslaTowerButton.gameObject.SetActive(true);
        else
            teslaTowerButton.gameObject.SetActive(false);

        if (PlayerPrefs.GetInt("frostTowerLock") == 1)
            frostTowerButton.gameObject.SetActive(true);
        else
            frostTowerButton.gameObject.SetActive(false);
    }

    public void Clicked()
    {   
        if (EventSystem.current.currentSelectedGameObject)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.transform.name == "Ground")
            {
                CloseAllUI();
                towerFactory.chooseTower("empty");
            }


            if (hit.transform.GetComponent<Waypoint>())
            {
                CloseAllUI();
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
                CloseAllUI();
                if (totalCoins >= towerFactory.towerPrice)
                {
                    towerFactory.AddTower();
                    AddPlacer(false, null, null);
                }
                else
                {
                    CoinsErrorMessage("Not enough to build", Color.red);
                }
            }

            if (hit.transform.GetComponent<basicTower>())
            {
                basicTower _basicTower = hit.transform.GetComponent<basicTower>();
                CloseOtherCharacteristics(_basicTower.tower.towerTypeParam);
                _basicTower.characteristics.Char_UI_On(TimeToCloseUI);
                _basicTower.tower.UI_On(TimeToCloseUI);
                _basicTower.Characteristic_text_updater();
                currentTower = _basicTower.tower;
            }

            if (hit.transform.GetComponent<teslaTower>())
            {
                teslaTower _teslaTower = hit.transform.GetComponent<teslaTower>();
                CloseOtherCharacteristics(_teslaTower.tower.towerTypeParam);
                _teslaTower.characteristics.Char_UI_On(TimeToCloseUI);
                _teslaTower.tower.UI_On(TimeToCloseUI);
                _teslaTower.Characteristic_text_updater();
                currentTower = _teslaTower.tower;
                
            }

            if (hit.transform.GetComponent<frostTower>())
            {
                frostTower _frostTower = hit.transform.GetComponent<frostTower>();
                CloseOtherCharacteristics(_frostTower.tower.towerTypeParam);
                _frostTower.characteristics.Char_UI_On(TimeToCloseUI);
                _frostTower.tower.UI_On(TimeToCloseUI);
                _frostTower.Characteristic_text_updater();
                currentTower = _frostTower.tower;
            }

            if (hit.transform.name == "LVL")
                currentTower.LevelUpTower();
            if (hit.transform.name == "DEL")
            {
                currentTower.DestroyTower();
                CloseAllUI();
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
        if (towerType == "BasicTower")
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
    private void UnlockTextFader()
    {
        if (t_unlocked < 1 && isUnlocked)
        {
            t_unlocked += Time.deltaTime * 3;
            unlockedObj.transform.localScale = new Vector3(t_unlocked, t_unlocked, t_unlocked);
        }
        else if (t_unlocked >= 1 && isUnlocked)
        {
            t_unlocked = Mathf.PingPong(Time.time / 2, 0.1f) + 1f;
            unlockedObj.transform.localScale = new Vector3(t_unlocked, t_unlocked, t_unlocked);
        }
        else
        {
            t_unlocked = 0;
            unlockedObj.transform.localScale = new Vector3(0, 0, 0);
        }     
    }

    
}
