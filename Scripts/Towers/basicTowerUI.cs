using UnityEngine;
using UnityEngine.UI;

public class basicTowerUI : MonoBehaviour
{
    private UI_Controller UI;
    public Text dmg_txt;
    public Text spd_txt;
    public Text rng_txt;
    public float _MenuCD;
    public bool _MenuON = false;

    private void Start() 
    {
        UI = FindObjectOfType<UI_Controller>();
    }
    private void Update() 
    {
        UI_Opener();
        UI_Timer();
    }

    public void Char_UI_On(float cooldown)
    {
        _MenuCD = cooldown;
        _MenuON = true;
    }
    private void UI_Timer()
    {
        if (_MenuON)
        {
            _MenuCD -= Time.deltaTime;
            if (_MenuCD <= 0)
            {
                _MenuON = false; 
            }
        }
    }
    private void UI_Opener()
    {
        if (_MenuON && transform.localScale.x <= 1)
        {
            float _increase = Mathf.Lerp(transform.localScale.x, transform.localScale.x + 3, Time.deltaTime);
            transform.localScale = new Vector3 (_increase, _increase, _increase); 
        }
        if (!_MenuON && transform.localScale.x > 0)
        {
            float _decrease = Mathf.Lerp(transform.localScale.x, transform.localScale.x - 4, Time.deltaTime);
            transform.localScale = new Vector3 (_decrease, _decrease, _decrease);    
        }
        if (!_MenuON && transform.localScale.x <= 0.1)
        {
            transform.localScale = new Vector3(0,0,0);
        }
    }
}
