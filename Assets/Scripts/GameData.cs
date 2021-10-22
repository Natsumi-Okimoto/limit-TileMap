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
    [Header("現在のエネミーのHP")]
    public int Enemyhp;
    public int MaxEnemyHP;
    public int MaxWaveCount;
    [SerializeField, Header("現在ウェーブの値")]
    public int carrentWaveCount;
    public int attackPower;
    //  現在のレベル
    public int playerLevel;

    //  経験値のトータル値
    public int totalExp;

    //  アビリティ開放用のポイント
    public int abilityPoint;


    // バトルで付与されたデバフのリスト
    public List<ConditionType> debuffConditionsList = new List<ConditionType>();


    // 選択しているステージの番号
    public int chooseStageNo;

    // クリア済のステージの番号
    public List<int> clearedStageNos;

    // TODO 選択しているステージのデータ
    //public StageData currentStageData;

    // ボスバトルになったかどうかの確認用
    public bool isBossBattled;


    void InitialzeGameData()
    {
        // TODO キャラ用のデータがある場合には、そのキャラごとの最大&#13259;を設定
        //maxHp = currentCharaData.maxHp;

        HitPoint = MaxHitPoint;

        playerLevel = 1;

        totalExp = 0;

        // TODO レベルアップで獲得できるボーナスのポイント
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
