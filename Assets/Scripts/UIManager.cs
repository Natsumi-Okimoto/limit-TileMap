using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text txtScore;        // txtScore ゲームオブジェクトの持つ Text コンポーネントをインスペクターからアサインする
    [SerializeField]
    private Slider slider;

    /// <summary>
    /// スコア表示を更新
    /// </summary>
    /// <param name="MoveCount"></param>
    public void UpdateDisplayMoveCount(float MoveCount)
    {
        txtScore.text = MoveCount.ToString();
    }

    public void UpdateHPvar()
    {
        float value =(float) GameData.instance.HitPoint / GameData.instance.MaxHitPoint;
        slider.DOValue(value, 0.25f);
    }
}
