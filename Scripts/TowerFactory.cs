using UnityEngine;
using UnityEngine.UI;

public class TowerFactory : MonoBehaviour
{
    [SerializeField] AudioClip towerPlacedSND;
    [SerializeField] basicTower basic_towerToPlace;
    [SerializeField] teslaTower tesla_towerToPlace;
    [SerializeField] frostTower frost_towerToPlace;
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
        }
    }
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
            createTower(waypoint, towerType);
        if (towerType == "TeslaTower" && waypoint.isPlaceble && Global_UI.isPlacerActive)
            createTeslaTower(waypoint, towerType);
        if (towerType == "FrostTower" && waypoint.isPlaceble && Global_UI.isPlacerActive)
            createFrostTower(waypoint, towerType);

        Global_UI.AddPlacer(false, null, null);
    }

    private void createTower(Waypoint waypoint, string _towerType)
    {
        if (Global_UI.totalCoins >= 2)
        {
            TowerNum++;
            AudioSource.PlayClipAtPoint(towerPlacedSND, Camera.main.transform.position, 0.1f);
            basicTower newTower = Instantiate(basic_towerToPlace, waypoint.transform.position, Quaternion.identity);
            newTower.tower.towerTypeParam = _towerType;
            newTower.name = "Tower №" + TowerNum;
            newTower.transform.parent = transform;
            waypoint.towerHere = true;
            // newTower.baseWaypoint = waypoint;
            Global_UI.AddCoin(-2);
            Global_UI.coins.text = "o " + Global_UI.totalCoins;
        }
    }

    private void createTeslaTower(Waypoint waypoint, string _towerType)
    {
        if (Global_UI.totalCoins >= 5)
        {
            TeslaTowerNum++;
            AudioSource.PlayClipAtPoint(towerPlacedSND, Camera.main.transform.position, 0.1f);
            teslaTower newTower = Instantiate(tesla_towerToPlace, waypoint.transform.position, Quaternion.identity);
            newTower.tower.towerTypeParam = _towerType;
            newTower.name = "Tesla Tower №" + TeslaTowerNum;
            newTower.transform.parent = transform;
            waypoint.towerHere = true;
            // newTower.baseWaypoint = waypoint;
            Global_UI.AddCoin(-5);
            Global_UI.coins.text = "o " + Global_UI.totalCoins;
        }
    }

    private void createFrostTower(Waypoint waypoint, string _towerType)
    {
        if (Global_UI.totalCoins >= 4)
        {
            FrostTowerNum++;
            AudioSource.PlayClipAtPoint(towerPlacedSND, Camera.main.transform.position, 0.1f);
            frostTower newTower = Instantiate(frost_towerToPlace, waypoint.transform.position, Quaternion.identity);
            newTower.tower.towerTypeParam = _towerType;
            newTower.name = "Frost Tower №" + FrostTowerNum;
            newTower.transform.parent = transform;
            waypoint.towerHere = true;
            // newTower.baseWaypoint = waypoint;
            Global_UI.AddCoin(-4);
            Global_UI.coins.text = "o " + Global_UI.totalCoins;
        }
    }
}
