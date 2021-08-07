using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSymbol : SymbolBase
{
    public override void TriggerSymbol()
    {
        base.TriggerSymbol();
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
    
