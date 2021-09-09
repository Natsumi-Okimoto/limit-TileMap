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

    [SerializeField]
    private Text txtInfo;

    [SerializeField]
    private CanvasGroup canvasGroupInfo;



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

    /// <summary>
    /// ゲームオーバー表示
    /// </summary>
    public void DisplayGameOverInfo()
    {

        // InfoBackGround ゲームオブジェクトの持つ CanvasGroup コンポーネントの Alpha の値を、1秒かけて 1 に変更して、背景と文字が画面に見えるようにする
        canvasGroupInfo.DOFade(1.0f, 1.0f);

        // 文字列をアニメーションさせて表示
        txtInfo.DOText("Game Over...", 1.0f);
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
