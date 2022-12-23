using UnityEngine;
using UnityEngine.UI;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] Tower basic_towerToPlace;
    [SerializeField] Tesla_tower tesla_towerToPlace;
    [SerializeField] Frost_tower frost_towerToPlace;
    UI_Controller Global_UI;

    [SerializeField] Image ChosedTowerImage;
    [SerializeField] Sprite[] TowerImages;
    string towerType;

    int TowerNum = 0;
    int TeslaTowerNum = 0;
    int FrostTowerNum = 0;

    private void Start() 
    {
        Global_UI = FindObjectOfType<UI_Controller>();
    }
    private void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        chooseTower("Tower");
        if (Input.GetKeyDown(KeyCode.Alpha2))
        chooseTower("TeslaTower");
        if (Input.GetKeyDown(KeyCode.Alpha3))
        chooseTower("FrostTower");
        if (Input.GetKeyDown(KeyCode.Escape))
        chooseTower("empty");
    }
    public void chooseTower(string _towerType)
    {
        if(_towerType == "Tower")
        {
            towerType = "Tower";
            ChosedTowerImage.sprite = TowerImages[1];
            Global_UI.AddPlacer(true, towerType);
        }
        else if(_towerType == "TeslaTower")
        {
            towerType = "TeslaTower";
            ChosedTowerImage.sprite = TowerImages[2];
            Global_UI.AddPlacer(true, towerType);
        }
        else if(_towerType == "FrostTower")
        {
            towerType = "FrostTower";
            ChosedTowerImage.sprite = TowerImages[3];
            Global_UI.AddPlacer(true, towerType);
        }
        else if(_towerType == "empty")
        {
            towerType = "empty";
            ChosedTowerImage.sprite = TowerImages[0];
            Global_UI.AddPlacer(false, null);
        }

    }
    public void AddTower(Waypoint waypoint)
    {
        if (towerType == "Tower")
        createTower(waypoint);
        if (towerType == "TeslaTower")
        createTeslaTower(waypoint);
        if (towerType == "FrostTower")
        createFrostTower(waypoint);
    }

    private void createTower(Waypoint waypoint)
    {
        if (Global_UI.totalCoins >= 2)
        {
            TowerNum++;
            Tower newTower = Instantiate(basic_towerToPlace, waypoint.transform.position, Quaternion.identity);
            newTower.name = "Tower №" + TowerNum;
            newTower.transform.parent = transform;
            waypoint.towerHere = true;
            // newTower.baseWaypoint = waypoint;
            Global_UI.totalCoins = Global_UI.totalCoins - 2;
            Global_UI.coins.text = "o " + Global_UI.totalCoins;
        }
    }

    private void createTeslaTower(Waypoint waypoint)
    {
        if (Global_UI.totalCoins >= 5)
        {
            TeslaTowerNum++;
            Tesla_tower newTower = Instantiate(tesla_towerToPlace, waypoint.transform.position, Quaternion.identity);
            newTower.name = "Tesla Tower №" + TeslaTowerNum;
            newTower.transform.parent = transform;
            waypoint.towerHere = true;
            // newTower.baseWaypoint = waypoint;
            Global_UI.totalCoins = Global_UI.totalCoins - 5;
            Global_UI.coins.text = "o " + Global_UI.totalCoins;
        }
    }

    private void createFrostTower(Waypoint waypoint)
    {
        if (Global_UI.totalCoins >= 4)
        {
            FrostTowerNum++;
            Frost_tower newTower = Instantiate(frost_towerToPlace, waypoint.transform.position, Quaternion.identity);
            newTower.name = "Frost Tower №" + FrostTowerNum;
            newTower.transform.parent = transform;
            waypoint.towerHere = true;
            // newTower.baseWaypoint = waypoint;
            Global_UI.totalCoins = Global_UI.totalCoins - 4;
            Global_UI.coins.text = "o " + Global_UI.totalCoins;
        }
    }
}
