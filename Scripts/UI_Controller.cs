using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;


public class UI_Controller : MonoBehaviour
{
    public int totalCoins = 10;
    public int pointsNum = 0;
    public Text life;
    public Text points;
    public Text coins;
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

    void Start()
    {
        castle = FindObjectOfType<castle>();
        spawner = FindObjectOfType<EnemySpawner>();

        points.text = pointsNum.ToString();
        coins.text =  "o " + totalCoins;
    }
    void Update()
    {
        CheckLOSE();
        CheckWIN();
        QuitLVL(false);
    }

    public void AddPointForKill()
    {
        pointsNum++;
        points.text = pointsNum.ToString();
    }
    public void AddCoinForKill()
    {
        totalCoins = totalCoins + Mathf.RoundToInt(Random.Range(1f,3f));
        coins.text =  "o " + totalCoins;
    }
    public void AddCoin(int CoindToAdd)
    {
        totalCoins = totalCoins + CoindToAdd;
        coins.text =  "o " + totalCoins;
    }

    public void AddPlacer(bool Add, string towerType)
    {
        if (Add)
        {
            if (isPlacerActive)
            return;

            else
            {
            _placer = Instantiate(placerPrefab, transform.position, Quaternion.identity);
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
        if (spawner.waveNum == 2 && spawner.start && spawner.transform.childCount == 0)
        {
        spawner.start = false;
        StopCoroutine(spawner.waveSpawner);

        continue_btn.gameObject.SetActive(true);
        quit_btn.gameObject.SetActive(true);

        endText.color = Color.green;
        endText.text = "YOU WIN!";


        }
    }
}
