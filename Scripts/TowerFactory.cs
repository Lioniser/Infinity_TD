using UnityEngine;
using UnityEngine.UI;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] AudioClip towerPlacedSND;
    [SerializeField] basicTower basic_towerToPlace;
    public int basic_towerPrice;
    [SerializeField] teslaTower tesla_towerToPlace;
    public int tesla_towerPrice;
    [SerializeField] frostTower frost_towerToPlace;
    public int frost_towerPrice;
    UI_Controller Global_UI;

    [SerializeField] Sprite[] TowerImages;
    public string towerType;

    public int basicTowerNum = 0;
    public int teslaTowerNum = 0;
    public int frostTowerNum = 0;
    public int towerPrice;

    private void Start() 
    {
        Global_UI = FindObjectOfType<UI_Controller>();
        towerType = "empty";

        
        Global_UI.basicTowerPrice_txt.text = "o" + basic_towerPrice;
        Global_UI.teslaTowerPrice_txt.text = "o" + tesla_towerPrice;
        Global_UI.frostTowerPrice_txt.text = "o" + frost_towerPrice;
    }
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            chooseTower("BasicTower");
        if (Input.GetKeyDown(KeyCode.Alpha2) && PlayerPrefs.GetInt("AvailabilityLVL") == 1)
            chooseTower("TeslaTower");
        if (Input.GetKeyDown(KeyCode.Alpha3) && PlayerPrefs.GetInt("AvailabilityLVL") == 2)
            chooseTower("FrostTower");
        if (Input.GetKeyDown(KeyCode.Escape))
            chooseTower("empty");
    }
    public void chooseTower(string _towerType)
    {
        if(_towerType == "BasicTower")
        {
            towerType = "BasicTower";
            Global_UI.ChosedTowerImage.sprite = TowerImages[1];
            towerPrice = basic_towerPrice;
        }
        else if(_towerType == "TeslaTower")
        {
            towerType = "TeslaTower";
            Global_UI.ChosedTowerImage.sprite = TowerImages[2];
            towerPrice = tesla_towerPrice;
        }
        else if(_towerType == "FrostTower")
        {
            towerType = "FrostTower";
            Global_UI.ChosedTowerImage.sprite = TowerImages[3];
            towerPrice = frost_towerPrice;
        }
        else if(_towerType == "empty")
        {
            towerType = "empty";
            Global_UI.ChosedTowerImage.sprite = TowerImages[0];
            towerPrice = 0;
            Global_UI.AddPlacer(false, null, null);
        }

    }
    public void AddTower()
    {
        if (towerType == "empty")
        return;

        Waypoint waypoint = FindObjectOfType<Placer>().currentWaypoint;
        if (towerType == "BasicTower" && waypoint.isPlaceble && Global_UI.isPlacerActive)
            createTower(waypoint, towerType);
        if (towerType == "TeslaTower" && waypoint.isPlaceble && Global_UI.isPlacerActive)
            createTeslaTower(waypoint, towerType);
        if (towerType == "FrostTower" && waypoint.isPlaceble && Global_UI.isPlacerActive)
            createFrostTower(waypoint, towerType);

        Global_UI.AddPlacer(false, null, null);
    }

    private void createTower(Waypoint waypoint, string _towerType)
    {
        if (Global_UI.totalCoins >= basic_towerPrice)
        {
            basicTowerNum++;
            AudioSource.PlayClipAtPoint(towerPlacedSND, Camera.main.transform.position, 0.1f);
            basicTower newTower = Instantiate(basic_towerToPlace, waypoint.transform.position, Quaternion.identity);
            newTower.tower.CalculateTotalTowerPrice(basic_towerPrice);
            newTower.tower.towerTypeParam = _towerType;
            newTower.name = "Basic Tower №" + basicTowerNum;
            newTower.transform.parent = transform;
            waypoint.towerHere = true;
            newTower.tower.baseWaypoint = waypoint;

            Global_UI.AddCoin(-basic_towerPrice);
            Global_UI.coins.text = "o " + Global_UI.totalCoins;

            basic_towerPrice += basicTowerNum * basicTowerNum / 3;
            towerPrice = basic_towerPrice;
            Global_UI.basicTowerPrice_txt.text = "o" + basic_towerPrice;
        }
    }

    private void createTeslaTower(Waypoint waypoint, string _towerType)
    {
        if (Global_UI.totalCoins >= tesla_towerPrice)
        {
            teslaTowerNum++;
            AudioSource.PlayClipAtPoint(towerPlacedSND, Camera.main.transform.position, 0.1f);
            teslaTower newTower = Instantiate(tesla_towerToPlace, waypoint.transform.position, Quaternion.identity);
            newTower.tower.CalculateTotalTowerPrice(tesla_towerPrice);
            newTower.tower.towerTypeParam = _towerType;
            newTower.name = "Tesla Tower №" + teslaTowerNum;
            newTower.transform.parent = transform;
            waypoint.towerHere = true;
            newTower.tower.baseWaypoint = waypoint;
            
            Global_UI.AddCoin(-tesla_towerPrice);
            Global_UI.coins.text = "o " + Global_UI.totalCoins;

            tesla_towerPrice += teslaTowerNum * teslaTowerNum * 2;
            towerPrice = tesla_towerPrice;
            Global_UI.teslaTowerPrice_txt.text = "o" + tesla_towerPrice;
        }
    }

    private void createFrostTower(Waypoint waypoint, string _towerType)
    {
        if (Global_UI.totalCoins >= frost_towerPrice)
        {
            frostTowerNum++;
            AudioSource.PlayClipAtPoint(towerPlacedSND, Camera.main.transform.position, 0.1f);
            frostTower newTower = Instantiate(frost_towerToPlace, waypoint.transform.position, Quaternion.identity);
            newTower.tower.CalculateTotalTowerPrice(frost_towerPrice);
            newTower.tower.towerTypeParam = _towerType;
            newTower.name = "Frost Tower №" + frostTowerNum;
            newTower.transform.parent = transform;
            waypoint.towerHere = true;
            newTower.tower.baseWaypoint = waypoint;

            Global_UI.AddCoin(-frost_towerPrice);
            Global_UI.coins.text = "o " + Global_UI.totalCoins;

            frost_towerPrice += frostTowerNum * frostTowerNum;
            towerPrice = frost_towerPrice;
            Global_UI.frostTowerPrice_txt.text = "o" + frost_towerPrice;
        }
    }
}
