using UnityEngine;

[SelectionBase]
public class Waypoint : MonoBehaviour
{
    Vector2Int gridPos;
    const int GridSize = 10;
    public bool check = false;
    public bool startCheck = false;
    public bool endCheck = false;
    public bool isPlaceble = true;
    public bool towerHere = false;
    public Waypoint checkFrom;
    public UI_Controller UI;
    private PathFinding pathFinding;

    private void Start() 
    {
        pathFinding = FindObjectOfType<PathFinding>();
        UI = FindObjectOfType<UI_Controller>();
    }
    public int GetGridSize(){
        return GridSize;
    }

    public Vector2Int GetGridPos()
    {
        return new Vector2Int
        (
        Mathf.RoundToInt(transform.position.x / GridSize),
        Mathf.RoundToInt(transform.position.z / GridSize)
        );
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isPlaceble && !towerHere)
            {
                FindObjectOfType<TowerFactory>().AddTower(this);
            }
            else 
            Debug.Log("Тут вже насрано");
        }
    }
    private void OnMouseEnter() 
    {
        if (UI.isPlacerActive)
        FindObjectOfType<Placer>().placerMovement(this);
    }
}
