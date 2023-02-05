using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class castle : MonoBehaviour
{
    public int castleLife = 10;

    UI_Controller UI;
    
    private void Start() 
    {
        UI = FindObjectOfType<UI_Controller>();
        UI.life.text = "Life: " + castleLife;
    }
    public void CastleDamage(int Damage)
    {
        if (castleLife > 0)
            {
                castleLife -= Damage;
                UI.life.text = "Life: " + castleLife;
            }
    }
}
