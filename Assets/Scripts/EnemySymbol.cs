using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySymbol : SymbolBase
{
    public override void TriggerSymbol()
    {
        base.TriggerSymbol();
        StartBattle();
    }
    public void StartBattle()
    {
        //SceneStateManager.instance.NextScene(SceneName.Battle);
        SceneStateManager.instance.PreparateBatlleScene();

    }

    protected override void DestroySymbol()
    {
        base.DestroySymbol();

    }

}
