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
        // SymbolBase(親クラス)に記述されている OnEnterSymbol メソッドを実行
        base.OnEnterSymbol(symbolManager);

        // 移動判定に Ray を利用するので、プレイヤーと同じように、コライダーのタイルマップの情報を取得
        tilemapCollider = symbolManager.tilemapCollider;

        // BoxColider2D の情報を取得
        TryGetComponent(out boxCol );
    }
    public override void TriggerSymbol(MapMoveController mapMoveController)
    {
        // SymbolBase(親クラス)の TriggerSymbol メソッドを実行
        base.TriggerSymbol(mapMoveController);
        // DOTween でエネミーのシンボルをアニメさせるので、OnExitSymbol メソッド内で実行する
        //StartBattle();

        Debug.Log("移動先で敵に接触");

        // エネミーのシンボルのアニメ演出。演出後、OnComplete メソッドを使って OnExitSymbol メソッド実行
        tween = transform.DOShakeScale(0.75f, 1.0f).SetEase(Ease.OutQuart).OnComplete(() => { OnExitSymbol(); });
    }

    protected override void OnExitSymbol()
    {
        // エネミーのシンボル用の List から削除
        symbolManager.RemoveEnemySymbol(this);

        // SymbolBase(親クラス)の OnExitSymbol を実行
        base.OnExitSymbol();

        // バトルの準備
        StartBattle();
    }

    /// <summary>
    /// バトルの準備
    /// </summary>
    public void StartBattle()
    {
        //SceneStateManager.instance.NextScene(SceneName.Battle);
        // シーン遷移の準備
        SceneStateManager.instance.PreparateBatlleScene();

        Debug.Log("Start");

    }

    /// <summary>
    /// エネミーをランダムな方向に１マス移動するか、その場で待機
    /// </summary>
    public void EnemyMove()
    {
        // 移動する方向をランダムに１つ設定
        MoveDirectionType randomDirType = (MoveDirectionType)Random.Range(0, (int)MoveDirectionType.Count);

        // 移動する方向の情報を座標に変換
        Vector3 nextPos = GetMoveDirection(randomDirType);

        // 自分のコライダーをオフにして Ray が自分のコライダーに当たってしまう誤判定を防ぐ
        SwitchCollider(false);

        // 移動する方向に Ray を投射して他のシンボルが存在していないかを確認
        RaycastHit2D hit = Physics2D.Raycast(transform.position, nextPos, 0.8f, LayerMask.GetMask("Symbol"));

        // Scene ビューにて Ray の可視化
        Debug.DrawRay(transform.position, nextPos, Color.blue, 0.8f);

        // コライダーをオン
        SwitchCollider(true);

        // Ray の投射先に別のシンボルがある場合には => エネミーのみとりあえず除外。アイテムの上にエネミーが乗るようになるので
        if (hit.collider!=null)
        {
            // 移動せず終了
            return;
        }

        // Ray がヒットし、それがエネミーであるなら
        if (hit.collider!=null&&hit.collider.TryGetComponent(out EnemySymbol enemySymbol))
        {
            // 移動せず終了
            return;
        }

        // 移動できるタイルかタイルマップの座標に変換して確認(プレイヤーと同じ手法)
        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + nextPos);

        // Grid のコライダーでなければ(プレイヤーと同じ手法)
        if (tilemapCollider.GetColliderType(tilePos) != Tile.ColliderType.Grid)
        {
            // 移動
            transform.DOMove(transform.position + nextPos, moveDuration).SetEase(Ease.Linear);
        }
    }

    /// <summary>
    /// 移動する方向の情報を座標に変換
    /// </summary>
    /// <param name="nextDirection"></param>
    /// <returns></returns>
    private Vector3 GetMoveDirection(MoveDirectionType nextDirection)
    {
        // switch 文の省略記法(case : break の代わりに => を使う。最後の _ => は default: break と同じ)
        return nextDirection switch
        {
            MoveDirectionType.Up => new Vector2(0, 1),
            MoveDirectionType.Down => new Vector2(0, -1),
            MoveDirectionType.Left => new Vector2(-1, 0),
            MoveDirectionType.Right => new Vector2(1, 0),
            _ => Vector2.zero
        };
    }

    /// <summary>
    /// コライダーのオンオフ切り替え
    /// </summary>
    /// <param name="isSwicth"></param>
    public void SwitchCollider(bool isSwitch)
    {
        boxCol.enabled = isSwitch;
    }
    protected override void DestroySymbol()
    {
        base.DestroySymbol();

    }

}
