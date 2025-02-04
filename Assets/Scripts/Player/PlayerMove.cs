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
    private float jumpingPower = 25f;//점프 높이

    //플레이어 로프 이동
    private HingeJoint2D joint;
    private bool isOnRope = false;
    HingeJoint2D linkedHinge;
    [SerializeField] private float ropeForce = 15f;
    float ropeCooltime = 0.1f;
    bool ableRope = false;

    //플레이어 대쉬
    //private bool isDash = false;
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
    private PlayerHP playerHP; // PlayerHP 참조 변수 추가
    Rigidbody2D rigid;
   

    //패링
    bool isparrying = false;
    private float parryingCoolTime = 0.5f;
    bool successParrying = false;
    float DamageUpTime = 1f;
    public GameObject shield;//임시 모션

    private SpriteRenderer spriteRenderer;

    private float originalGravityScale; //대시 중력

    Animator anim;

    private void Start()
    {
        joint = GetComponent<HingeJoint2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 초기화
        rigid = GetComponent<Rigidbody2D>(); // Rigidbody2D 초기화
        playerHP = GetComponent<PlayerHP>();
        originalGravityScale = rigid.gravityScale;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeySetting.Keys[KeyAction.LEFT]))//기본 좌우 이동
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

       

        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.UP]) && IsGrounded())//기본 점프
        {
            rb.velocity += new Vector2(0, jumpingPower);
            anim.SetTrigger("JumpStart"); // 트리거 사용
        }

        float yVelocity = rb.velocity.y;
        anim.SetFloat("yVelocity", yVelocity);

        if (!IsGrounded()) // 공중에 있는 동안
        {
            if (yVelocity > 0.1f)
            {
                anim.SetBool("IsJump", true); // 점프 중
            }
            else if (yVelocity < -0.1f)
            {
                anim.SetBool("IsFalling", true); // 낙하 중
            }
        }
        else // 착지 시
        {
            anim.SetBool("IsJump", false);
            anim.SetBool("IsFalling", false);
            anim.SetTrigger("JumpOver"); // 착지 애니메이션
        }

        /*
        if (Input.GetKeyUp(KeySetting.Keys[KeyAction.UP]) && rb.velocity.y > 0f)
        {
            //rb.velocity += new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
<<<<<<< Updated upstream
        }
        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.DASH]) && Time.time >= lastDashTime + dashCooldown)
=======
        }*/



        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.DASH]) && Time.time >= lastDashTime + dashCooldown )//대쉬
        {
            //StartCoroutine(dash());
            StartDash();
        }
        if (isDashing && Time.time >= dashTime)
        {
            EndDash();
        }


        if (Input.GetKey(KeySetting.Keys[KeyAction.UP]) && isOnRope)//로프 올라가기
        {
            if (!ableRope)
            {
                StartCoroutine(UpRope());
            }
        }
        if (Input.GetKey(KeySetting.Keys[KeyAction.DOWN]) && isOnRope)//로프 내려가기
        {
            if (!ableRope)
            {
                StartCoroutine(DownRope());
            }
        }
        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.INTERACTION]) && isOnRope)//로프 나오기
        {
            isOnRope = false;
            joint.enabled = false;
            //rb.velocity+=new Vector2(rb.velocity.x, rb.velocity.y);
            rb.velocity += rb.velocity.normalized * rb.velocity.magnitude * 1.5f;//1.5f는 반동 계수

        }

        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.PARRYING]) && !isparrying) //패링
        {

            StartCoroutine(Parrying());
        }

        Flip();

        if(rigid.velocity.normalized.x == 0)
        {
            anim.SetBool("IsRun", false);
        }
        else
        {
            anim.SetBool("IsRun", true);
        }

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

        rigid.velocity = Vector2.zero;
        rigid.velocity += new Vector2(dashDirection.x * dashSpeed * 4f, 0); // 대시 속도 적용

        rigid.gravityScale = 0; // 중력 비활성화
        IgnoreEnemyCollision(true); // Enemy와의 충돌 비활성화
    }

    private void EndDash()
    {
        isDashing = false;
        rigid.velocity -= new Vector2(dashDirection.x * dashSpeed * 3f, 0); // 대시 종료 시 속도 감소
        rigid.gravityScale = originalGravityScale; // 원래 중력값 복구
        IgnoreEnemyCollision(false); // Enemy와의 충돌 활성화
    }

    private void IgnoreEnemyCollision(bool ignore)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (playerLayer == -1 || enemyLayer == -1)
        {
            Debug.LogError("Layer names 'Player' or 'Enemy' are not defined in the Tags and Layers settings.");
            return;
        }

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, ignore);
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
            }
        }


    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    /* private void Flip()
     {
         if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
         {
             isFacingRight = !isFacingRight;
             Vector3 localScale = transform.localScale;
             localScale.x *= -1f;
             transform.localScale = localScale;

         }
     }
    */

    private void Flip()
    {
        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;

            // Flip 상태 변경 시 애니메이션 업데이트
            anim.SetBool("IsRun", true); // 애니메이션 상태 전환
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
        if (rigid == null)
        {
            Debug.LogError("Rigidbody2D not found on Player!");
        }
    }
    public void SetUp(float amount)
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }

    public void OnDamaged(Vector2 targetPos)
    {
        if (gameObject.layer == LayerMask.NameToLayer("PlayerDamaged"))
        {
            // 이미 무적 상태인 경우 처리하지 않음
            return;
        }

        gameObject.layer = LayerMask.NameToLayer("PlayerDamaged"); // 무적 레이어 설정
        spriteRenderer.color = new Color(1, 1, 1, 0.3f); // 무적 상태 시 투명도 변경

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1; // 넉백 방향 결정
        rigid.AddForce(new Vector2(dirc, 2) * 5, ForceMode2D.Impulse); // 넉백 적용

        StartCoroutine(HandleTemporaryInvincibility(1.5f)); // 무적 상태 관리 코루틴 호출
    }

    void OffDamaged()
    {
        gameObject.layer = LayerMask.NameToLayer("Player"); // 무적 레이어 해제
        spriteRenderer.color = new Color(1, 1, 1, 1); // 원래 상태로 복구
    }

    IEnumerator HandleTemporaryInvincibility(float duration)
    {
        int playerLayer = LayerMask.NameToLayer("PlayerDamaged");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (playerLayer == -1 || enemyLayer == -1)
        {
            Debug.LogError("Layer names 'PlayerDamaged' or 'Enemy' are not defined in the Tags and Layers settings.");
            yield break;
        }

        // 충돌을 무시하도록 설정
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        // 무적 타이머
        yield return new WaitForSeconds(duration);

        // 충돌 다시 활성화
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

        OffDamaged(); // 무적 해제
    }


    IEnumerator TemporarilyIgnoreEnemyCollision(float duration)
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");

        if (playerLayer == -1 || enemyLayer == -1)
        {
            Debug.LogError("Layer names 'Player' or 'Enemy' are not defined in the Tags and Layers settings.");
            yield break;
        }

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true); // 플레이어와 적의 충돌 무시
        yield return new WaitForSeconds(duration);
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false); // 충돌 복구
    }




    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            EndDash();
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 targetPos = collision.transform.position;

            // 넉백 먼저 적용
            OnDamaged(targetPos);

            // PlayerHP의 TakeDamage 호출
            playerHP.TakeDamage(1, targetPos); // 대미지 처리 및 넉백 적용
        }
    }

}