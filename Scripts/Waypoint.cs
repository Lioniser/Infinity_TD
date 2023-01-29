using UnityEngine;
using System.Collections;

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
    
    public int GetGridSize()
    {
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

    public void SetPathMat()
    {
        transform.Find("Ground").gameObject.SetActive(false);
        transform.Find("Ground_enemy").gameObject.SetActive(true);
    }
}
