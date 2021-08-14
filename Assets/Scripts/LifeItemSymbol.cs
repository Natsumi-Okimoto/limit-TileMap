using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeItemSymbol : SymbolBase
{
    public override void TriggerSymbol(MapMoveController mapMoveController)
    {
        base.TriggerSymbol(mapMoveController);
        HealMoveCount();
    }



    public void HealMoveCount()
    {
        GameData.instance.MaxMoveCount += 10;

    }



    protected override void DestroySymbol()
    {
        base.DestroySymbol();

    }
}
    
