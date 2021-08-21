using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //private string weapon = "Weapon";   // �^�O�w��p

    [Header("���݂�HP")]
    public int hp;

    [Header("�ړ����x�̍ŏ��l")]
    public float minMoveSpeed;
    [Header("�ړ����x�̍ő�l")]
    public float maxMoveSpeed;

    [Header("�U���͂̍ŏ��l")]
    public int minAttackPower;
    [Header("�U���͂̍ő�l")]
    public int maxAttackPower;

    private float moveSpeed;�@�@// �K�p����ړ����x
    private int attackPower;  // �K�p����U����

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
    // Start is called before the first frame update
    void Start()
    {
        SetUpEnemyParameter();
    }

    /// <summary>
    �@�@/// �G�̃p�����[�^(�ړ����x��U���͂Ȃ�)�Ə�Ԃ�ݒ�
    �@�@/// </summary>
    private void SetUpEnemyParameter()
    {
        enemyState = ENEMY_STATE.SET_UP;
        Debug.Log(enemyState);
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        attackPower = Random.Range(minAttackPower, maxAttackPower);
        waitTime = Random.Range(3, 6);

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        enemyState = ENEMY_STATE.WAIT;

        Debug.Log(enemyState);
        Debug.Log("MoveSpeed:" + moveSpeed);
        Debug.Log("AttackPower:" + attackPower);
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
        //if (actionTime == 0)
       // {
           // return;
       // }

        // �ҋ@���
        if (enemyState == ENEMY_STATE.WAIT)
        {
            Debug.Log("�ҋ@�� ���� : " + actionTime + " �b");
            if (actionTime <= 0)
            {
                CheckNextAction();
                Debug.Log("�ҋ@�I��");
                //NextAction(ENEMY_STATE.MOVE);
            }
        }

        //�ړ����
        if (enemyState == ENEMY_STATE.MOVE)
        {
            Move();
            Debug.Log("�ړ��� ���� : " + actionTime + " �b");
            if (actionTime <= 0)
            {
                NextWait();
            }
        }
       
        // ���G�͈͓��ŖړI�n(Player�̈ʒu)������ꍇ�A�ړI�n�܂ňړ�����
        if (enemyState == ENEMY_STATE.ATTACK)
        {
            if (Vector3.Distance(transform.position, destinationPos)>0.8f)
            {
                Move();
                Debug.Log("�ړ��U����");
            }
            else
            {
                // �ړI�n�ɒ�������U��
                enemyState = ENEMY_STATE.READY;
                StartCoroutine(Attack());
                Debug.Log("�U��");
                return;
            }
        }
    }


    /// <summary>
    /// ����̍s�����m�F����
    /// </summary>
   private void CheckNextAction()
    {
        if (searchArea.isSearching)
        {
            if (searchArea.player != null)
            {
                MoveAttack(searchArea.player);
                Debug.Log("�ړ����čU��");
            }
        }
        else
        {
            PreparateMove();
            Debug.Log("�ړ�����");
        }
    }

    /// <summary>
    /// ���G�͈͓���Player�̈ʒu��ړI�n�Ƃ��A�ړ�����
    /// </summary>
    /// <param name="player"></param>
    private void MoveAttack(GameObject player)
    {
        // �ړI�n��ݒ肵�Ă���
        destinationPos = player.transform.position;
        // �ړ����������ݒ肷��
        direction = (transform.position - destinationPos).normalized;
        // �ړ��������������
        //transform.LookAt(destinationPos);
        // Update��Attack�̕����ňړ�������
        enemyState = ENEMY_STATE.ATTACK;
        //anim.SetBool("Run", true);
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
        //anim.SetBool("Run", true);
    }

    /// <summary>
    /// ����̍s���܂ł̑ҋ@
    /// </summary>
    /// <param name="nextState">ENEMY_STATE</param>
    private void NextWait()
    {
        //anim.SetBool("Run", false);
        actionTime = Random.Range(3, 6);
        enemyState = ENEMY_STATE.WAIT;
    }

    /// <summary>
    /// �ړ��̏���
    /// </summary>
    private void Move()
    {
        rb.AddForce(-direction * moveSpeed);
        Debug.Log(rb.velocity);
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

   
    
}
