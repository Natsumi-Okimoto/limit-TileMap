using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BattleUIManager : MonoBehaviour
{

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Slider Slider;



    public void UpdateEnemyHPvar(int Enemyhp,int MaxEnemyHP)
    {
        float enemyValue = (float)Enemyhp / MaxEnemyHP;
        slider.DOValue(enemyValue, 0.25f);
    }

    public void UpdatePlayerHPvar()
    {
        float value = (float)GameData.instance.HitPoint / GameData.instance.MaxHitPoint;
        Debug.Log(value);
        Slider.DOValue(value, 0.25f);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
