using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected SpriteRenderer render;

    [SerializeField] protected float speed = 2.5f;
    [SerializeField] protected float followDistance = 5f;    // 추격 시작 거리
    [SerializeField] protected float stopChaseRange = 1.5f;    // 추적 멈출 거리 (공격 준비 거리)
    [SerializeField] protected float Hp = 5f;
    EnemyWeapon weapon;

    Animator animator;
    [SerializeField] float attackTime = 2.5f;

    protected Transform player;
    protected bool isPlayerOnSamePlatform;
    protected bool isChasing;
    protected int nextMove;

    bool ableAttack = true;//true 일 때 공격 가능
    void Awake()
    {
        //rigid = GetComponent<Rigidbody2D>();
        rigid = GetComponentInChildren<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        weapon = this.GetComponentInChildren<EnemyWeapon>();
        animator = GetComponentInChildren<Animator>();
        Think(); // 초기 이동 방향 설정
    }

    void FixedUpdate()
    {
        CheckPlatform(); // 플레이어와 같은 플랫폼에 있는지 확인

        if (isPlayerOnSamePlatform && Vector2.Distance(transform.position, player.position) <= followDistance)
        {
            // 플레이어가 추적 범위 내에 있으면
            if (Vector2.Distance(transform.position, player.position) > stopChaseRange)
            {
                // 공격 거리 이상일 때 추격
                ChasePlayer();
            }
            if (ableAttack)
            {
                MoveStop();
                StartCoroutine(Attack());
            }
        }
        else if (isChasing && Vector2.Distance(transform.position, player.position) > followDistance)
        {
            // 추적 중지
            StopChasing();
        }
        else if (!isChasing)
        {
            // 정찰 상태
            Patrol();
        }
    }

    protected void Patrol()
    {
        rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y);
        Vector2 frontVector = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVector, Vector3.down, Color.green);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVector, Vector3.down, 2f, LayerMask.GetMask("Ground"));

        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    protected void ChasePlayer()
    {
        isChasing = true;
        Vector2 direction = (player.position - transform.position).normalized;
        rigid.velocity = new Vector2(direction.x * speed, rigid.velocity.y);
        render.flipX = direction.x < 0;
    }

    protected void StopChasing()
    {
        isChasing = false;
        Think(); // 정찰 상태로 전환
    }
    protected void MoveStop()
    {
        rigid.velocity = Vector2.zero; // 적 멈춤
    }
    IEnumerator Attack()
    {
        Debug.Log("적 플레이어 공격");
        //연속 공격이 안되게 플래그 켜주기
        ableAttack = false;
        // 플레이어가 근처에 있는지 확인
        animator.SetBool("IsAttack", true);
        yield return new WaitForSeconds(attackTime);
        animator.SetBool("IsAttack", false);
        //공격이 끝나면 끝내기
        ableAttack = true;
    }
    protected void Think()
    {
        nextMove = Random.Range(-1, 2);
        render.flipX = nextMove == -1;
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime); // 일정 시간 후 방향 변경
    }

    protected void Turn()
    {
        nextMove *= -1;
        render.flipX = nextMove == -1;
    }

    protected void CheckPlatform()
    {
        isPlayerOnSamePlatform = Mathf.Abs(player.position.y - transform.position.y) < 0.5f;
    }
    // 데미지 사망
    protected void TakeDamage(float damage)
    {
        Debug.Log("아야");
        Hp -= damage;
        if (Hp <= 0)
            Destroy(this.gameObject);
    }

    // 슬로우
    protected IEnumerator Slow()
    {
        speed -= 1.5f;
        yield return new WaitForSeconds(1.5f);
        speed += 1.5f;
    }
}
