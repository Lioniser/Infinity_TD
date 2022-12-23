using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Waypoint))] 
public class MeshBinding : MonoBehaviour
{
    Waypoint waypoint;
    private void Awake() 
    {
        waypoint = GetComponent<Waypoint>();
    }
    void Update()
    {
        GridBind();
        PositionName();
    }

    private void PositionName()
    {
        int GridSize = waypoint.GetGridSize();
        string blockNamePos = waypoint.GetGridPos().x + "," + waypoint.GetGridPos().y;
        TextMesh currentPos = GetComponentInChildren<TextMesh>();
        currentPos.text = blockNamePos;
        gameObject.name = blockNamePos;
    }

    private void GridBind()
    {
        int GridSize = waypoint.GetGridSize();
        transform.position = new Vector3(waypoint.GetGridPos().x*GridSize,0f,waypoint.GetGridPos().y*GridSize);
    }
}
