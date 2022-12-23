using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaButton : MonoBehaviour
{
    public Tesla_tower tower;
    private int _lvlUpPrice = 5;

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

            tower.Damage = Mathf.Round((Mathf.Sqrt(tower.lvl + tower.Damage)) * 10) / 10;

            if (tower.lvl%2 == 0)
            tower.attackSpeed = Mathf.Round(1.1f * tower.attackSpeed * 10) / 10;

            if (tower.lvl%5 == 0)
            tower.chainNums++;

            Global_UI.AddCoin(-_lvlUpPrice);
            CalculatelvlUpPrice();
        }
    }

    private void CalculatelvlUpPrice()
    {
        _lvlUpPrice = Mathf.RoundToInt(Mathf.Sqrt(tower.lvl - 1) * 2f + _lvlUpPrice);
        tower.lvlUP_price_txt.text = "o" + _lvlUpPrice;
    }
}
