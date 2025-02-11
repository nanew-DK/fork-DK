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
    [Header("기본 이동 관련 변수")]
    [SerializeField] private float speed = 2f;        // 플레이어 이동 속도
    private float moveInput = 0f;                     // 좌우 이동 입력값
    private bool isFacingRight = true;                // 캐릭터가 오른쪽을 보고 있는지 여부
    private float jumpingPower = 25f;                 // 점프력

    [Header("로프 관련 변수")]
    private HingeJoint2D joint;                       // 로프에 매달리기 위한 조인트
    private bool isOnRope = false;                    // 로프에 매달려있는지 여부
    HingeJoint2D linkedHinge;                         // 연결된 로프의 힌지 조인트
    [SerializeField] private float ropeForce = 15f;   // 로프에서 움직일 때 가해지는 힘
    float ropeCooltime = 0.1f;                        // 로프 동작 쿨타임
    bool ableRope = false;                            // 로프 사용 가능 여부

    [Header("대시 관련 변수")]
    private bool canDash = true;                      // 대시 가능 여부
    [SerializeField] private float dashDuration = 0.2f;// 대시 지속시간
    [SerializeField] private float dashCoolTime = 2.0f;// 대시 쿨타임
    [SerializeField] private float dashSpeed = 20.0f;  // 대시 속도
    public float dashCooldown = 1f;                   // 대시 쿨다운 시간
    private Vector2 dashDirection;                     // 대시 방향
    private bool isDashing = false;                    // 현재 대시 중인지 여부
    private float dashTime;                           // 대시 타이머
    private float lastDashTime;                       // 마지막 대시 시간

    [Header("기타 컴포넌트")]
    [SerializeField] private Rigidbody2D rb;          // 리지드바디 컴포넌트
    [SerializeField] private Transform groundCheck;    // 지면 체크 위치
    [SerializeField] private LayerMask groundLayer;    // 지면 레이어

    [Header("체력 관련 변수")]
    [SerializeField] private float curHealth;          // 현재 체력
    [SerializeField] public float maxHealth;           // 최대 체력
    private PlayerHP playerHP;                         // PlayerHP 스크립트 참조

    [Header("패링 관련 변수")]
    bool isparrying = false;                          // 패링 중인지 여부
    private float parryingCoolTime = 0.5f;            // 패링 쿨타임
    bool successParrying = false;                     // 패링 성공 여부
    float DamageUpTime = 1f;                         // 패링 성공 후 데미지 증가 시간
    public GameObject shield;                         // 방패 오브젝트

    [Header("동료스킬 관련 변수")]
    private int skillUnlock = 1;
    public SkillEffect skillPanel;
    public GameObject skillRange;
    public GameObject mainCamera;
    private bool canUseSkill = true;
    private float lastSkillTime = -10f;
    private float skillCooldown = 10f;

    private SpriteRenderer spriteRenderer;

    private float originalGravityScale; // 원래 중력 스케일

    public Animator anim;

    private void Start()
    {
        // 컴포넌트 초기화
        joint = GetComponent<HingeJoint2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerHP = GetComponent<PlayerHP>();
        originalGravityScale = rb.gravityScale;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeySetting.Keys[KeyAction.LEFT]))//기본 점프
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

        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.SKILL_1]))
        {
            DoSkill();
        }


        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.UP]) && IsGrounded())//기본 점프
        {
            rb.velocity += new Vector2(0, jumpingPower);
            anim.SetBool("IsJump", true);
            anim.SetBool("IsGrounded", false);
            anim.SetTrigger("JumpStart");
        }

        float yVelocity = rb.velocity.y;
        anim.SetFloat("yVelocity", yVelocity);

        if (!IsGrounded()) // 공중에 있는 상태
        {
            anim.SetBool("IsGrounded", false);
            if (yVelocity > 0.1f)
            {
                anim.SetBool("IsJump", true);
            }
            else if (yVelocity < -0.1f)
            {
                anim.SetBool("IsJump", false);
            }
        }
        else // 착지 시
        {
            anim.SetBool("IsGrounded", true);
            anim.SetBool("IsJump", false);
        }

        /*
        if (Input.GetKeyUp(KeySetting.Keys[KeyAction.UP]) && rb.velocity.y > 0f)
        {
            //rb.velocity += new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.DASH]) && Time.time >= lastDashTime + dashCooldown)
        */



        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.DASH]) && Time.time >= lastDashTime + dashCooldown)//대시
        {
            //StartCoroutine(dash());
            StartDash();
        }
        if (isDashing && Time.time >= dashTime)
        {
            EndDash();
        }


        if (Input.GetKey(KeySetting.Keys[KeyAction.UP]) && isOnRope)//로프 오버
        {
            if (!ableRope)
            {
                StartCoroutine(UpRope());
            }
        }
        if (Input.GetKey(KeySetting.Keys[KeyAction.DOWN]) && isOnRope)//로프 아래로 이동
        {
            if (!ableRope)
            {
                StartCoroutine(DownRope());
            }
        }
        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.INTERACTION]) && isOnRope)//로프 끊기
        {
            isOnRope = false;
            anim.SetBool("isSwing", false);

            joint.enabled = false;
            //rb.velocity+=new Vector2(rb.velocity.x, rb.velocity.y);
            rb.velocity += rb.velocity.normalized * rb.velocity.magnitude * 1.5f;//1.5f를 곱해서 더 멀리 보내기

        }

        if (Input.GetKeyDown(KeySetting.Keys[KeyAction.PARRYING]) && !isparrying) //패링
        {

            StartCoroutine(Parrying());
        }

        Flip();

        if (rb.velocity.normalized.x == 0)
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
        // 대시 시작 시 설정
        isDashing = true;
        dashTime = Time.time + dashDuration;
        lastDashTime = Time.time;

        // 대시 방향 결정 (입력 방향 기준)
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput == 0 && verticalInput == 0)
        {
            // 대시 방향이 결정되지 않았을 때 기본 방향으로 설정
            dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            dashDirection = new Vector2(horizontalInput, verticalInput).normalized;
        }

        rb.velocity = Vector2.zero;
        rb.velocity += new Vector2(dashDirection.x * dashSpeed * 4f, 0); // 대시 속도 적용

        rb.gravityScale = 0; // 중력 무시
        IgnoreEnemyCollision(true); // Enemy 충돌 무시
    }

    private void EndDash()
    {
        isDashing = false;
        rb.velocity -= new Vector2(dashDirection.x * dashSpeed * 3f, 0); // 대시 끝날 때 속도 줄이기
        rb.gravityScale = originalGravityScale; // 원래 중력 스케일로 복귀
        IgnoreEnemyCollision(false); // Enemy 충돌 복귀
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

    private void DoSkill()
    {
        if (!canUseSkill || Time.time < lastSkillTime + skillCooldown)
        {
            Debug.Log("스킬 쿨타임 중!");
            return;
        }

        canUseSkill = false;
        lastSkillTime = Time.time;

        skillPanel.PlaySkillEffect();
        Debug.Log("doing skill");
        CameraMove cameraMove = mainCamera.GetComponent<CameraMove>();
        cameraMove.StartShake();
        GameObject StunRange = Instantiate(skillRange, transform.position, Quaternion.identity);
        Destroy(StunRange, 0.1f);

        StartCoroutine(ResetSkillCooldown());
    }

    private IEnumerator ResetSkillCooldown()
    {
        yield return new WaitForSeconds(skillCooldown);
        canUseSkill = true;
    }

    IEnumerator Parrying()
    {
        isparrying = true;
        //패링 지속 중인지 확인하고 PlayerHp에 TakeDamage 스크립트가 있는지 확인
        //isparrying이 false가 되면 1.5초 후 다시 확인
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
            //로프 연결된 조인트(1)를 로프 연결된 조인트(2)에 연결
            joint.connectedBody = connectedRigidbody;//로프 연결된 조인트(2)를 플레이어에 연결

            joint.anchor = new Vector2(0, 0.5f);//플레이어의 anchor를 로프 연결된 조인트에 연결
            joint.connectedAnchor = new Vector2(0, -0.5f);
            linkedHinge = connectedRigidbody.GetComponent<HingeJoint2D>();
            //로프 연결된 조인트(2)를 로프 연결된 조인트(1)에 연결
        }
        yield return new WaitForSeconds(ropeCooltime);
        ableRope = false;
    }
    IEnumerator DownRope()
    {
        ableRope = true;
        Rigidbody2D connectedRigidbody = Rope.FindBefore(linkedHinge);
        //로프 연결된 조인트(1)를 로프 연결된 조인트(0)에 연결
        joint.connectedBody = connectedRigidbody;//로프 연결된 조인트(0)를 플레이어에 연결

        joint.anchor = new Vector2(0, 0.5f);//플레이어의 anchor를 로프 연결된 조인트에 연결
        joint.connectedAnchor = new Vector2(0, -0.5f);
        linkedHinge = connectedRigidbody.GetComponent<HingeJoint2D>();
        //로프 연결된 조인트(0)를 로프 연결된 조인트(1)에 연결
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

            // Flip 애니메이션을 재생하고 로프 애니메이션을 중지
            anim.SetBool("IsRun", true); // 로프 애니메이션 재생
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

            anim.SetBool("isSwing", true);
        }
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
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
            // 이미 처리된 충돌은 무시
            return;
        }

        gameObject.layer = LayerMask.NameToLayer("PlayerDamaged"); // 플레이어 레이어 변경
        spriteRenderer.color = new Color(1, 1, 1, 0.3f); // 플레이어 색상 변경

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1; // 플레이어 방향 결정
        rb.AddForce(new Vector2(dirc, 2) * 5, ForceMode2D.Impulse); // 플레이어 힘 가하기

        StartCoroutine(HandleTemporaryInvincibility(1.5f)); // 임시 무적 지속
    }

    void OffDamaged()
    {
        gameObject.layer = LayerMask.NameToLayer("Player"); // 플레이어 레이어 복귀
        spriteRenderer.color = new Color(1, 1, 1, 1); // 플레이어 색상 복귀
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

        // 충돌 무시 설정
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        // 충돌 무시 지속
        yield return new WaitForSeconds(duration);

        // 충돌 무시 복귀
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

        OffDamaged(); // 임시 무적 복귀
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

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true); // 플레이어 충돌 무시
        yield return new WaitForSeconds(duration);
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false); // 충돌 복귀
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

            // 플레이어 충돌 처리
            OnDamaged(targetPos);

            // PlayerHP에 TakeDamage 호출
            playerHP.TakeDamage(1, targetPos); // 충돌 후 플레이어 체력 감소
        }
    }

}