using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using UnityEngine.Events;


public class MapMoveController : MonoBehaviour
{
    private Vector3 movePos; //�L�[���͂̓��ꕨ�p
    private float moveDuration = 0.5f; //DOMove�̈ړ�����ۂɂ����鎞��
    public float MoveDuration { get => moveDuration; }
    //[SerializeField]
    //public static float MaxMoveCount=30; //������ő��
    [SerializeField]
    private Tilemap tilemapCollider;�@//�Փ˔���p
    [SerializeField]
    public bool isMoving;�@//�����邩�̔���

    private Tween tween;
    public UIManager uiManager;

    public bool IsMoving { set => isMoving = value; get => isMoving; }

    private Stage stage;

    private int steppingRecoveryPoint = 3; // �����݂����Ƃ��� HP �񕜗�

  
    private UnityEvent<MapMoveController> enemySymbolTriggerEvent;

    private UnityEvent<MapMoveController> orbSymbolTiggerEvent;
    // Start is called before the first frame update
    void Start()
    {
        uiManager.UpdateHPvar();
    }

    // Update is called once per frame
    void Update()
    {
        if (stage!=null)
        {
            InputMove();
        }
        
    }
    /// <summary>
    /// �L�[���͔���
    /// </summary>
    public void InputMove()
    {
        // �v���C���[�̔ԂłȂ���Ώ������Ȃ�
        if (stage.CurrentTurnState != Stage.TurnState.Player)
        {
            return;
        }
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
                //if (GameData.instance.MaxMoveCount > 0)
                //{
                    Move(transform.position + movePos);
                   // GameData.instance.MaxMoveCount--;
                   // uiManager.UpdateDisplayMoveCount(GameData.instance.MaxMoveCount);
                //}
            }
        }
        
        
    }

    private void Move(Vector2 destination)
    {
        // InputMove ���\�b�h���̏�����������ɒǉ�
        GameData.instance.MaxMoveCount--;
        uiManager.UpdateDisplayMoveCount(GameData.instance.MaxMoveCount);
        // �ړ�(OnComplete ���\�b�h�̒��g���C������)
        tween = transform.DOMove(destination, moveDuration).SetEase(Ease.Linear).OnComplete(() => { StartCoroutine(stage.ObserveEnemyTurnState()); });
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���\�b�h�̊J�n���Ƀ`�F�b�N�������Ă���̂ŃR�����g�A�E�g
        //if(collision.TryGetComponent(out EnemySymbol enemySymbol))
        //{
        //    Debug.Log(enemySymbol.symbolType);
        //    enemySymbol.StartBattle();
        //}

        //if (collision.TryGetComponent(out ItemSymbol inemySymbol))
        //{
        //    Debug.Log(inemySymbol.symbolType);
        //    inemySymbol.HealMoveCount();
        //    Destroy(collision.gameObject);
        //}
        if (collision.TryGetComponent(out SymbolBase symbolBase))
        {
            //symbolBase.TriggerSymbol(this);

            // TODO �G�l�~�[�̃V���{���ɐڐG�����ہA�v���C���[�ɓ����̃R���f�B�V�������t�^����Ă���ꍇ

            // �����V���{���ɐڐG�����ꍇ�͏������Ȃ�
            if (symbolBase.isSymbolTriggerd)
            {
                return;
            }

            Debug.Log("�ړ���ŃV���{���ɐڐG�F" + symbolBase.symbolType.ToString());

            // �G�l�~�[�̃V���{���̏ꍇ
            if (symbolBase.symbolType == SymbolType.enemy)
            {
                // �G�l�~�[�̃V���{���̃C�x���g�̏d���o�^�͂��Ȃ�
                if (enemySymbolTriggerEvent != null)
                {
                    return;
                }

                symbolBase.isSymbolTriggerd = true;

                // �V���{���̃C�x���g��o�^���ė\�񂵁A���ׂẴG�l�~�[�̈ړ����I�����Ă�����s
                enemySymbolTriggerEvent = new UnityEvent<MapMoveController>();

                enemySymbolTriggerEvent.AddListener(symbolBase.TriggerSymbol);

                Debug.Log("�G�l�~�[�o�g����o�^");
            }

            // TODO ����V���{���̏ꍇ�A����V���{���p�̃C�x���g��o�^���ė\�񂵁A�o�g���� Stage �ɖ߂��Ă��Ă�����s
            
            if (symbolBase.symbolType != SymbolType.enemy)
            {
                // TODO �􂢂̃R���f�B�V�����̊m�F
                // �􂢏�Ԃł���ꍇ�́A�V���{���̃C�x���g�𔭐������Ȃ�

                // ����ȊO�̃V���{���͂����Ɏ��s
                symbolBase.TriggerSymbol(this);
            }
        }
    }

    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="stage"></param>
    public void SetUpMapMoveController(Stage stage)
    {
        this.stage = stage;
    }

    /// <summary>
    /// ������
    /// </summary>
    public void Stepping()
    {
        // �v���C���[�̔ԂłȂ���Ώ������Ȃ�
        if (stage.CurrentTurnState != Stage.TurnState.Player)
        {
            return;
        }

        GameData.instance.MaxMoveCount--;

        // �����݂���HP��
        GameData.instance.HitPoint = Mathf.Clamp(GameData.instance.HitPoint += steppingRecoveryPoint, 0, GameData.instance.MaxHitPoint);

        // Hp bar �X�V
        uiManager.UpdateHPvar();

        // �G�̃^�[���J�n
        StartCoroutine(stage.ObserveEnemyTurnState());
    }

    /// <summary>
    /// �o�^����Ă���G�l�~�[�V���{���̃C�x���g(�G�l�~�[�Ƃ̃o�g��)�����s
    /// </summary>
    public bool CallBackEnemySymbolTriggerEvent()
    {
       
        if (enemySymbolTriggerEvent != null)
        {
            // �ϐ����̍Ō�� ? ������ꍇ�A�ϐ��̒l(���g)�� null �ł͂Ȃ��ꍇ�̂ݎ��s�B
            // �܂�A�C�x���g������Ƃ��������s����
            enemySymbolTriggerEvent?.Invoke(this);

            // �C�x���g���N���A�B��������Ɠ����ŁA�ϐ����ɒl�����邩�m�F���āA
            // ����ꍇ�̂݁ARemove�` ���ăC�x���g���N���A����
            enemySymbolTriggerEvent?.RemoveAllListeners();
            enemySymbolTriggerEvent = null;

            return true;
        }
        return false;
    }

    /// <summary>
    /// Stage �̏����擾
    /// </summary>
    /// <returns></returns>
    public Stage GetStage()
    {
        return stage;
    }
    //private void UpdateHP()
    //{
        //GameData.instance.HitPoint -= GameData.instance.EnemyAttackPower;
        // Hp �̒l�̏���E�������m�F���Ĕ͈͓��ɐ���
        //GameData.instance.HitPoint = Mathf.Clamp(GameData.instance.HitPoint, 0, GameData.instance.MaxHitPoint);
    //}

}
