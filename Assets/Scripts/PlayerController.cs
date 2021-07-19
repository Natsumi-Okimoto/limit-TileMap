using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("ˆÚ“®‘¬“x")]
    public float moveSpeed;

    private Rigidbody2D rb;

    private float horizontal;
    private float vertical;

    private Animator anim;
    private Vector2 lookDirection = new Vector2(0, -1.0f);
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        SyncMoveAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }
    /// <summary>
    /// ˆÚ“®
    /// </summary>
    private void Move()
    {
        rb.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
    }

    private void SyncMoveAnimation()
    {
        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        anim.SetFloat("Look X", lookDirection.x);
        anim.SetFloat("Look Y", lookDirection.y);

      // anim.SetFloat("Speed", move.magnitude);
    }
}
