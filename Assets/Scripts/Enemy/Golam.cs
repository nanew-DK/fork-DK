using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golam : EnemyMove
{
    [SerializeField] private int attackPower; // 공격력 설정
    [SerializeField] public BoxCollider2D attackRange; // 공격 범위 설정
    [SerializeField] private float attackDelay = 2f; // 공격 딜레이
    [SerializeField] private float pushBackForce = 5f;

    private bool canAttack = true;

    protected override void Awake()
    {
        base.Awake(); // 부모 클래스의 Awake 메서드를 호출
    }

    protected override void FixedUpdate()
    {
        if (isStunned) return;


        base.CheckPlatform(); // 부모 클래스의 플랫폼 확인 메서드 호출

        if (isPlayerOnSamePlatform && Vector2.Distance(transform.position, player.position) <= followDistance)
        {
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
            StopChasing(); // 추적 중지
        }
        else if (!isChasing)
        {
            Patrol(); // 정찰 상태
        }
    }

    protected override void Patrol()
    {
        StartMoving();
        rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y);
        Vector2 frontVector = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVector, Vector3.down, Color.green);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVector, Vector3.down, 6f, LayerMask.GetMask("Ground"));
        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    protected override void StopAndPrepareAttack()
    {
        if (!canAttack) return;

        speed = 0;
        anim.SetBool("isReady", true);
        render.flipX = player.position.x < transform.position.x; // 플레이어 방향으로 바라보기

        StartCoroutine(WaitAttack());
    }

    private IEnumerator WaitAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackDelay);
        Attack();
        yield return new WaitForSeconds(attackDelay);
        canAttack = true;
    }

    private void Attack()
    {
        if (isStunned)
        {
            anim.SetBool("isReady", false);
            speed = 2.5f;
            return;
        }

        anim.SetTrigger("isAttack");
        Debug.Log("골렘이 플레이어를 공격합니다!");

        PlayerHP playerScript = player.GetComponent<PlayerHP>();
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        if (playerScript != null && playerMove != null)
        {
            if (player.gameObject.layer != LayerMask.NameToLayer("PlayerDamaged"))
            {
                playerScript.TakeDamage(attackPower, transform.position);
                playerMove.OnDamaged(transform.position);
            }
            else
            {
                Debug.Log("플레이어가 무적 상태이므로 공격하지 않습니다.");
            }
        }

        anim.SetBool("isReady", false);
        speed = 2.5f;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}
