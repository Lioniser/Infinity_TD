using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Starter_Menu_UI : MonoBehaviour
{
    [SerializeField] Transform openMenu;
    float scaleMulti;
    [SerializeField] bool isOpen = false;
    [SerializeField] Text hint_text;
    [SerializeField] Text points_text;
    [SerializeField] Text wave1_text;
    [SerializeField] Text wave2_text;
    [SerializeField] Text wave3_text;
    private UI_Controller Global_UI;
    
    float timer;

    void Start()
    {
        Hints();
        PointsUpdater();
    }

    
    void Update()
    {
        MapMenuController();
        Timer();
    }

    // ПІДКАЗКИ ЇБАТЬ
    private void Hints()
    {
        int randomizedNumber = Mathf.RoundToInt(Random.Range(1f, 6f));
        
        switch (randomizedNumber)
        {
            case 1:
            {
                hint_text.text = 
                "You can lvlup towers. Just click on it";
            }
            break;
            case 2:
            {
                hint_text.text = 
                "You must complete 10 Wave to win. Also you can continue playing, to check your limits";
            }
            break;
            case 3:
            {
                hint_text.text = 
                "You can choose any tower you want, but don't forget that everything has its price";
            }
            break;
            case 4:
            {
                hint_text.text = 
                "Tesla tower can chain. Each 5 LVL`s of your tesla tower will increase your chain number of targets";
            }
            break;
            case 5:
            {
                hint_text.text = 
                "Base tower. It`s just a base tower. With balanced characteristics";
            }
            break;
            case 6:
            {
                hint_text.text = 
                "Frost tower can slow down your enemies. Each 10 LVL`s will increase tower`s area of effect";
            }
            break;
        }

    }

    private void Timer()
    {
        timer += Time.deltaTime;
        if (timer >= 8f)
        {
            Hints();
            timer = 0;
        }
    }

    private void PointsUpdater()
    {
        if (PlayerPrefs.HasKey("points"))
        {
            points_text.text = "Max points earned \n <size=80>" + PlayerPrefs.GetInt("points") + "</size>";
        }
        if (PlayerPrefs.HasKey("planes"))
        {
            wave1_text.text = "Max waves \ncompleted " + PlayerPrefs.GetInt("planes");
        }
        if (PlayerPrefs.HasKey("lakes"))
        {
            wave2_text.text = "Max waves \ncompleted " + PlayerPrefs.GetInt("lakes");
        }
        if (PlayerPrefs.HasKey("forest"))
        {    
            wave3_text.text = "Max waves \ncompleted " + PlayerPrefs.GetInt("forest");
        }
    }

    private void MapMenuController()
    {
        openMenu.transform.localScale = new Vector3(scaleMulti, scaleMulti, scaleMulti);
        
        if (isOpen)
        {
            if (scaleMulti < 1)
            scaleMulti += Time.deltaTime * 4;
            else
            scaleMulti = 1;
        }
        else
        {
            if (scaleMulti > 0.1)
            scaleMulti -= Time.deltaTime * 5;
            else
            scaleMulti = 0;
        }

    }

    public void MapMenuButton()
    {
        if (isOpen)
        isOpen = false;
        else
        isOpen = true;
    }

    public void ChooseLevel(int levelNum)
    {
        PlayerPrefs.SetInt("level", levelNum); 
        SceneManager.LoadScene(levelNum);
    }

    public void ResetSave()
    {
        PlayerPrefs.SetInt("points", 0);
        PlayerPrefs.SetInt("planes", 0);
        PlayerPrefs.SetInt("lakes", 0);
        PlayerPrefs.SetInt("forest", 0);
        PointsUpdater();
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}
