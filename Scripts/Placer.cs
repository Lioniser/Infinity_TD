using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placer : MonoBehaviour
{
    public Material _TargetColorController;
    private int multi = 3;
    float blinkerFloatNum;
    private UI_Controller UI;
    public Waypoint currentWaypoint;
    [SerializeField] TextMesh placerText;
    [SerializeField] GameObject button;
    string color;

    private void Start() 
    {
        UI = FindObjectOfType<UI_Controller>();
    }
    private void Update() 
    {
        blinkerFloatNum = Mathf.PingPong(Time.time * multi, 1f);
        placerColor();
    }
    
    public void placerColor ()
    {
        if (color == "Green")
        _TargetColorController.color = new Color(0, blinkerFloatNum, 0);
        if (color == "Red")
        _TargetColorController.color = new Color(blinkerFloatNum, 0, 0);
    }

    public void placerMovement(Waypoint waypoint)
    {
        //ПЕРЕМІСТИТИ ВИБІР
        transform.position = waypoint.transform.position;
        currentWaypoint = waypoint;
        
        if (waypoint.isPlaceble)
        {
            color = "Green";
            button.SetActive(true);
        }
        else
        {
            color = "Red";
            button.SetActive(false);
        }
    }

    // private void towerGhost(string TowerType)
    // {
        
    // }
}
