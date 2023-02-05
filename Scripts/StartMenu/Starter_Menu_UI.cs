using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Starter_Menu_UI : MonoBehaviour
{
    [SerializeField] Text hint_text;
    float timer;
    public int TestInt {get; set;}
    void Start()
    {
        Hints();
    }

    
    void Update()
    {
        Timer();
    }

    // ПІДКАЗКИ ЇБАТЬ
    private void Hints()
    {
        int randomizedNumber = Mathf.RoundToInt(Random.Range(1f, 7f));
        
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

    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
    }
    public void CloseApp()
    {
        Application.Quit();
    }
}
