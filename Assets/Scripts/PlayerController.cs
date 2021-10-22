using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed;
    [Header("ダッシュの速度")]
    public float DashSpeed;
    [Header("インターバル")]
    public float AttackWait;
    public float newAttackwait;
    [Header("攻撃力")]
    public int Attackpower;

    private bool isGameOver = false;             // GameOver状態の判定用。true ならゲームオーバー。

    private Rigidbody rb;

    private float horizontal;
    private float vertical;

    private float Scale;

    public BattleUIManager uIManager;
    //[SerializeField]
    //public EnemyController enemyController;


    

    private Animator anim;
    //private Vector2 lookDirection = new Vector2(0, -1.0f);
    

    public enum PLAYER_STATE
    {
        WAIT,
        READY,
        ATTACK,
        DASHAVOID,
    }

    public PLAYER_STATE playerState;
     


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Scale = transform.localScale.x;
        playerState = PLAYER_STATE.READY;
        Debug.Log(playerState);
        CalcHP(0);
    }

    // Update is called once per frame
    void Update()
    {
        
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (isGameOver == true)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            //ダッシュ移動の処理
            DashMove();




            if (playerState == PLAYER_STATE.READY)
            {
                //ステートをアタックに
                playerState = PLAYER_STATE.ATTACK;
                Debug.Log(playerState);
                //攻撃アニメーションの再生
                StartCoroutine(AttackMotion());
                //その後ステートは待機状態へ
                //playerState = PLAYER_STATE.WAIT;
                Debug.Log(playerState);


            }else if (playerState == PLAYER_STATE.WAIT)
            {
                //ステートを回避に
                playerState = PLAYER_STATE.DASHAVOID;
                Debug.Log(playerState);
                //スライディングモーション
                StartCoroutine(SlidingMotion());
                //その後またWAITへ
                playerState = PLAYER_STATE.WAIT;
                Debug.Log(playerState);
            }


        }

        if (playerState == PLAYER_STATE.WAIT)
        {
            //ステートがWAITなら設定したインターバルを減らしていく
            AttackWait -= Time.deltaTime;
            //０になればREADY状態へ移行
            if (AttackWait <= 0)
            {
                playerState = PLAYER_STATE.READY;
                Debug.Log(playerState);
                //初期値に戻す
                AttackWait = newAttackwait;
                
            }
        }



        if (Input.GetKey(KeyCode.X))
            {
                BlockMotion();
            }

        //SyncMoveAnimation();

       
    }

    private void FixedUpdate()
    {
        if (isGameOver == true)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Move(3);
       
    }
    /// <summary>
    /// 移動
    /// </summary>
    private void Move(float Speed)
    {
        Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;

        rb.velocity = new Vector3(moveDir.x * Speed, rb.velocity.y, moveDir.z * Speed);

        LookDirection(moveDir);

        anim.SetFloat("Speed", moveDir.x != 0 ? Mathf.Abs(moveDir.x) : Mathf.Abs(moveDir.z));

    }

    private void DashMove()
    {
        //Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 dashX = transform.right * horizontal;
        Vector3 dashZ = transform.forward * vertical;

        rb.AddForce((dashX+dashZ) * 300,ForceMode.Impulse);
        //Debug.Log(rb.velocity);

        //LookDirection(moveDir);
    }

    /// <summary>
    /// 向きを変える
    /// </summary>
    /// <param name="dir">移動値</param>
    private void LookDirection(Vector3 dir)
    {
        // ベクトル(向きと大きさ)の2乗の長さをfloatで戻す = 動いているかどうかの確認し、動いていなければ処理しない
        if (dir.sqrMagnitude <= 0f)
        {
            return;
        }

        // 横方向への入力がない場合には処理しない
        if (dir.x == 0)
        {
            return;
        }

        float pos = 0;
        if (dir.x > 0)
        {
            pos = 1;    // 右
        }
        else
        {
            pos = -1;   // 左
        }

        // プレイヤーの向きを進行方向に合わせる(上下移動の際には変更しない)
        //transform.rotation = Quaternion.Euler(new Vector3(0, pos, 0));
        transform.localScale = new Vector3(Scale * pos, transform.localScale.y, transform.localScale.z);
    }

    //private void SyncMoveAnimation()
    //{
    // Vector2 move = new Vector2(horizontal, vertical);
    //
    //if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
    //{
    //  lookDirection.Set(move.x, move.y);
    // lookDirection.Normalize();
    // }

    //anim.SetFloat("Look X", lookDirection.x);
    //anim.SetFloat("Look Y", lookDirection.y);

    // anim.SetFloat("Speed", move.magnitude);
    //}

    private IEnumerator AttackMotion()
    {
       
    anim.SetTrigger("Attack 0");

        yield return new WaitForSeconds(1.0f);

        playerState = PLAYER_STATE.WAIT;

    }

    private void BlockMotion()
    {
        anim.SetTrigger("Block");
    }

    private void DamageMotion()
    {
        anim.SetBool("Damege",true);
    }

    private IEnumerator SlidingMotion()
    {
        anim.SetTrigger("Sliding");
        gameObject.layer = LayerMask.NameToLayer("Star");

        yield return new WaitForSeconds(0.5f);

        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerEnter(Collider other)
    {

        if (playerState == PLAYER_STATE.ATTACK&&other.TryGetComponent(out EnemyController enemyController))
        {
            enemyController.Damage(Attackpower);
            Debug.Log("攻撃中");
            

        }

        if (other.TryGetComponent(out EnemyController enemyContrller))
        {
            if (enemyContrller.enemyState==EnemyController.ENEMY_STATE.ATTACK)
            {
                CalcHP(enemyContrller.attackPower);
                Debug.Log("ダメージを受けた");
                // デバフ付与の判定
                JudgeDebuffCondition(enemyContrller.GetEnemyData());
                GameOverCheck();
            }
            
        }
    }

    /// <summary>
    /// デバフ付与の判定
    /// </summary>
    public void JudgeDebuffCondition(EnemyData enemyData)
    {
        // エネミー側にデバフの付与設定がない場合には処理しない
        if (enemyData.debuffDatas.Length == 0)
        {
            return;
        }

        // エネミー側に設定されているデバフの数だけ判定
        for (int i = 0; i < enemyData.debuffDatas.Length; i ++){
            // 乱数が付与確率以下なら
            if (Random.Range(0, 100) <= enemyData.debuffDatas[i].rate)
            {
                // デバフ付与
                AddDebuffCondition(enemyData.debuffDatas[i].debuffConditionType);
            }
        }
    }

    /// <summary>
    /// 指定したタイプのデバフを付与し、GameData にて保持。Stage シーンに戻ってから適用
    /// </summary>
    /// <param name="debuffConditionType"></param>
    private void AddDebuffCondition(ConditionType debuffConditionType)
    {
        GameData.instance.debuffConditionsList.Add(debuffConditionType);
    }


    private void CalcHP(int value)
    {
        GameData.instance.HitPoint -= value;
        uIManager.UpdatePlayerHPvar();

    }

    private void GameOverCheck()
    {
        if (GameData.instance.HitPoint <= 0)
        {
            isGameOver = true;

            Debug.Log("GameOver");

            anim.SetBool("Die", true);

            uIManager.DisplayGameOverInfo();

        }
    }
}
