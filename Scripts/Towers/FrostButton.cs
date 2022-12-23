using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostButton : MonoBehaviour
{
    public Frost_tower tower;
    private int _lvlUpPrice = 4;

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

            tower.Damage = Mathf.Round((Mathf.Sqrt(tower.lvl * 0.1f + tower.Damage)) * 10) / 10;

            if (tower.lvl%5 == 0)
            tower.attackSpeed = Mathf.Round(1.1f * tower.attackSpeed * 10) / 10;

            if (tower.lvl%10 == 0)
            {
            tower.shootRange += 5;
            tower.AreaOfEffect += 5;
            }

            if (tower.lvl%3 == 0)
            {
            tower.FrozenMulti = Mathf.Round((tower.FrozenMulti * 1.3f) * 10) / 10f;    
            tower.FrozenTime = Mathf.Round((tower.FrozenTime * 1.1f) * 10) / 10f;  
            }

            Global_UI.AddCoin(-_lvlUpPrice);
            CalculatelvlUpPrice();
        }
    }

    private void CalculatelvlUpPrice()
    {
        _lvlUpPrice = Mathf.RoundToInt(Mathf.Sqrt(tower.lvl - 1) * 10f + _lvlUpPrice);
        tower.lvlUP_price_txt.text = "o" + _lvlUpPrice;
    }
}
