using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;


public enum MoveDirectionType
{
    Up,
    Down,
    Left,
    Right,
    Count
}
public class EnemySymbol : SymbolBase
{
    private Tilemap tilemapCollider;
    private BoxCollider2D boxCol;
    private float moveDuration = 0.05f;


    public override void OnEnterSymbol(SymbolManager symbolManager)
    {
        base.OnEnterSymbol(symbolManager);

        tilemapCollider = symbolManager.tilemapCollider;

        TryGetComponent(out boxCol );
    }
    public override void TriggerSymbol(MapMoveController mapMoveController)
    {
        base.TriggerSymbol(mapMoveController);
        //StartBattle();

        Debug.Log("ˆÚ“®æ‚Å“G‚ÉÚG");

        tween = transform.DOShakeScale(0.75f, 1.0f).SetEase(Ease.OutQuart).OnComplete(() => { OnExitSymbol(); });
    }

    protected override void OnExitSymbol()
    {
        symbolManager.RemoveEnemySymbol(this);

        base.OnExitSymbol();

        StartBattle();
    }
    public void StartBattle()
    {
        //SceneStateManager.instance.NextScene(SceneName.Battle);
        SceneStateManager.instance.PreparateBatlleScene();

    }

    public void EnemyMove()
    {
        MoveDirectionType randomDirType = (MoveDirectionType)Random.Range(0, (int)MoveDirectionType.Count);

        Vector3 nextPos = GetMoveDirection(randomDirType);

        SwitchCollider(false);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, nextPos, 0.8f, LayerMask.GetMask("Symbol"));

        Debug.DrawRay(transform.position, nextPos, Color.blue, 0.8f);

        SwitchCollider(true);

        if(hit.collider!=null)
        {
            return;
        }   

        if(hit.collider!=null&&hit.collider.TryGetComponent(out EnemySymbol enemySymbol))
        {
            return;
        }

        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + nextPos);

        if (tilemapCollider.GetColliderType(tilePos) != Tile.ColliderType.Grid)
        {
            transform.DOMove(transform.position + nextPos, moveDuration).SetEase(Ease.Linear);
        }
    }

    private Vector3 GetMoveDirection(MoveDirectionType nextDirection)
    {
        return nextDirection switch
        {
            MoveDirectionType.Up => new Vector2(0, 1),
            MoveDirectionType.Down => new Vector2(0, -1),
            MoveDirectionType.Left => new Vector2(-1, 0),
            MoveDirectionType.Right => new Vector2(1, 0),
            _ => Vector2.zero
        };
    }

    public void SwitchCollider(bool isSwitch)
    {
        boxCol.enabled = isSwitch;
    }
    protected override void DestroySymbol()
    {
        base.DestroySymbol();

    }

}
