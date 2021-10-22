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

    [SerializeField]
    private List<PlayerConditionBase> conditionsList = new List<PlayerConditionBase>(); //�@�R���f�B�V�����p�̃��X�g

    [SerializeField]
    private Transform conditionEffectTran; //�@�R���f�B�V�����t�^���Ƀv���C���[�̂ǂ̕ӂ�ɃG�t�F�N�g��\�����邩�̈ʒu�̐ݒ�

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

        // MapMoveController �N���X�̃A�^�b�`����Ă���Q�[���I�u�W�F�N�g(Player)�ɐV�����N���X(PlayerCondition_Fatigue)��ǉ�����
        //PlayerConditionBase condition = gameObject.AddComponent<PlayerCondition_Fatigue>();

        // PlayerConditionBase �N���X�� AddCondition ���\�b�h�����s����B
        // �����͍����珇�Ԃ�(�R���f�B�V�����̎�ށA�R���f�B�V�����̎������ԁA�R���f�B�V�����̌���(����͍U���͂ɏ�Z����l)�AMapMoveController �N���X�ASymbolManager �N���X)
        //condition.AddCondition(ConditionType.Fatigue, 5, 0.5f, this,stage.GetSymbolManager());

        // �R���f�B�V�����p�̃��X�g�ɒǉ�
        //conditionsList.Add(condition);
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

        //�擾�^�C�~���O�ɂ���ĕs�p�ӂȐ��l������̂ŁA���̏ꍇ�ɂ͏������Ȃ�
        if (movePos == Vector3.zero)
        {
            return;
        }

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
        //Symbol�X�N���v�g�����Ă��邩�m�F���ď��������s
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
    

    /// <summary>
    /// �����Ɏw�肳�ꂽ�R���f�B�V�������t�^����Ă��邩�m�F
    /// </summary>
    /// <param name="conditionType"></param>
    /// <returns></returns>
    public bool JudgeConditionType(ConditionType conditionType)
    {
        return conditionsList.Find(x => x.GetConditionType() == conditionType);
    }

    /// <summary>
    /// �R���f�B�V�����p�̃G�t�F�N�g�����ʒu�̎擾
    /// </summary>
    /// <returns></returns>
    public Transform GetConditionEffectTran()
    {
        return conditionEffectTran;
    }

    /// <summary>
    /// ���݂̃R���f�B�V�����̏�Ԃ̎c�莞�Ԃ��X�V
    /// </summary>
    public void UpdateConditionsDuration()
    {
        for(int i = 0; i < conditionsList.Count; i++)
        {
            conditionsList[i].CalcDuration();
        }
    }

    /// <summary>
    /// �R���f�B�V������ǉ�
    /// </summary>
    /// <param name="playerCondition"></param>
    public void AddConditionsList(PlayerConditionBase playerCondition)
    {
        conditionsList.Add(playerCondition);
    }

    /// <summary>
    /// �R���f�B�V�������폜
    /// </summary>
    public void RemoveConditionsList(PlayerConditionBase playerCondition)
    {
        conditionsList.Remove(playerCondition);
        Destroy(playerCondition);
    }

    /// <summary>
    /// �R���f�B�V������ List ���擾
    /// </summary>
    /// <returns></returns>
    public List<PlayerConditionBase> GetConditionsList()
    {
        return conditionsList;
    }
}
