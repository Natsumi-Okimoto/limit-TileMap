using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class MapMoveController : MonoBehaviour
{
    private Vector3 movePos; //キー入力の入れ物用
    private float moveDuration = 0.5f; //DOMoveの移動する際にかかる時間
    [SerializeField]
    private float MaxMoveCount; //動ける最大回数
    [SerializeField]
    private Tilemap tilemapCollider;　//衝突判定用
    [SerializeField]
    private bool isMoving;　//動けるかの判定

    private Tween tween;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputMove();
    }
    /// <summary>
    /// キー入力判定
    /// </summary>
    public void InputMove()
    {
        //移動中には処理しない
        if (isMoving)
        {
            return;
        }
        if (MaxMoveCount == 0)
        {
            return;
            
        }
        //キー入力の受け取り
        movePos.x = Input.GetAxisRaw("Horizontal");
        movePos.y = Input.GetAxisRaw("Vertical");

        if (movePos.x != 0||movePos.y!=0)
        {
            isMoving = true;

            //斜め移動の抑制
            if (Mathf.Abs(movePos.x) != 0)
            {
                movePos.y = 0;
            }

            // タイルマップの座標に変換
            Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + movePos);

            Debug.Log(tilemapCollider.GetColliderType(tilePos));

            // Grid のコライダーの場合
            if (tilemapCollider.GetColliderType(tilePos) == Tile.ColliderType.Grid)
            {
                // 移動しないで終了
                isMoving = false;
            }
            else // Grid 以外の場合
            {
                if (MaxMoveCount > 0)
                {
                    Move(transform.position + movePos);
                    MaxMoveCount--;
                }
            }
        }
        
        
    }

    private void Move(Vector2 destination)
    {
        // 移動
        tween=transform.DOMove(destination, moveDuration).SetEase(Ease.Linear).OnComplete(() => { isMoving = false; });
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemySymbol enemySymbol))
        {
            Debug.Log(enemySymbol.symbolType);
        }
    }

}
