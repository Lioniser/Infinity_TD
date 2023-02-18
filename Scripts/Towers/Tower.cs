using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tower : MonoBehaviour
{
    public string towerTypeParam;
    public Waypoint baseWaypoint;
    [SerializeField] private int totalCoinsSpent = 0;
    [SerializeField] private TextMesh delPrice_txt; 
    [SerializeField] private AudioClip towerBreak_sound;
    [SerializeField] private ParticleSystem towerBreak_PS;

    public Transform Tmenu;
    private UI_Controller Global_UI;
    public float _MenuCD = 3f;
    public bool _MenuON = false;

    private void Start() 
    {
        Global_UI = FindObjectOfType<UI_Controller>();

        Vector3 camPosForLook = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - 40);
        Tmenu.transform.LookAt(camPosForLook);
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

    public void CalculateTotalTowerPrice(int addedCoins)
    {
        totalCoinsSpent += addedCoins;
        delPrice_txt.text = "o" + totalCoinsSpent / 2;
    }

    public void LevelUpTower()
    {
        switch (towerTypeParam)
        {
            case "BasicTower":
                GetComponent<basicTower>().lvlUp();
                break;
            case "TeslaTower":
                GetComponent<teslaTower>().lvlUp();
                break;
            case "FrostTower":
                GetComponent<frostTower>().lvlUp();
                break;
        }
    }
    public void DestroyTower()
    {
        baseWaypoint.towerHere = false;

        TowerFactory factory = FindObjectOfType<TowerFactory>();
        switch (towerTypeParam)
        {
            case "BasicTower":
            {
                factory.basic_towerPrice -= factory.basicTowerNum * factory.basicTowerNum / 3;
                factory.basicTowerNum--;     
                Global_UI.basicTowerPrice_txt.text = "o" + factory.basic_towerPrice;
                break;
            }
            case "TeslaTower":
            {
                factory.tesla_towerPrice -= factory.teslaTowerNum * factory.teslaTowerNum * 2;
                factory.teslaTowerNum--;  
                Global_UI.teslaTowerPrice_txt.text = "o" + factory.tesla_towerPrice;
                break;
            }
            case "FrostTower":
            {
                factory.frost_towerPrice -= factory.frostTowerNum * factory.frostTowerNum;
                factory.frostTowerNum--; 
                Global_UI.frostTowerPrice_txt.text = "o" + factory.frost_towerPrice;
                break;
            }
        }
        AudioSource.PlayClipAtPoint(towerBreak_sound, Camera.main.transform.position, 0.2f);
        Instantiate(towerBreak_PS, transform.position, Quaternion.Euler(-90f,0f,0f));

        Global_UI.AddCoin(totalCoinsSpent / 2);
        Destroy(gameObject);
    }
}
