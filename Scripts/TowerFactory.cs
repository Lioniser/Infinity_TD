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
    public string towerType;

    int TowerNum = 0;
    int TeslaTowerNum = 0;
    int FrostTowerNum = 0;
    public int towerPrice;

    private void Start() 
    {
        Global_UI = FindObjectOfType<UI_Controller>();
        towerType = "empty";
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            if (hit.transform.name == "Ground")
            chooseTower("empty");
            // else
            // Debug.Log("NOT PLANE");
        }
    }
    // FindObjectOfType<TowerFactory>().AddTower(this);
    public void chooseTower(string _towerType)
    {
        if(_towerType == "Tower")
        {
            towerType = "Tower";
            ChosedTowerImage.sprite = TowerImages[1];
            towerPrice = 2;
        }
        else if(_towerType == "TeslaTower")
        {
            towerType = "TeslaTower";
            ChosedTowerImage.sprite = TowerImages[2];
            towerPrice = 5;
        }
        else if(_towerType == "FrostTower")
        {
            towerType = "FrostTower";
            ChosedTowerImage.sprite = TowerImages[3];
            towerPrice = 4;
        }
        else if(_towerType == "empty")
        {
            towerType = "empty";
            ChosedTowerImage.sprite = TowerImages[0];
            towerPrice = 0;
            Global_UI.AddPlacer(false, null, null);
        }

    }
    public void AddTower()
    {
        if (towerType == "empty")
        return;

        Waypoint waypoint = FindObjectOfType<Placer>().currentWaypoint;
        if (towerType == "Tower" && waypoint.isPlaceble && Global_UI.isPlacerActive)
        createTower(waypoint);
        if (towerType == "TeslaTower" && waypoint.isPlaceble && Global_UI.isPlacerActive)
        createTeslaTower(waypoint);
        if (towerType == "FrostTower" && waypoint.isPlaceble && Global_UI.isPlacerActive)
        createFrostTower(waypoint);

        Global_UI.AddPlacer(false, null, null);
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
