using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    public UIManager uI;
    [SerializeField]
    private SymbolManager symbolManager;
    [SerializeField]
    private StageGenerater stageGenerater;
    [SerializeField]
    private MapMoveController mapMoveController;

    private TurnState currentTurnState = TurnState.None;

    


    public enum TurnState
    {
        None,
        Player,
        Enemy,
        Boss
    }

    public TurnState CurrentTurnState // �v���p�e�B
    {
        set => currentTurnState = value;
        get => currentTurnState;
    }

    // Stage �Q�[���I�u�W�F�N�g�� Active ��ԂɂȂ�x�Ɏ��s����郁�\�b�h
    // �Q�[���J�n���ɂ� Start ���\�b�h�����O�Ɏ��s�����(Awake ���\�b�h�̌�)
    private void OnEnable()
    {
        uI.UpdateHPvar();

        // TODO �o�g����Ƀ��x���A�b�v�������̃J�E���g�̏�����


        // TODO ���x���A�b�v���邩�m�F


        // TODO ���x���A�b�v���Ă����烌�x���A�b�v�̃{�[�i�X




        // �o�g���ŕt�^���ꂽ�f�o�t�̊m�F�ƕt�^
        CheckDebuffConditions();




        // �^�[���̊m�F�ƃv���C���[�̃^�[���ɐ؂�ւ��B�R���f�B�V�����̍X�V
        CheckTurn();

        // TODO ����V���{�����l�����Ă���ꍇ�͊l�����������s


        // �{�X�̏o���m�F
        if (CurrentTurnState == TurnState.Boss)
        {

            // �{�X�̏o��
            Debug.Log("Boss �o��");


            // TODO ���o


            // TODO �V�[���J��

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // TODO �X�e�[�W�I���@�\�������������̏���

        // �X�e�[�W�̃����_���쐬
        stageGenerater.GenerateStageFromRandomTiles();

        // �ʏ�̃V���{���̃����_���쐬���� List �ɒǉ�
        symbolManager.AllClearSymbolsList();
        symbolManager.SymbolsList = stageGenerater.GenerateSymbols(-1);

        // TODO ����V���{���̃����_���쐬���� List �ɒǉ�

        // �S�V���{���̐ݒ�
        symbolManager.SetUpAllSymbols();

        // �v���C���[�̐ݒ�
        mapMoveController.SetUpMapMoveController(this);

        // ���݂̃^�[�����v���C���[�̃^�[���ɐݒ�
        currentTurnState = TurnState.Player;

        // TODO �G�l�~�[�̃V���{���ɐN���ł���悤�ɂ���(����V���{���g���ꍇ)

        GameData.instance.carrentWaveCount++;
    }

    /// <summary>
    /// �G�l�~�[�̃^�[���o�ߊĎ�����
    /// </summary>
    /// <returns></returns>
    public IEnumerator ObserveEnemyTurnState()
    {
        Debug.Log("�G�̈ړ��A�J�n");

        // �G�l�~�[�̈ړ����P�̂��s���B���ׂĈړ����I���܂ŁA���̏����ɂ͂����Ȃ�
        yield return
            StartCoroutine(symbolManager.EnemisMove());

        Debug.Log("�S�Ă̓G�̈ړ��A����");

        // �V���{���̃C�x���g�𔭐�������
        bool isEnemyTriggerEvent = mapMoveController.CallBackEnemySymbolTriggerEvent();

        Debug.Log(isEnemyTriggerEvent);

        // �^�[���̏�Ԃ��m�F
        if (!isEnemyTriggerEvent) CheckTurn();

        if (currentTurnState == TurnState.Boss)
        {
            // �{�X�̏o��
            Debug.Log("Boss �o��");

             // TODO ���o??
            // TODO �V�[���J��
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    

    /// <summary>
    /// �^�[���̊m�F�B�v���C���[�̃^�[���ɐ؂�ւ��B�R���f�B�V�����̍X�V
    /// </summary>
    private void CheckTurn()
    {
        if (GameData.instance==null)
        {
            return;
        }
        // �ړ��ł��邩�m�F
        if (GameData.instance.MaxMoveCount <= 0)
        {
            if (GameData.instance.carrentWaveCount <= GameData.instance.MaxWaveCount)
            {
                SceneStateManager.instance.NextScene(SceneName.Main);
            }
            else
            {
                // �ړ��ł��Ȃ����E�F�[�u���ő�Ȃ�A�{�X�̃^�[���ɂ���
                currentTurnState = TurnState.Boss;
            }
            
        }
        else
        {
            // �ړ��ł���Ȃ�A�v���C���[�̃^�[���ɂ���
            currentTurnState = TurnState.Player;

            //�R���f�B�V�����̎c�莞�Ԃ̍X�V
            mapMoveController.UpdateConditionsDuration();


            // TODO �ړ��{�^���Ƒ����݃{�^�����������Ԃɂ���??

            // �R���f�B�V�����̌��ʂ�K�p
            ApplyEffectConditions();
            
            // �ړ��̓��͂��󂯕t����悤�ɂ���
            mapMoveController.isMoving = false;
        }
        Debug.Log(currentTurnState);
    }

    /// <summary>
    /// SymbolManager �̏����擾
    /// </summary>
    /// <returns></returns>
    public SymbolManager GetSymbolManager()
    {
        return symbolManager;
    }

    /// <summary>
    /// �o�g���ŕt�^���ꂽ�f�o�t�̊m�F
    /// </summary>
    private void CheckDebuffConditions()
    {
        // �o�g�����ŕt�^����Ă���f�o�t���Ȃ�������
        if (GameData.instance.debuffConditionsList.Count == 0)
        {
            // �f�o�t���o�^����Ă��Ȃ���΁A�������I������
            return;
        }

        // �o�^����Ă���f�o�t�����Ԃ�
        for (int i = 0; i < GameData.instance.debuffConditionsList.Count; i++)
        {
            // �f�o�t�̕t�^
            AddDebuff(GameData.instance.debuffConditionsList[i]);
        }

        // �f�o�t�̃��X�g���N���A�B���̃o�g���ɔ�����
        GameData.instance.debuffConditionsList.Clear();
    }

    /// <summary>
    /// �f�o�t�̕t�^
    /// </summary>
    private void AddDebuff(ConditionType conditionType)
    {
        // TODO ConditionDataSO �X�N���v�^�u���E�I�u�W�F�N�g���쐬���Ă���K�p
        ConditionData conditionData = DataBaseManager.instance.conditionDataSO.conditionDatasList.Find(x => x.conditionType == conditionType);

        // ���łɓ����R���f�B�V�������t�^����Ă��邩�m�F
        if (mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == conditionType))
        {
            // ���łɕt�^����Ă���ꍇ�́A�������Ԃ��X�V���A���ʂ͏㏑�����ď������I������
            mapMoveController.GetConditionsList().Find(x => x.GetConditionType() == conditionType).ExtentionCondition(conditionData.duration, conditionData.conditionValue);

            return;
        }

        // �t�^����R���f�B�V�������������A���łɍ����̃R���f�B�V�������t�^����Ă���Ƃ��ɂ́A�����̃R���f�B�V�����͖�������(����s�\�ɂȂ邽��)
        if (conditionType == ConditionType.Sleep && mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == ConditionType.Confusion))
        {
            return;
        }

        // �t�^����Ă��Ȃ��R���f�B�V�����̏ꍇ�́A�t�^���鏀������
        PlayerConditionBase playerCondition;

        // Player �ɃR���f�B�V������t�^
        playerCondition = conditionType switch
        {
            // TODO �R���f�B�V�����̎�ނ���������A�����ɒǉ�����

            ConditionType.Fatigue => mapMoveController.gameObject.AddComponent<PlayerCondition_Fatigue>(),
            _ => null
        };

        // �����ݒ�����s
        playerCondition.AddCondition(conditionType, conditionData.duration, conditionData.conditionValue, mapMoveController, symbolManager);

        // �R���f�B�V�����p�� List �ɒǉ�
        mapMoveController.AddConditionsList(playerCondition);
    }

    /// <summary>
    /// �t�^����Ă���R���f�B�V�����̌��ʂ����ׂēK�p
    /// </summary>
    private void ApplyEffectConditions()
    {
        // �t�^����Ă���R���f�B�V�����̌��ʂ����Ԃɂ��ׂēK�p
        foreach (PlayerConditionBase condition in mapMoveController.GetConditionsList())
        {
            // �R���f�B�V�����̌��ʂ�K�p
            condition.ApplyEffect();
        }
    }
}
