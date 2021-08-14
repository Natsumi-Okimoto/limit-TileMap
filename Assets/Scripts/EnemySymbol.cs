using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySymbol : SymbolBase
{
    public override void TriggerSymbol(MapMoveController mapMoveController)
    {
        base.TriggerSymbol(mapMoveController);
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
