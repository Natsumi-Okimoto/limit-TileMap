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

    private TurnState currentTurnState;

    


    public enum TurnState
    {
        None,
        Player,
        Enemy,
        Boss
    }

    public TurnState CurrentTurnState
    {
        set => currentTurnState = value;
        get => currentTurnState;
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

    // Stage �Q�[���I�u�W�F�N�g�� Active ��ԂɂȂ�x�Ɏ��s����郁�\�b�h
    // �Q�[���J�n���ɂ� Start ���\�b�h�����O�Ɏ��s�����(Awake ���\�b�h�̌�)
    private void OnEnable()
    {
        Debug.Log("OnEnable");

        // HP �̍X�V
        //uI.UpdateHPvar();

           // TODO �o�g����Ƀ��x���A�b�v�������̃J�E���g�̏�����?

        // TODO ���x���A�b�v���邩�m�F??

        // TODO ���x���A�b�v���Ă����烌�x���A�b�v�̃{�[�i�X??
        
        // TODO �o�g���ŕt�^���ꂽ�f�o�t�̊m�F�ƕt�^??
        
        // �^�[���̊m�F�ƃv���C���[�̃^�[���ɐ؂�ւ��B�R���f�B�V�����̍X�V
        CheckTurn();

         // TODO ����V���{�����l�����Ă���ꍇ�͊l�����������s?
         // ?
        // �{�X�̏o���m�F
        if (currentTurnState == TurnState.Boss)
        {
            // �{�X�̏o��
            Debug.Log("Boss �o��");
             // TODO ���o?
             // ?
            // TODO �V�[���J��
        }
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

            // TODO �R���f�B�V�����̎c�莞�Ԃ̍X�V??

            
            // TODO �ړ��{�^���Ƒ����݃{�^�����������Ԃɂ���??
            
            // TODO �R���f�B�V�����̌��ʂ�K�p?
            
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
}
