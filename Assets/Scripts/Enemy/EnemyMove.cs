using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected SpriteRenderer render;

    [SerializeField] protected float speed = 2.5f;
    [SerializeField] protected float followDistance = 5f;    // 추격 시작 거리
    [SerializeField] protected float stopChaseRange = 2f;    // 추적 멈출 거리 (공격 준비 거리)
    [SerializeField] protected int Hp;

    protected Transform player;
    protected bool isPlayerOnSamePlatform;
    protected bool isChasing;
    protected int nextMove;


    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        Think(); // 초기 이동 방향 설정
    }

    protected virtual void FixedUpdate()
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
            else
            {
                // 공격 범위 내에 도달하면 정지
                StopAndPrepareAttack();
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

    protected virtual void Patrol()
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

    protected virtual void StopAndPrepareAttack()
    {
        rigid.velocity = Vector2.zero; // 적 멈춤
        Debug.Log("적 플레이어 공격");

        // 플레이어가 근처에 있는지 확인
        if (player != null)
        {
            PlayerHP playerScript = player.GetComponent<PlayerHP>();
            if (playerScript != null)
                if (playerScript != null)
                {
                    playerScript.TakeDamage(1, this.transform.position);
                }
        }
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

    // 2024/11/14 남정현 적 hp 및 사망 추가

    // 충돌 감지
    // 이거 istrigger로 가면 사거리가 trigger라서 플레이어가 적 사거리를 때려도 적이 죽어버린다
    // collision으로 가면 총알과 검이 둘 다 trigger 여서 적의 체력이 닳지 않는다


    // 데미지 사망
    public virtual void TakeDamage(int damage)
    {
        Debug.Log("아야");
        Hp -= damage;
        if (Hp <= 0)
            Destroy(this.gameObject);
    }

    // 슬로우
    public IEnumerator Slow()
    {
        speed -= 1.5f;
        yield return new WaitForSeconds(1.5f);
        speed += 1.5f;
    }
    public IEnumerator Buff()
    {
        speed = 6f;
        yield return new WaitForSeconds(3f);
        speed = 2.5f;
    }
}

