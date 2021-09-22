using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �R���f�B�V�����̃x�[�X�N���X
/// </summary>
public class PlayerConditionBase : MonoBehaviour
{
    [SerializeField] //debug
    protected float conditionDuration;

    [SerializeField] //debug
    protected float conditionValue;

    protected ConditionEffect conditionEffect;

    protected MapMoveController mapmoveController;
    protected SymbolManager symbolManager;

    protected ConditionType conditionType;

    /// <summary>
    /// �R���f�B�V�������Z�b�g����ۂɌĂяo��
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="value"></param>
    public void AddCondition(ConditionType conditionType,float duration,float value,MapMoveController mapMoveController,SymbolManager symbolManager)
    {
        this.conditionType = conditionType;
        conditionDuration = duration;
        conditionValue = value;
        this.mapmoveController=mapMoveController;
        this.symbolManager = symbolManager;

        OnEnterCondition();

    }

    /// <summary>
    /// �R���f�B�V�����̌��ʂ�K�p
    /// </summary>
    /// <returns></returns>
    protected virtual void OnEnterCondition()
    {
        ConditionEffect conditionEffectPrefab = ConditionEffectManager.instance.GetConditionEffect(conditionType);
        Debug.Log(conditionEffectPrefab);

        if (conditionEffectPrefab != null)
        {
            conditionEffect = Instantiate(conditionEffectPrefab, mapmoveController.GetConditionEffectTran());

            Debug.Log("�G�t�F�N�g�����F" + conditionType.ToString());
        }

        Debug.Log("�R���f�B�V�����t�^");
    }

    public void RemoveCondition()
    {
        OnExitCondition();

       
    }

    protected virtual void OnExitCondition()
    {
        if (conditionEffect!=null)
        {
            Destroy(conditionEffect.gameObject);
        }
        Debug.Log("�R���f�B�V�����폜");

        mapmoveController.RemoveConditionsList(this);
    }

    public virtual void CalcDuration()
    {
        conditionDuration--;

        if (conditionDuration <= 0)
        {
            RemoveCondition();
        }
    }


    public ConditionType GetConditionType()
    {
        return conditionType;
    }


    public void ExtentionCondition(float duration, float value)
    {
        conditionDuration += duration;
        conditionValue = value;

        OnEnterCondition();
    }

    public float GetConditionValue()
    {
        return conditionValue;
        
    }

    public virtual void ApplyEffect()
    {
        // �ł̃_���[�W�A�U���͔����A�ړ����x�����Ȃǂ�K�p����

        // �l��ω���������ʂ̏ꍇ�́A�������Ԍo�ߌ�� OnExitCondition() ���㏑�����Č��̒l�ɖ߂�����
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
