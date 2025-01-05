using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float dashForce;
    [SerializeField] private bool canDash = true;
    private bool isDashing = false;
    private Rigidbody2D rb;
    [HideInInspector] public float lastHorizontalVector;
    [HideInInspector] public float lastVerticalVector;
    [HideInInspector] public Vector2 moveDir;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        InputManagement();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Dash();
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorizontalVector = moveDir.x;
        }

        if (moveDir.y != 0)
        {
            lastVerticalVector = moveDir.y;
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            rb.AddForce(moveDir * dashForce, ForceMode2D.Impulse);
            if (moveDir == Vector2.zero)
            {
                Debug.LogWarning("MoveDir is zero.");
            }
            canDash = false;
        }
        else if (!canDash && !isDashing)
        {
            StartCoroutine(CanDash(2));
        }
    }
    
    IEnumerator CanDash(float waitTime)
    {
        isDashing = true;
        yield return new WaitForSecondsRealtime(waitTime);
        canDash = true;
        isDashing = false;
        yield break;
    }
}
