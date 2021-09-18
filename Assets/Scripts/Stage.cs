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

    // Stage ゲームオブジェクトが Active 状態になる度に実行されるメソッド
    // ゲーム開始時にも Start メソッドよりも前に実行される(Awake メソッドの後)
    private void OnEnable()
    {
        Debug.Log("OnEnable");

        // HP の更新
        //uI.UpdateHPvar();

           // TODO バトル後にレベルアップした時のカウントの初期化?

        // TODO レベルアップするか確認??

        // TODO レベルアップしていたらレベルアップのボーナス??
        
        // TODO バトルで付与されたデバフの確認と付与??
        
        // ターンの確認とプレイヤーのターンに切り替え。コンディションの更新
        CheckTurn();

         // TODO 特殊シンボルを獲得している場合は獲得処理を実行?
         // ?
        // ボスの出現確認
        if (currentTurnState == TurnState.Boss)
        {
            // ボスの出現
            Debug.Log("Boss 出現");
             // TODO 演出?
             // ?
            // TODO シーン遷移
        }
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

            // TODO コンディションの残り時間の更新??

            
            // TODO 移動ボタンと足踏みボタンを押せる状態にする??
            
            // TODO コンディションの効果を適用?
            
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
}
