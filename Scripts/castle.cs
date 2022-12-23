using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class castle : MonoBehaviour
{
    public int castleLife = 10;
    [SerializeField] int damageDealt = 1;

    UI_Controller UI;
    
    private void Start() 
    {
        UI = FindObjectOfType<UI_Controller>();
        UI.life.text = "Life: " + castleLife;
    }
    public void CastleDamage()
    {
        if (castleLife > 0)
            {
                castleLife -= damageDealt;
                UI.life.text = "Life: " + castleLife;
            }
    }
}
