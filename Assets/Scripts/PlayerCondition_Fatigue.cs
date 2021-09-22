using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition_Fatigue : PlayerConditionBase
{
    private int originValue;

    /// <summary>
    /// �U���͔���
    /// </summary>
    /// <returns></returns>
    protected override void OnEnterCondition()
    {
        // ���ɖ߂����߂ɕێ�
        originValue = GameData.instance.attackPower;

        // �o�g�����̍U���͂𔼌�
        GameData.instance.attackPower = Mathf.FloorToInt(GameData.instance.attackPower * conditionValue);

        // �e�N���X�� OnEnterCondition ���\�b�h�����s
        base.OnEnterCondition();
    }

    /// <summary>
    /// �U���͂�߂�
    /// </summary>
    /// <returns></returns>
    protected override void OnExitCondition()
    {
        // �U���͂����̒l�ɖ߂�
        GameData.instance.attackPower = originValue;

        // �e�N���X�� OnExitCondition ���\�b�h�����s
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
