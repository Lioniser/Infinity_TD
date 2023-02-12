using UnityEngine;
using System.Collections.Generic;
[ExecuteInEditMode]
[RequireComponent(typeof(Waypoint))] 
public class MeshBinding : MonoBehaviour
{
    Waypoint waypoint;
    [SerializeField] GameObject selectPref;
    [SerializeField] bool placeObject = false;
    private void Awake() 
    {
        waypoint = GetComponent<Waypoint>();
    }
    void Update()
    {
        GridBind();
        PositionName();
        ChangeBlock();
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
    private void ChangeBlock()
    {
        if (placeObject)
        {
            GameObject currentBlock = transform.Find("baseBlock").gameObject;
            DestroyImmediate(currentBlock);
            currentBlock = Instantiate(selectPref, transform.position, Quaternion.identity);
            currentBlock.transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y - 9f, transform.position.z + 0.5f);
            currentBlock.transform.localScale = new Vector3(4.5f,4.5f,4.5f);
            currentBlock.transform.parent = transform;
            currentBlock.transform.name = "baseBlock";
            placeObject = false;
        }
    }
}
