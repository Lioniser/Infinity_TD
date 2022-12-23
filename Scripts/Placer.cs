using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placer : MonoBehaviour
{
    public Material _TargetColorController;
    private UI_Controller UI;

    private void Start() 
    {
        UI = FindObjectOfType<UI_Controller>();
    }
    
    public Material materialChanger (bool isPlaceable)
    {
        if (isPlaceable)
        _TargetColorController.color = Color.white;
        else
        _TargetColorController.color = Color.red;
        return _TargetColorController;
    }

    public void placerMovement(Waypoint waypoint)
    {
        //ПЕРЕМІСТИТИ ВИБІР
        transform.position = waypoint.transform.position;
        
        if (waypoint.isPlaceble)
        materialChanger(true);
        else
        materialChanger(false);
    }

    // private void towerGhost(string TowerType)
    // {
        
    // }
}
