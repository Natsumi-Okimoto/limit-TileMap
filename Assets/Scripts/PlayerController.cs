using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("�ړ����x")]
    public float moveSpeed;
    [Header("�_�b�V���̑��x")]
    public float DashSpeed;
    [Header("�C���^�[�o��")]
    public float AttackWait;

    private Rigidbody rb;

    private float horizontal;
    private float vertical;

    private float Scale;
        


    

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
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Z))
        {
            //�_�b�V���ړ��̏���
            DashMove();




            if (playerState == PLAYER_STATE.READY)
            {
                //�X�e�[�g���A�^�b�N��
                playerState = PLAYER_STATE.ATTACK;
                Debug.Log(playerState);
                //�U���A�j���[�V�����̍Đ�
                AttackMotion();
                //���̌�X�e�[�g�͑ҋ@��Ԃ�
                playerState = PLAYER_STATE.WAIT;
                Debug.Log(playerState);


            }else if (playerState == PLAYER_STATE.WAIT)
            {
                //�X�e�[�g�������
                playerState = PLAYER_STATE.DASHAVOID;
                Debug.Log(playerState);
                //�X���C�f�B���O���[�V����
                SlidingMotion();
                //���̌�܂�WAIT��
                playerState = PLAYER_STATE.WAIT;
                Debug.Log(playerState);
            }


        }

        if (playerState == PLAYER_STATE.WAIT)
        {
            //�X�e�[�g��WAIT�Ȃ�ݒ肵���C���^�[�o�������炵�Ă���
            AttackWait -= Time.deltaTime;
            //�O�ɂȂ��READY��Ԃֈڍs
            if (AttackWait <= 0)
            {
                playerState = PLAYER_STATE.READY;
                Debug.Log(playerState);
                
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
       
        Move(3);
       
    }
    /// <summary>
    /// �ړ�
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

        rb.AddForce(transform.right * 300);
        Debug.Log(rb.velocity);

        //LookDirection(moveDir);
    }

    /// <summary>
    /// ������ς���
    /// </summary>
    /// <param name="dir">�ړ��l</param>
    private void LookDirection(Vector3 dir)
    {
        // �x�N�g��(�����Ƒ傫��)��2��̒�����float�Ŗ߂� = �����Ă��邩�ǂ����̊m�F���A�����Ă��Ȃ���Ώ������Ȃ�
        if (dir.sqrMagnitude <= 0f)
        {
            return;
        }

        // �������ւ̓��͂��Ȃ��ꍇ�ɂ͏������Ȃ�
        if (dir.x == 0)
        {
            return;
        }

        float pos = 0;
        if (dir.x > 0)
        {
            pos = 1;    // �E
        }
        else
        {
            pos = -1;   // ��
        }

        // �v���C���[�̌�����i�s�����ɍ��킹��(�㉺�ړ��̍ۂɂ͕ύX���Ȃ�)
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

    private void AttackMotion()
    {
        anim.SetTrigger("Attack 0");
    }

    private void BlockMotion()
    {
        anim.SetTrigger("Block");
    }

    private void DamageMotion()
    {
        anim.SetBool("Damege",true);
    }

    private void SlidingMotion()
    {
        anim.SetTrigger("Sliding");
    }
}
