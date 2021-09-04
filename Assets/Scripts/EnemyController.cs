using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    

   

    [Header("�ړ����x�̍ŏ��l")]
    public float minMoveSpeed;
    [Header("�ړ����x�̍ő�l")]
    public float maxMoveSpeed;

    [Header("�U���͂̍ŏ��l")]
    public int minAttackPower;
    [Header("�U���͂̍ő�l")]
    public int maxAttackPower;
    [Header("���݂̃G�l�~�[��HP")]
    public int Enemyhp;
    public int MaxEnemyHP;

    private float moveSpeed;�@�@// �K�p����ړ����x
    public int attackPower;  // �K�p����U����
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
    private Vector3 destinationPos; // ���G���̈ړ��̖ړI�n(Player�̈ʒu���)
    Vector3 direction;  // �ړ�����ۂ̕���

    Tween tween;

    public bool isDOMove;
    // Start is called before the first frame update
    void Start()
    {
        //SetUpEnemyParameter();
        
    }

    /// <summary>
    �@�@/// �G�̃p�����[�^(�ړ����x��U���͂Ȃ�)�Ə�Ԃ�ݒ�
    �@�@/// </summary>
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
        //DOTween�ɂ��ړ����g��Ȃ��ꍇ
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
        // ������ԁB�G�̃p�����[�^�̏������I�����Ă��Ȃ��ꍇ�ɂ�Update���������Ȃ�
        if (enemyState == ENEMY_STATE.SET_UP)
        {
            return;
        }

        // �s���p�^�C�}�[���J�E���g�_�E��
        actionTime -= Time.deltaTime;
        if (actionTime <= 0)
        {
            CheckNextAction();
        }

        
        
    }


    /// <summary>
    /// ����̍s�����m�F����
    /// </summary>
   private void CheckNextAction()
    {
        // ���݂̃X�e�[�g����Ɏ��̍s����ݒ肷��
        switch (enemyState)
        {
            case ENEMY_STATE.WAIT:
                // �U���Ώۂ�����ꍇ
                if (searchArea.isSearching && searchArea.player != null)
                {
                    // ���G�͈͓��ŖړI�n(Player�̈ʒu)������ꍇ�A�ړI�n�܂ňړ�����
                    if (isDOMove)
                    {
                        DOMove(searchArea.player);
                    }
                    else
                    {
                        MoveAttack(searchArea.player);
                    }
                    
                    Debug.Log("�ړ����čU��");
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
                    Debug.Log("�ړ�����");
                }
                break;
                
            case ENEMY_STATE.MOVE:
                
                NextWait();
                break;
                
            case ENEMY_STATE.ATTACK:
                // �ړI�n�ɒ�������U��
                enemyState = ENEMY_STATE.READY;
                StartCoroutine(Attack());
                Debug.Log("�U��");
                break;
                
            case ENEMY_STATE.READY:
            case ENEMY_STATE.SET_UP:
            default:
                break;
        }
    }

    /// <summary>
    /// ���G�͈͓���Player�̈ʒu��ړI�n�Ƃ��A�ړ�����
    /// </summary>
    /// <param name="player"></param>
    private void MoveAttack(GameObject player)
    {
        // �ړI�n��ݒ肵�Ă���
        destinationPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        // �ړ����������ݒ肷��
        direction = (transform.position - destinationPos).normalized;
        // �ړ��������������
        //transform.LookAt(destinationPos);
        // Update��Attack�̕����ňړ�������
        actionTime = Random.Range(1.0f, 2.0f);
        enemyState = ENEMY_STATE.ATTACK;
        anim.SetBool("Run", true);
    }

    /// <summary>
    /// �ړ��̏���
    /// </summary>
    private void PreparateMove()
    {
        // �����_���ȕ���������
        transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), new Vector3(0, 1, 0));
        direction = transform.position.normalized;
        enemyState = ENEMY_STATE.MOVE;
        actionTime = Random.Range(3, 5);
        anim.SetBool("Run", true);
    }

    /// <summary>
    /// ����̍s���܂ł̑ҋ@
    /// </summary>
    /// <param name="nextState">ENEMY_STATE</param>
    private void NextWait()
    {
        anim.SetBool("Run", false);
        actionTime = Random.Range(3, 6);
        enemyState = ENEMY_STATE.WAIT;
    }

    /// <summary>
    /// �ړ��̏���
    /// </summary>
    private void Move()
    {
        Debug.Log("�ړ��U����");
        rb.velocity = new Vector3(-direction.x * moveSpeed, rb.velocity.y, -direction.z * moveSpeed);
        //rb.AddForce(-direction * moveSpeed);
        //Debug.Log(rb.velocity);
    }

    /// <summary>
    /// �U���̏���
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
            //BattleManager���Ăяo���Č��j�J�E���g�{�P]
            battleManager.AddDestroyCount();
            Destroy(this.gameObject);
        }
    }


    /// <summary>
    /// DOTween �𗘗p�����ړ��U���̕��@
    /// (Enemy�̃R���C�_�[�� IsTrigger ����� Rigidbody �� Use Gravity ��؂� => OnTriggerEnter �œ����蔻�肪����) 
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
    /// DOTween �𗘗p�����ړ����@
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
                //�U������
                Debug.Log("�U���q�b�g");
            }
        }
    }
}
