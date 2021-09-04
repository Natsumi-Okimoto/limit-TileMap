using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    

   

    [Header("移動速度の最小値")]
    public float minMoveSpeed;
    [Header("移動速度の最大値")]
    public float maxMoveSpeed;

    [Header("攻撃力の最小値")]
    public int minAttackPower;
    [Header("攻撃力の最大値")]
    public int maxAttackPower;
    [Header("現在のエネミーのHP")]
    public int Enemyhp;
    public int MaxEnemyHP;

    private float moveSpeed;　　// 適用する移動速度
    public int attackPower;  // 適用する攻撃力
    [SerializeField]
    public BattleUIManager uIManager;
    [SerializeField]
    public BattleManager battleManager;

    public enum ENEMY_STATE
    {
        SET_UP,
        WAIT,
        MOVE,
        ATTACK,
        READY,
    }
    public ENEMY_STATE enemyState;

    private float actionTime;
    private float waitTime;
    private Rigidbody rb;
    private Animator anim;

    public SearchArea searchArea;
    private Vector3 destinationPos; // 索敵時の移動の目的地(Playerの位置情報)
    Vector3 direction;  // 移動する際の方向

    Tween tween;

    public bool isDOMove;
    // Start is called before the first frame update
    void Start()
    {
        //SetUpEnemyParameter();
        
    }

    /// <summary>
    　　/// 敵のパラメータ(移動速度や攻撃力など)と状態を設定
    　　/// </summary>
    public void SetUpEnemyParameter(BattleManager battle,BattleUIManager battleUI)
    {
        uIManager = battleUI;
        battleManager = battle;
        enemyState = ENEMY_STATE.SET_UP;
        Debug.Log(enemyState);
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        attackPower = Random.Range(minAttackPower, maxAttackPower);
        waitTime = Random.Range(3, 6);
        Enemyhp = MaxEnemyHP;
        uIManager.UpdateEnemyHPvar(Enemyhp, MaxEnemyHP);

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        enemyState = ENEMY_STATE.WAIT;

        Debug.Log(enemyState);
        Debug.Log("MoveSpeed:" + moveSpeed);
        Debug.Log("AttackPower:" + attackPower);
    }
    private void FixedUpdate()
    {
        //DOTweenによる移動を使わない場合
        if (enemyState == ENEMY_STATE.ATTACK && !isDOMove)
        {
            if (Vector3.Distance(transform.position, destinationPos) <= 0.3f)
            {
                return;
            }
            if (rb.velocity.magnitude < 5)
            {
                Move();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 準備状態。敵のパラメータの準備が終了していない場合にはUpdateを処理しない
        if (enemyState == ENEMY_STATE.SET_UP)
        {
            return;
        }

        // 行動用タイマーをカウントダウン
        actionTime -= Time.deltaTime;
        if (actionTime <= 0)
        {
            CheckNextAction();
        }

        
        
    }


    /// <summary>
    /// 次回の行動を確認する
    /// </summary>
   private void CheckNextAction()
    {
        // 現在のステートを基準に次の行動を設定する
        switch (enemyState)
        {
            case ENEMY_STATE.WAIT:
                // 攻撃対象がいる場合
                if (searchArea.isSearching && searchArea.player != null)
                {
                    // 索敵範囲内で目的地(Playerの位置)がある場合、目的地まで移動する
                    if (isDOMove)
                    {
                        DOMove(searchArea.player);
                    }
                    else
                    {
                        MoveAttack(searchArea.player);
                    }
                    
                    Debug.Log("移動して攻撃");
                }
                else
                {
                    if (isDOMove)
                    {
                        DOPreparateMove();
                    }
                    else
                    {
                        PreparateMove();
                    }
                    Debug.Log("移動準備");
                }
                break;
                
            case ENEMY_STATE.MOVE:
                
                NextWait();
                break;
                
            case ENEMY_STATE.ATTACK:
                // 目的地に着いたら攻撃
                enemyState = ENEMY_STATE.READY;
                StartCoroutine(Attack());
                Debug.Log("攻撃");
                break;
                
            case ENEMY_STATE.READY:
            case ENEMY_STATE.SET_UP:
            default:
                break;
        }
    }

    /// <summary>
    /// 索敵範囲内のPlayerの位置を目的地とし、移動する
    /// </summary>
    /// <param name="player"></param>
    private void MoveAttack(GameObject player)
    {
        // 目的地を設定しておく
        destinationPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        // 移動する方向を設定する
        direction = (transform.position - destinationPos).normalized;
        // 移動する方向を向く
        //transform.LookAt(destinationPos);
        // UpdateのAttackの部分で移動させる
        actionTime = Random.Range(1.0f, 2.0f);
        enemyState = ENEMY_STATE.ATTACK;
        anim.SetBool("Run", true);
    }

    /// <summary>
    /// 移動の準備
    /// </summary>
    private void PreparateMove()
    {
        // ランダムな方向を向く
        transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), new Vector3(0, 1, 0));
        direction = transform.position.normalized;
        enemyState = ENEMY_STATE.MOVE;
        actionTime = Random.Range(3, 5);
        anim.SetBool("Run", true);
    }

    /// <summary>
    /// 次回の行動までの待機
    /// </summary>
    /// <param name="nextState">ENEMY_STATE</param>
    private void NextWait()
    {
        anim.SetBool("Run", false);
        actionTime = Random.Range(3, 6);
        enemyState = ENEMY_STATE.WAIT;
    }

    /// <summary>
    /// 移動の処理
    /// </summary>
    private void Move()
    {
        Debug.Log("移動攻撃中");
        rb.velocity = new Vector3(-direction.x * moveSpeed, rb.velocity.y, -direction.z * moveSpeed);
        //rb.AddForce(-direction * moveSpeed);
        //Debug.Log(rb.velocity);
    }

    /// <summary>
    /// 攻撃の処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator Attack()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(2.0f);
        NextWait();
    }

   public void Damage(int Attackpower)
    {
        Enemyhp -= Attackpower;
        uIManager.UpdateEnemyHPvar(Enemyhp, MaxEnemyHP);
        if (Enemyhp <= 0)
        {
            //BattleManagerを呼び出して撃破カウント＋１]
            battleManager.AddDestroyCount();
            Destroy(this.gameObject);
        }
    }


    /// <summary>
    /// DOTween を利用した移動攻撃の方法
    /// (Enemyのコライダーの IsTrigger 入れて Rigidbody の Use Gravity を切る => OnTriggerEnter で当たり判定が取れる) 
    /// </summary>
    /// <param name="player"></param>
    private void DOMove(GameObject player)
    {
        tween = null;

        actionTime = Random.Range(1.0f, 2.0f);

        Vector3 targetPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

        tween = transform.DOMove(targetPos, actionTime).SetEase(Ease.InOutQuart);

        enemyState = ENEMY_STATE.ATTACK;
        anim.SetBool("Run", true);
    }


    /// <summary>
    /// DOTween を利用した移動方法
    /// </summary>
    private void DOPreparateMove()
    {
        tween = null;

        actionTime = Random.Range(3, 5);

        direction = new Vector3(transform.position.x + Random.Range(3, 5), transform.position.y, transform.position.z + Random.Range(3, 5));

        tween = transform.DOMove(direction, actionTime).SetEase(Ease.Linear);

        enemyState=ENEMY_STATE.MOVE;

        anim.SetBool("Run", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            if (enemyState == ENEMY_STATE.ATTACK || enemyState == ENEMY_STATE.READY)
            {
                //攻撃する
                Debug.Log("攻撃ヒット");
            }
        }
    }
}
