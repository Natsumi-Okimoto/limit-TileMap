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

    public TurnState CurrentTurnState // プロパティ
    {
        set => currentTurnState = value;
        get => currentTurnState;
    }

    // Stage ゲームオブジェクトが Active 状態になる度に実行されるメソッド
    // ゲーム開始時にも Start メソッドよりも前に実行される(Awake メソッドの後)
    private void OnEnable()
    {
        uI.UpdateHPvar();

        // TODO バトル後にレベルアップした時のカウントの初期化


        // TODO レベルアップするか確認


        // TODO レベルアップしていたらレベルアップのボーナス




        // バトルで付与されたデバフの確認と付与
        CheckDebuffConditions();




        // ターンの確認とプレイヤーのターンに切り替え。コンディションの更新
        CheckTurn();

        // TODO 特殊シンボルを獲得している場合は獲得処理を実行


        // ボスの出現確認
        if (CurrentTurnState == TurnState.Boss)
        {

            // ボスの出現
            Debug.Log("Boss 出現");


            // TODO 演出


            // TODO シーン遷移

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // TODO ステージ選択機能を実装した時の処理

        // ステージのランダム作成
        stageGenerater.GenerateStageFromRandomTiles();

        // 通常のシンボルのランダム作成して List に追加
        symbolManager.AllClearSymbolsList();
        symbolManager.SymbolsList = stageGenerater.GenerateSymbols(-1);

        // TODO 特殊シンボルのランダム作成して List に追加

        // 全シンボルの設定
        symbolManager.SetUpAllSymbols();

        // プレイヤーの設定
        mapMoveController.SetUpMapMoveController(this);

        // 現在のターンをプレイヤーのターンに設定
        currentTurnState = TurnState.Player;

        // TODO エネミーのシンボルに侵入できるようにする(特殊シンボル使う場合)

        GameData.instance.carrentWaveCount++;
    }

    /// <summary>
    /// エネミーのターン経過監視処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator ObserveEnemyTurnState()
    {
        Debug.Log("敵の移動、開始");

        // エネミーの移動を１体ずつ行う。すべて移動が終わるまで、下の処理にはいかない
        yield return
            StartCoroutine(symbolManager.EnemisMove());

        Debug.Log("全ての敵の移動、完了");

        // シンボルのイベントを発生させる
        bool isEnemyTriggerEvent = mapMoveController.CallBackEnemySymbolTriggerEvent();

        Debug.Log(isEnemyTriggerEvent);

        // ターンの状態を確認
        if (!isEnemyTriggerEvent) CheckTurn();

        if (currentTurnState == TurnState.Boss)
        {
            // ボスの出現
            Debug.Log("Boss 出現");

             // TODO 演出??
            // TODO シーン遷移
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    

    /// <summary>
    /// ターンの確認。プレイヤーのターンに切り替え。コンディションの更新
    /// </summary>
    private void CheckTurn()
    {
        if (GameData.instance==null)
        {
            return;
        }
        // 移動できるか確認
        if (GameData.instance.MaxMoveCount <= 0)
        {
            if (GameData.instance.carrentWaveCount <= GameData.instance.MaxWaveCount)
            {
                SceneStateManager.instance.NextScene(SceneName.Main);
            }
            else
            {
                // 移動できないかつウェーブが最大なら、ボスのターンにする
                currentTurnState = TurnState.Boss;
            }
            
        }
        else
        {
            // 移動できるなら、プレイヤーのターンにする
            currentTurnState = TurnState.Player;

            //コンディションの残り時間の更新
            mapMoveController.UpdateConditionsDuration();


            // TODO 移動ボタンと足踏みボタンを押せる状態にする??

            // コンディションの効果を適用
            ApplyEffectConditions();
            
            // 移動の入力を受け付けるようにする
            mapMoveController.isMoving = false;
        }
        Debug.Log(currentTurnState);
    }

    /// <summary>
    /// SymbolManager の情報を取得
    /// </summary>
    /// <returns></returns>
    public SymbolManager GetSymbolManager()
    {
        return symbolManager;
    }

    /// <summary>
    /// バトルで付与されたデバフの確認
    /// </summary>
    private void CheckDebuffConditions()
    {
        // バトル内で付与されているデバフがないか判定
        if (GameData.instance.debuffConditionsList.Count == 0)
        {
            // デバフが登録されていなければ、処理を終了する
            return;
        }

        // 登録されているデバフを順番に
        for (int i = 0; i < GameData.instance.debuffConditionsList.Count; i++)
        {
            // デバフの付与
            AddDebuff(GameData.instance.debuffConditionsList[i]);
        }

        // デバフのリストをクリア。次のバトルに備える
        GameData.instance.debuffConditionsList.Clear();
    }

    /// <summary>
    /// デバフの付与
    /// </summary>
    private void AddDebuff(ConditionType conditionType)
    {
        // TODO ConditionDataSO スクリプタブル・オブジェクトを作成してから適用
        ConditionData conditionData = DataBaseManager.instance.conditionDataSO.conditionDatasList.Find(x => x.conditionType == conditionType);

        // すでに同じコンディションが付与されているか確認
        if (mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == conditionType))
        {
            // すでに付与されている場合は、持続時間を更新し、効果は上書きして処理を終了する
            mapMoveController.GetConditionsList().Find(x => x.GetConditionType() == conditionType).ExtentionCondition(conditionData.duration, conditionData.conditionValue);

            return;
        }

        // 付与するコンディションが睡眠かつ、すでに混乱のコンディションが付与されているときには、睡眠のコンディションは無視する(操作不能になるため)
        if (conditionType == ConditionType.Sleep && mapMoveController.GetConditionsList().Exists(x => x.GetConditionType() == ConditionType.Confusion))
        {
            return;
        }

        // 付与されていないコンディションの場合は、付与する準備する
        PlayerConditionBase playerCondition;

        // Player にコンディションを付与
        playerCondition = conditionType switch
        {
            // TODO コンディションの種類が増えたら、ここに追加する

            ConditionType.Fatigue => mapMoveController.gameObject.AddComponent<PlayerCondition_Fatigue>(),
            _ => null
        };

        // 初期設定を実行
        playerCondition.AddCondition(conditionType, conditionData.duration, conditionData.conditionValue, mapMoveController, symbolManager);

        // コンディション用の List に追加
        mapMoveController.AddConditionsList(playerCondition);
    }

    /// <summary>
    /// 付与されているコンディションの効果をすべて適用
    /// </summary>
    private void ApplyEffectConditions()
    {
        // 付与されているコンディションの効果を順番にすべて適用
        foreach (PlayerConditionBase condition in mapMoveController.GetConditionsList())
        {
            // コンディションの効果を適用
            condition.ApplyEffect();
        }
    }
}
