using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using UnityEngine.Events;


public class MapMoveController : MonoBehaviour
{
    private Vector3 movePos; //キー入力の入れ物用
    private float moveDuration = 0.5f; //DOMoveの移動する際にかかる時間
    public float MoveDuration { get => moveDuration; }
    //[SerializeField]
    //public static float MaxMoveCount=30; //動ける最大回数
    [SerializeField]
    private Tilemap tilemapCollider;　//衝突判定用
    [SerializeField]
    public bool isMoving;　//動けるかの判定

    private Tween tween;
    public UIManager uiManager;

    public bool IsMoving { set => isMoving = value; get => isMoving; }

    private Stage stage;

    private int steppingRecoveryPoint = 3; // 足踏みしたときの HP 回復量

  
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
    /// キー入力判定
    /// </summary>
    public void InputMove()
    {
        // プレイヤーの番でなければ処理しない
        if (stage.CurrentTurnState != Stage.TurnState.Player)
        {
            return;
        }
        //移動中には処理しない
        if (isMoving)
        {
            return;
        }
        if (GameData.instance.MaxMoveCount == 0)
        {
            return;
            
        }
        //キー入力の受け取り
        movePos.x = Input.GetAxisRaw("Horizontal");
        movePos.y = Input.GetAxisRaw("Vertical");

        if (movePos.x != 0||movePos.y!=0)
        {
            isMoving = true;

            //斜め移動の抑制
            if (Mathf.Abs(movePos.x) != 0)
            {
                movePos.y = 0;
            }

            // タイルマップの座標に変換
            Vector3Int tilePos = tilemapCollider.WorldToCell(transform.position + movePos);

            Debug.Log(tilemapCollider.GetColliderType(tilePos));

            // Grid のコライダーの場合
            if (tilemapCollider.GetColliderType(tilePos) == Tile.ColliderType.Grid)
            {
                // 移動しないで終了
                isMoving = false;
            }
            else // Grid 以外の場合
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
        // InputMove メソッド内の処理をこちらに追加
        GameData.instance.MaxMoveCount--;
        uiManager.UpdateDisplayMoveCount(GameData.instance.MaxMoveCount);
        // 移動(OnComplete メソッドの中身を修正する)
        tween = transform.DOMove(destination, moveDuration).SetEase(Ease.Linear).OnComplete(() => { StartCoroutine(stage.ObserveEnemyTurnState()); });
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // メソッドの開始時にチェックが入っているのでコメントアウト
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

            // TODO エネミーのシンボルに接触した際、プレイヤーに透明のコンディションが付与されている場合

            // 同じシンボルに接触した場合は処理しない
            if (symbolBase.isSymbolTriggerd)
            {
                return;
            }

            Debug.Log("移動先でシンボルに接触：" + symbolBase.symbolType.ToString());

            // エネミーのシンボルの場合
            if (symbolBase.symbolType == SymbolType.enemy)
            {
                // エネミーのシンボルのイベントの重複登録はしない
                if (enemySymbolTriggerEvent != null)
                {
                    return;
                }

                symbolBase.isSymbolTriggerd = true;

                // シンボルのイベントを登録して予約し、すべてのエネミーの移動が終了してから実行
                enemySymbolTriggerEvent = new UnityEvent<MapMoveController>();

                enemySymbolTriggerEvent.AddListener(symbolBase.TriggerSymbol);

                Debug.Log("エネミーバトルを登録");
            }

            // TODO 特殊シンボルの場合、特殊シンボル用のイベントを登録して予約し、バトル後 Stage に戻ってきてから実行
            
            if (symbolBase.symbolType != SymbolType.enemy)
            {
                // TODO 呪いのコンディションの確認
                // 呪い状態である場合は、シンボルのイベントを発生させない

                // それ以外のシンボルはすぐに実行
                symbolBase.TriggerSymbol(this);
            }
        }
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="stage"></param>
    public void SetUpMapMoveController(Stage stage)
    {
        this.stage = stage;
    }

    /// <summary>
    /// 足踏み
    /// </summary>
    public void Stepping()
    {
        // プレイヤーの番でなければ処理しない
        if (stage.CurrentTurnState != Stage.TurnState.Player)
        {
            return;
        }

        GameData.instance.MaxMoveCount--;

        // 足踏みしてHP回復
        GameData.instance.HitPoint = Mathf.Clamp(GameData.instance.HitPoint += steppingRecoveryPoint, 0, GameData.instance.MaxHitPoint);

        // Hp bar 更新
        uiManager.UpdateHPvar();

        // 敵のターン開始
        StartCoroutine(stage.ObserveEnemyTurnState());
    }

    /// <summary>
    /// 登録されているエネミーシンボルのイベント(エネミーとのバトル)を実行
    /// </summary>
    public bool CallBackEnemySymbolTriggerEvent()
    {
       
        if (enemySymbolTriggerEvent != null)
        {
            // 変数名の最後に ? がある場合、変数の値(中身)が null ではない場合のみ実行。
            // つまり、イベントがあるときだけ実行する
            enemySymbolTriggerEvent?.Invoke(this);

            // イベントをクリア。ここも上と同じで、変数内に値があるか確認して、
            // ある場合のみ、Remove～ してイベントをクリアする
            enemySymbolTriggerEvent?.RemoveAllListeners();
            enemySymbolTriggerEvent = null;

            return true;
        }
        return false;
    }

    /// <summary>
    /// Stage の情報を取得
    /// </summary>
    /// <returns></returns>
    public Stage GetStage()
    {
        return stage;
    }
    //private void UpdateHP()
    //{
        //GameData.instance.HitPoint -= GameData.instance.EnemyAttackPower;
        // Hp の値の上限・下限を確認して範囲内に制限
        //GameData.instance.HitPoint = Mathf.Clamp(GameData.instance.HitPoint, 0, GameData.instance.MaxHitPoint);
    //}

}
