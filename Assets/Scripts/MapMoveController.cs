using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class MapMoveController : MonoBehaviour
{
    private Vector3 movePos;
    private float moveDuration = 0.5f;
    [SerializeField]
    private Tilemap tilemapCollider;
    [SerializeField]
    private bool isMoving;
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
    /// ÉLÅ[ì¸óÕîªíË
    /// </summary>
    public void InputMove()
    {
        if (isMoving)
        {
            return;
        }

        movePos.x = Input.GetAxisRaw("Horizontal");
        movePos.y = Input.GetAxisRaw("Vertical");

        isMoving = true;

        //éŒÇﬂà⁄ìÆÇÃó}êß
        if (Mathf.Abs(movePos.x) != 0)
        {
            movePos.y = 0;
        }

        Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + movePos);

        Debug.Log(tilemapCollider.GetColliderType(tilePos));

        if (tilemapCollider.GetColliderType(tilePos) == Tile.ColliderType.Grid)
        {
            isMoving = false;
        }
        else
        {
            Move(transform.position + movePos);
        }
    }

    private void Move(Vector2 destination)
    {
        transform.DOMove(destination, moveDuration).SetEase(Ease.Linear).OnComplete(() => { isMoving = false; });
    }
}
