using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float MaxMoveCount;
    public int HitPoint;
    public int EnemyAttackPower;
    public int MaxHitPoint;
    [Header("���݂̃G�l�~�[��HP")]
    public int Enemyhp;
    public int MaxEnemyHP;
    public int MaxWaveCount;
    [SerializeField, Header("���݃E�F�[�u�̒l")]
    public int carrentWaveCount;
    public int attackPower;
    //  ���݂̃��x��
    public int playerLevel;

    //  �o���l�̃g�[�^���l
    public int totalExp;

    //  �A�r���e�B�J���p�̃|�C���g
    public int abilityPoint;


    // �o�g���ŕt�^���ꂽ�f�o�t�̃��X�g
    public List<ConditionType> debuffConditionsList = new List<ConditionType>();


    // �I�����Ă���X�e�[�W�̔ԍ�
    public int chooseStageNo;

    // �N���A�ς̃X�e�[�W�̔ԍ�
    public List<int> clearedStageNos;

    // TODO �I�����Ă���X�e�[�W�̃f�[�^
    //public StageData currentStageData;

    // �{�X�o�g���ɂȂ������ǂ����̊m�F�p
    public bool isBossBattled;


    void InitialzeGameData()
    {
        // TODO �L�����p�̃f�[�^������ꍇ�ɂ́A���̃L�������Ƃ̍ő�&#13259;��ݒ�
        //maxHp = currentCharaData.maxHp;

        HitPoint = MaxHitPoint;

        playerLevel = 1;

        totalExp = 0;

        // TODO ���x���A�b�v�Ŋl���ł���{�[�i�X�̃|�C���g
        //abilityPoint += playerLevel;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
