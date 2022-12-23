using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerButton : MonoBehaviour
{
    public Tower tower;
    private int _lvlUpPrice = 3;

    private void OnMouseOver()
    {
        
        tower.UI_On();
        lvlUp();
    }
    public void lvlUp()
    {
        UI_Controller Global_UI = FindObjectOfType<UI_Controller>();
        if (Input.GetKeyDown(KeyCode.Mouse0) && Global_UI.totalCoins >= _lvlUpPrice)
        {
            Instantiate(tower.lvlUPParticle.gameObject, tower.transform.position, Quaternion.Euler(-90f,0f,0f));
            tower.lvl++;
            tower.lvl_txt.text = "LVL " + tower.lvl;

            tower.Damage = Mathf.Round((Mathf.Sqrt(tower.lvl + tower.Damage))*10)/10;

            ParticleSystem.EmissionModule towerSpeed = tower.bulletParticle.emission;
            towerSpeed.rateOverTimeMultiplier = Mathf.Round((Mathf.Sqrt(tower.lvl * 0.3f + tower.bulletParticle.emission.rateOverTimeMultiplier))*10)/10;

            Global_UI.AddCoin(-_lvlUpPrice);
            CalculatelvlUpPrice();
        }
    }

    private void CalculatelvlUpPrice()
    {
        _lvlUpPrice = Mathf.RoundToInt(Mathf.Sqrt(tower.lvl - 1) * 1.5f + _lvlUpPrice);
        tower.lvlUP_price_txt.text = "o" + _lvlUpPrice;
    }
}
