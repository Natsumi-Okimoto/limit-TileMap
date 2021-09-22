using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Fatigue : PlayerConditionBase
{
    private int originValue;

    /// <summary>
    /// 攻撃力半減
    /// </summary>
    /// <returns></returns>
    protected override void OnEnterCondition()
    {
        // 元に戻すために保持
        originValue = GameData.instance.attackPower;

        // バトル時の攻撃力を半減
        GameData.instance.attackPower = Mathf.FloorToInt(GameData.instance.attackPower * conditionValue);

        // 親クラスの OnEnterCondition メソッドを実行
        base.OnEnterCondition();
    }

    /// <summary>
    /// 攻撃力を戻す
    /// </summary>
    /// <returns></returns>
    protected override void OnExitCondition()
    {
        // 攻撃力を元の値に戻す
        GameData.instance.attackPower = originValue;

        // 親クラスの OnExitCondition メソッドを実行
        base.OnExitCondition();
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
