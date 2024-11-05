using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private bool isDash=false;
    private bool canDash = true;
    public float dashDuration = 0.2f;

    [SerializeField] private float dashSpeed = 10.0f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float dashCoolTime = 2.0f;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.UpArrow) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !canDash) 
        {
            StartCoroutine(dash());
        }

        Flip();
    }

    IEnumerator dash()
    {
        isDash = true;
        canDash = false;
        
        Debug.Log("Dash!");
        
        //rb.AddForce(new Vector2(horizontal* dashSpeed, 1f), ForceMode2D.Impulse);  // 대시 힘 가하기

        float dashDirection = transform.localScale.x > 0 ? 1 : -1;
        rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);

        yield return new WaitForSeconds(dashDuration);
        isDash = false;
        yield return new WaitForSeconds(dashCoolTime);
        canDash = true;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
    }
}


