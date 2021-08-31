using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BattleUIManager : MonoBehaviour
{

    [SerializeField]
    private Slider slider;



    public void UpdateEnemyHPvar(int Enemyhp,int MaxEnemyHP)
    {
        float enemyValue = (float)Enemyhp / MaxEnemyHP;
        slider.DOValue(enemyValue, 0.25f);
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
