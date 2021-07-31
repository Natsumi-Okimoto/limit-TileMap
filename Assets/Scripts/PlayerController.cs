using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("移動速度")]
    public float moveSpeed;

    private Rigidbody rb;

    private float horizontal;
    private float vertical;

    private float Scale;
        


    

    private Animator anim;
    //private Vector2 lookDirection = new Vector2(0, -1.0f);
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        Scale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        //SyncMoveAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }
    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        Vector3 moveDir = new Vector3(horizontal, 0, vertical).normalized;

        rb.velocity = new Vector3(moveDir.x * moveSpeed,rb.velocity.y, moveDir.z * moveSpeed);

        LookDirection(moveDir);

        anim.SetFloat("Speed", moveDir.x != 0 ? Mathf.Abs(moveDir.x) : Mathf.Abs(moveDir.z));

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
}
