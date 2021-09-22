using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// コンディションのベースクラス
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
    /// コンディションをセットする際に呼び出す
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
    /// コンディションの効果を適用
    /// </summary>
    /// <returns></returns>
    protected virtual void OnEnterCondition()
    {
        ConditionEffect conditionEffectPrefab = ConditionEffectManager.instance.GetConditionEffect(conditionType);
        Debug.Log(conditionEffectPrefab);

        if (conditionEffectPrefab != null)
        {
            conditionEffect = Instantiate(conditionEffectPrefab, mapmoveController.GetConditionEffectTran());

            Debug.Log("エフェクト生成：" + conditionType.ToString());
        }

        Debug.Log("コンディション付与");
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
        Debug.Log("コンディション削除");

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
        // 毒のダメージ、攻撃力半減、移動速度半減などを適用する

        // 値を変化させる効果の場合は、持続時間経過後に OnExitCondition() を上書きして元の値に戻すこと
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
