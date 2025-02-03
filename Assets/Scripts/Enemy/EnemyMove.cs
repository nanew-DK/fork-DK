using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected SpriteRenderer render;
    [SerializeField] protected BoxCollider2D boxCollider;

    [SerializeField] protected float speed = 2.5f;
    [SerializeField] protected float followDistance = 5f;    // 추격 시작 거리
    [SerializeField] protected float stopChaseRange = 2f;    // 추적 멈출 거리 (공격 준비 거리)
    [SerializeField] protected int Hp = 3;
    [SerializeField] protected float knockbackForce = 10f;   // 넉백 힘

    protected Transform player;
    protected bool isPlayerOnSamePlatform;
    protected bool isChasing;
    protected int nextMove;
    protected Animator anim; // 애니메이션 추가

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>(); // BoxCollider2D 컴포넌트 가져오기
        player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>(); // Animator 가져오기
        Think(); // 초기 이동 방향 설정
    }

    protected virtual void FixedUpdate()
    {
        CheckPlatform(); // 플레이어와 같은 플랫폼에 있는지 확인

        if (isPlayerOnSamePlatform && Vector2.Distance(transform.position, player.position) <= followDistance)
        {
            if (Vector2.Distance(transform.position, player.position) > stopChaseRange)
            {
                StartMoving();
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
            StopMoving();
        }
        else if (!isChasing)
        {
            StartMoving();
            Patrol(); // 정찰 상태
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
            {
                playerScript.TakeDamage(1, this.transform.position);
            }
        }
    }

    protected void Think()
    {
        StopMoving();
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

    // 충돌 처리 (플레이어의 공격)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            TakeDamage(1); // 데미지 1
        }
    }

    // 데미지 처리 및 사망
    public virtual void TakeDamage(int damage)
    {
        Hp -= damage;

        Debug.Log("적이 데미지를 받음. 현재 HP: " + Hp);

        // 넉백 방향 계산
        Vector2 knockbackDirection = (transform.position - player.position).normalized;

        // 넉백 적용
        rigid.velocity = Vector2.zero; // 현재 속도 초기화
        rigid.AddForce(new Vector2(knockbackDirection.x * knockbackForce, rigid.velocity.y), ForceMode2D.Impulse);

        if (Hp <= 0)
        {
            Debug.Log("적이 사망했습니다.");
            Destroy(this.gameObject);
        }
    }

    // 슬로우 효과
    public IEnumerator Slow()
    {
        speed -= 1.5f;
        yield return new WaitForSeconds(1.5f);
        speed += 1.5f;
    }

    // 버프 효과
    public IEnumerator Buff()
    {
        speed = 6f;
        yield return new WaitForSeconds(3f);
        speed = 2.5f;
    }

    //이동 애니메이션 관리
    protected virtual void StartMoving()
    {
        if (anim != null)
            anim.SetBool("isMoving", true);
    }

    protected virtual void StopMoving()
    {
        if (anim != null)
            anim.SetBool("isMoving", false);
    }
}