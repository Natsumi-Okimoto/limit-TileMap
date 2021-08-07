using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class MapMoveController : MonoBehaviour
{
    private Vector3 movePos; //�L�[���͂̓��ꕨ�p
    private float moveDuration = 0.5f; //DOMove�̈ړ�����ۂɂ����鎞��
    //[SerializeField]
    //public static float MaxMoveCount=30; //������ő��
    [SerializeField]
    private Tilemap tilemapCollider;�@//�Փ˔���p
    [SerializeField]
    private bool isMoving;�@//�����邩�̔���

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
    /// �L�[���͔���
    /// </summary>
    public void InputMove()
    {
        //�ړ����ɂ͏������Ȃ�
        if (isMoving)
        {
            return;
        }
        if (GameData.instance.MaxMoveCount == 0)
        {
            return;
            
        }
        //�L�[���͂̎󂯎��
        movePos.x = Input.GetAxisRaw("Horizontal");
        movePos.y = Input.GetAxisRaw("Vertical");

        if (movePos.x != 0||movePos.y!=0)
        {
            isMoving = true;

            //�΂߈ړ��̗}��
            if (Mathf.Abs(movePos.x) != 0)
            {
                movePos.y = 0;
            }

            // �^�C���}�b�v�̍��W�ɕϊ�
            Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + movePos);

            Debug.Log(tilemapCollider.GetColliderType(tilePos));

            // Grid �̃R���C�_�[�̏ꍇ
            if (tilemapCollider.GetColliderType(tilePos) == Tile.ColliderType.Grid)
            {
                // �ړ����Ȃ��ŏI��
                isMoving = false;
            }
            else // Grid �ȊO�̏ꍇ
            {
                if (GameData.instance.MaxMoveCount > 0)
                {
                    Move(transform.position + movePos);
                    GameData.instance.MaxMoveCount--;
                }
            }
        }
        
        
    }

    private void Move(Vector2 destination)
    {
        // �ړ�
        tween=transform.DOMove(destination, moveDuration).SetEase(Ease.Linear).OnComplete(() => { isMoving = false; });
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemySymbol enemySymbol))
        {
            Debug.Log(enemySymbol.symbolType);
            enemySymbol.StartBattle();
        }

        if (collision.TryGetComponent(out ItemSymbol inemySymbol))
        {
            Debug.Log(inemySymbol.symbolType);
            inemySymbol.HealMoveCount();
            Destroy(collision.gameObject);
        }
    }

}
