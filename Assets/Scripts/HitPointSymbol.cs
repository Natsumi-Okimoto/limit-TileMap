using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointSymbol : SymbolBase
{
    [SerializeField]
    public int HealPoint;
    public override void TriggerSymbol()
    {
        base.TriggerSymbol();
        AddHitPoint();
    }

    public void AddHitPoint()
    {
        GameData.instance.HitPoint += HealPoint;
    }
}
