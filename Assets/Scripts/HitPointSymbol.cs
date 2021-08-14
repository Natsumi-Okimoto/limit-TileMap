using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPointSymbol : SymbolBase
{
    [SerializeField]
    public int HealPoint;
    public override void TriggerSymbol(MapMoveController mapMoveController)
    {
        base.TriggerSymbol(mapMoveController);
        AddHitPoint(mapMoveController);
    }

    public void AddHitPoint(MapMoveController mapMoveController)
    {
        GameData.instance.HitPoint += HealPoint;
        mapMoveController.uiManager.UpdateHPvar();
    }
}
