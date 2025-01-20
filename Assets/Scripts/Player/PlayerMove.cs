using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //플레이어 좌우 이동
    [SerializeField] private float speed = 2f;//플레이어 스피드
    private float moveInput = 0f;//플레이어 좌우이동 input
    private bool isFacingRight = true;//좌우 처다보는것
    //플레이어 점프
    private float jumpingPower = 15f;//점프 높이

    //플레이어 로프 이동
    private HingeJoint2D joint;
    private bool isOnRope = false;
    HingeJoint2D linkedHinge;
    [SerializeField] private float ropeForce = 15f;
    float ropeCooltime = 0.1f;
    bool ableRope = false;

    //플레이어 대쉬
    private bool isDash = false;
    private bool canDash = true;

    [Header("Dash Settings")]
    [SerializeField] private float dashDuration = 0.2f;//대쉬 지속시간
    [SerializeField] private float dashCoolTime = 2.0f;//대쉬 쿨타임
    [SerializeField] private float dashSpeed = 20.0f;//대쉬 속도

    
    public float dashCooldown = 1f; // 대시 재사용 대기 시간
    private Vector2 dashDirection;

    private bool isDashing = false;
    private float dashTime;
    private float lastDashTime;
    //그외
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //현재 체력
    [SerializeField] private float curHealth;
    //최대 체력
    [SerializeField] public float maxHealth;
    //HP 설정
    public Slider HpBarSlider;
    Rigidbody2D rigid;

    //패링
    bool isparrying = false;
    private float parryingCoolTime = 0.5f;
    bool successParrying=false;
    float DamageUpTime = 1f;
    public GameObject shield;//임시 모션

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        joint = GetComponent<HingeJoint2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 초기화
        rigid = GetComponent<Rigidbody2D>(); // Rigidbody2D 초기화
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeySetting.Keys[KeyAction.LEFT]))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeySetting.Keys[KeyAction.RIGHT]))
        {
            moveInput = 1f;
        }
        else
        {
            moveInput = 0f;
        }
        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.UP]) && IsGrounded())
        {
            rb.velocity += new Vector2(0, jumpingPower);
        }
        if (Input.GetKey(KeySetting.Keys[KeyAction.UP]) && isOnRope)
        {
            if (!ableRope)
            {
                StartCoroutine(UpRope());
            }
        }
        if (Input.GetKey(KeySetting.Keys[KeyAction.DOWN]) && isOnRope)
        {
            if (!ableRope)
            {
                StartCoroutine(DownRope());
            }
        }
        if (Input.GetKeyUp(KeySetting.Keys[KeyAction.UP]) && rb.velocity.y > 0f)
        {
            //rb.velocity += new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.DASH]) && Time.time >= lastDashTime + dashCooldown )
        {
            //StartCoroutine(dash());
            StartDash();
        }
        if (isDashing && Time.time >= dashTime)
        {
            EndDash();
        }
        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.INTERACTION]) && isOnRope)
        {
            isOnRope = false;
            joint.enabled = false;
            //rb.velocity+=new Vector2(rb.velocity.x, rb.velocity.y);
            rb.velocity += rb.velocity.normalized * rb.velocity.magnitude * 1.5f;//1.5f는 반동 계수
        }
        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.PARRYING]) && !isparrying) 
        {

            StartCoroutine(Parrying());
        }

        Flip();
    }

    private void StartDash()
    {
        isDashing = true;
        dashTime = Time.time + dashDuration;
        lastDashTime = Time.time;

        // 대시 방향 설정 (현재 이동 방향 기준)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput == 0 && verticalInput == 0)
        {
            // 대시 방향이 없으면 마지막 이동 방향으로 설정
            dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            dashDirection = new Vector2(horizontalInput, verticalInput).normalized;
        }
        rb.velocity = Vector2.zero;
        rb.velocity += new Vector2( dashDirection.x * dashSpeed*4f,0); // 대시 속도 적용
    }

    private void EndDash()
    {
        isDashing = false;
        //rb.velocity = Vector2.zero; // 대시 후 정지
        rb.velocity -= new Vector2(dashDirection.x * dashSpeed * 3f, 0);
    }

    IEnumerator Parrying()
    {
        isparrying = true;
        //일정 시간동안 패링 true인 상황에서 PlayerHp에 있는TakeDamage함수가 실행된다면
        //isparrying이 false로 바뀌고 공격력을 1.5초간 올림
        shield.SetActive(true);
        yield return new WaitForSeconds(parryingCoolTime);
        shield.SetActive(false);
        isparrying = false;
    }
    public bool GetParrying()
    {
        return isparrying;
    }
    public bool GetSuccessParrying()
    {
        return successParrying;
    }
    public IEnumerator ParryingSuccess()
    {
        Debug.Log("패링 성공");
        successParrying = true;
        shield.SetActive(false);
        isparrying = false;
        //animator.SetBool("IsParrying",false);
        yield return new WaitForSeconds(DamageUpTime);
        successParrying = false;
    }
    IEnumerator UpRope()
    {
        
        if (Rope.FindHead(linkedHinge) != linkedHinge.connectedBody)
        {
            ableRope = true;
            Rigidbody2D connectedRigidbody = linkedHinge.connectedBody;
            //현재 연결되어있는 오브젝트(1)에서 오브젝트(1)을 잡고있는 오브젝트(2)를 구함
            joint.connectedBody = connectedRigidbody;//오브젝트(2)에 플레이어를 연결

            joint.anchor = new Vector2(0, 0.5f);//플레이어의 anchor를 오브젝트의 아랫부분으로 연결
            joint.connectedAnchor = new Vector2(0, -0.5f);
            linkedHinge = connectedRigidbody.GetComponent<HingeJoint2D>();
            //현재 연결 된 오브젝트(2)를 오브젝트(1)이 있던 변수에 덮어씌움
        }
        yield return new WaitForSeconds(ropeCooltime);
        ableRope = false;
    }
    IEnumerator DownRope()
    {
        ableRope = true;
        Rigidbody2D connectedRigidbody = Rope.FindBefore(linkedHinge);
        //연결되어있는 오브젝트(1)이 잡고있는 오브젝트(0)를 구함
        joint.connectedBody = connectedRigidbody;//오브젝트(0)에 플레이어를 연결

        joint.anchor = new Vector2(0, 0.5f);//플레이어의 anchor를 오브젝트의 아랫부분으로 연결
        joint.connectedAnchor = new Vector2(0, -0.5f);
        linkedHinge = connectedRigidbody.GetComponent<HingeJoint2D>();
        //현재 연결 된 오브젝트(0)를 오브젝트(1)이 있던 변수에 덮어씌움
        yield return new WaitForSeconds(ropeCooltime);
        ableRope = false;
    }
    
    private void FixedUpdate()
    {

        if (isOnRope)
        {
            rb.AddForce(new Vector2(ropeForce * moveInput, 0f));
        }

        else if (rb.velocity.x >= -speed && rb.velocity.x <= speed)
        {
            if (moveInput != 0)
            {
                rb.velocity += new Vector2(moveInput * speed / 8, 0);
                Debug.Log(rb.velocity.x);
            }
        }
        

    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Rope") && !isOnRope && Input.GetKey(KeySetting.Keys[KeyAction.UP]))
        {
            joint.enabled = true;
            Rigidbody2D ropeRb = coll.GetComponent<Rigidbody2D>();
            joint.connectedBody = ropeRb;

            joint.anchor = new Vector2(0, 0.5f);
            joint.connectedAnchor = new Vector2(0, -0.5f);

            isOnRope = true;
            linkedHinge = coll.GetComponent<HingeJoint2D>();


        }
    }

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void SetUp(float amount)
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }
   
    public void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDamaged"); // 무적 레이어
        
        spriteRenderer.color = new Color(1, 1, 1, 0.3f);
      
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1; //넉백
        rigid.AddForce(new Vector2(dirc, 2) * 5, ForceMode2D.Impulse);

        Invoke("OffDamaged", 0.5f); //무적해제
    }
    void OffDamaged()
    {
        //무적판정 풀림
        gameObject.layer = LayerMask.NameToLayer("Player"); ; // 무적 레이어 해제
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 대시 중 벽에 충돌하면 대시 종료
        if (isDashing)
        {
            EndDash();
        }
    }

}
