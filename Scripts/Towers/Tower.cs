using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tower : MonoBehaviour
{
    public string towerTypeParam;
    public Transform Tmenu;
    private UI_Controller Global_UI;
    public float _MenuCD = 3f;
    public bool _MenuON = false;
    private void Start() 
    {
        Global_UI = FindObjectOfType<UI_Controller>();
    }
    private void Update()
    {
        UI_Opener();
        UI_Timer();
    }

    public void UI_On(float cooldown)
    {
        _MenuCD = cooldown;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _MenuON = true;
            UI_Closer();
        }
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
        if (_MenuON && Tmenu.transform.localScale.x <= 1)
        {
            float _increase = Mathf.Lerp(Tmenu.transform.localScale.x, Tmenu.transform.localScale.x + 3, Time.deltaTime);
            Tmenu.transform.localScale = new Vector3 (_increase, _increase, _increase); 
        }
        if (!_MenuON && Tmenu.transform.localScale.x > 0)
        {
            float _decrease = Mathf.Lerp(Tmenu.transform.localScale.x, Tmenu.transform.localScale.x - 4, Time.deltaTime);
            Tmenu.transform.localScale = new Vector3 (_decrease, _decrease, _decrease);   
        }
        if (!_MenuON && Tmenu.transform.localScale.x <= 0.1)
        {
            Tmenu.transform.localScale = new Vector3(0,0,0);
        }
    }
    
    public void UI_Closer()
    {
        tower[] alltowers = FindObjectsOfType<tower>();
        foreach (tower CurrentTower in alltowers)
        {
            if (CurrentTower.transform != transform)
            CurrentTower._MenuCD = 0;  
        }
    }
}
