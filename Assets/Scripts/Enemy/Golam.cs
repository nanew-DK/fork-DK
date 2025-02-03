using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golam : EnemyMove
{
    [SerializeField] private int attackPower; // 공격력 설정
    [SerializeField] public BoxCollider2D attackRange; // 공격 범위 설정
    [SerializeField] private float attackDelay = 1.5f; // 공격 딜레이
    [SerializeField] private float pushBackForce = 5f;

    private float attackCooldown = 0f; // 현재 쿨타임 상태

    protected override void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake 메서드를 호출
    }

    protected override void FixedUpdate()
    {
        attackCooldown -= Time.deltaTime;

        base.CheckPlatform(); // 부모 클래스의 플랫폼 확인 메서드 호출

        if (isPlayerOnSamePlatform && Vector2.Distance(transform.position, player.position) <= followDistance)
        {
            // 플레이어가 추적 범위 내에 있으면
            if (Vector2.Distance(transform.position, player.position) > stopChaseRange)
            {
                ChasePlayer(); // 공격 거리 이상일 때 추격
            }
            else
            {
                StopAndPrepareAttack(); // 공격 범위 내에 도달하면 정지
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

    // Patrol 메서드는 필요하면 Golam에서만 커스터마이징
    protected override void Patrol()
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

    // StopAndPrepareAttack 메서드는 Golam만의 공격 준비 로직을 추가
    protected override void StopAndPrepareAttack()
    {
        rigid.velocity = Vector2.zero; // 적을 멈추게 함
        render.flipX = player.position.x < transform.position.x; // 플레이어 방향으로 바라보기

        if (attackCooldown <= 0) // 쿨타임이 없을 때
        {
            StartCoroutine(PrepareAttack()); // 공격 준비
        }
        else
        {
            Debug.Log("공격 대기 중, 남은 쿨타임: " + attackCooldown);
        }
    }

    private IEnumerator PrepareAttack()
    {
        attackCooldown = attackDelay; // 쿨타임 초기화
        Debug.Log("공격 준비 중...");

        // 공격 준비 시간 동안 멈춤
        rigid.velocity = Vector2.zero;

        yield return new WaitForSeconds(attackDelay); // 공격 준비 시간

        Debug.Log("공격 시작!");
        Attack(); // 공격 실행
    }

    private void Attack()
    {
        Debug.Log("골렘이 플레이어를 공격합니다!");

        // 플레이어에 데미지를 입힘
        PlayerHP playerScript = player.GetComponent<PlayerHP>();
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        if (playerScript != null && playerMove != null)
        {
            // 무적 상태인지 확인
            if (player.gameObject.layer != LayerMask.NameToLayer("PlayerDamaged"))
            {
                // 공격 범위에서 플레이어에게 데미지 적용
                playerScript.TakeDamage(attackPower, transform.position); // position을 targetpos로 전달
                playerMove.OnDamaged(transform.position); // 넉백 및 무적 상태 활성화
            }
            else
            {
                Debug.Log("플레이어가 무적 상태이므로 공격하지 않습니다.");
            }
        }
    }


    // 데미지 처리
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

}