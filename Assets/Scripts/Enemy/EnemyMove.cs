using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected SpriteRenderer render;
    [SerializeField] protected BoxCollider2D boxCollider;

    [SerializeField] protected float speed = 2.5f;
    [SerializeField] protected float attackSpeed = 2f;
    [SerializeField] protected float followDistance = 5f;    // �߰� ���� �Ÿ�
    [SerializeField] protected float stopChaseRange = 2f;    // ���� ���� �Ÿ� (���� �غ� �Ÿ�)
    [SerializeField] protected int Hp = 3;
    [SerializeField] protected float knockbackForce = 10f;   // �˹� ��

    protected Transform player;
    protected bool isPlayerOnSamePlatform;
    protected bool isChasing;
    protected bool isStunned;
    protected bool isHoldingPosition;
    protected int nextMove;
    protected Animator anim; // �ִϸ��̼� �߰�
    protected Vector2 previousPosition;

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>(); // BoxCollider2D ������Ʈ ��������
        player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>(); // Animator ��������
        Think(); // �ʱ� �̵� ���� ����
        previousPosition = transform.position;
    }
    protected virtual void FixedUpdate()
    {
        if (isStunned == true) return;
        StartMoving();
        CheckPlatform(); // �÷��̾�� ���� �÷����� �ִ��� Ȯ��

        if (isPlayerOnSamePlatform && Vector2.Distance(transform.position, player.position) <= followDistance)
        {
            if (Vector2.Distance(transform.position, player.position) > stopChaseRange)
            {
                ChasePlayer(); // ���� �Ÿ� �̻��� �� �߰�
            }
            else
            {
                StopAndPrepareAttack(); // ���� ���� ���� �����ϸ� ����
            }
        }
        else if (isChasing && Vector2.Distance(transform.position, player.position) > followDistance)
        {
            StopChasing(); // ���� ����
        }
        else if (!isChasing)
        {
            Patrol(); // ���� ����
        }
    }

    protected virtual void Patrol()
    {
        rigid.velocity = new Vector2(nextMove * speed, rigid.velocity.y);
        Vector2 frontVector = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVector, Vector3.down, Color.green);

        RaycastHit2D rayHit = Physics2D.Raycast(frontVector, Vector3.down, 6f, LayerMask.GetMask("Ground"));

        if (rayHit.collider == null)
        {
            Turn();
        }
    }

    protected void ChasePlayer()
    {
        if (isStunned == true) return;
        isChasing = true;
        Vector2 direction = (player.position - transform.position).normalized;
        rigid.velocity = new Vector2(direction.x * speed, rigid.velocity.y);
        render.flipX = direction.x < 0;
    }

    protected void StopChasing()
    {
        isChasing = false;
        Think(); // ���� ���·� ��ȯ
    }

    protected virtual void StopAndPrepareAttack()
    {
        rigid.velocity = Vector2.zero; // �� ����
        Debug.Log("�� �÷��̾� ����");

        // �÷��̾ ��ó�� �ִ��� Ȯ��
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
        if (isStunned == true) return;
        nextMove = (Random.Range(0, 2) * 2) - 1;
        render.flipX = nextMove == -1;
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime); // ���� �ð� �� ���� ����
    }

    protected void Turn()
    {
        if (isStunned == true) return;
        nextMove *= -1;
        render.flipX = nextMove == -1;
    }

    protected void CheckPlatform()
    {
        isPlayerOnSamePlatform = Mathf.Abs(player.position.y - transform.position.y) < 0.5f;
    }

    // �浹 ó�� (�÷��̾��� ����)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerAttack"))
        {
            TakeDamage(1); // ������ 1
        }
    }

    // ������ ó�� �� ���
    public virtual void TakeDamage(int damage)
    {
        Hp -= damage;

        Debug.Log("���� �������� ����. ���� HP: " + Hp);

        // �˹� ���� ���
        Vector2 knockbackDirection = (transform.position - player.position).normalized;

        // �˹� ����
        rigid.velocity = Vector2.zero; // ���� �ӵ� �ʱ�ȭ
        rigid.AddForce(new Vector2(knockbackDirection.x * knockbackForce, rigid.velocity.y), ForceMode2D.Impulse);

        if (Hp <= 0)
        {
            Debug.Log("���� ����߽��ϴ�.");
            StartCoroutine(FadeOutAndDestroy()); // ���� ���� �� ����
        }
    }

    // ���� ���߸鼭 �� ����
    private IEnumerator FadeOutAndDestroy()
    {
        float fadeDuration = 1.5f; // ����ȭ �ӵ�
        float elapsedTime = 0f;
        Color originalColor = render.color;

        // ���� ����ϸ� �ݶ��̴��� �̵� ���߱�
        boxCollider.enabled = false;
        rigid.velocity = Vector2.zero;
        isStunned = true; // �߰������� ��� �ൿ�� ����

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            render.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }

    // ���ο� ȿ��
    public IEnumerator Slow()
    {
        speed -= 1.5f;
        yield return new WaitForSeconds(1.5f);
        speed += 1.5f;
    }

    // ���� ȿ��
    public IEnumerator Buff()
    {
        speed = 6f;
        attackSpeed /= 3f;
        yield return new WaitForSeconds(3f);
        speed = 2.5f;
        attackSpeed *= 3f;
    }

    //�̵� �ִϸ��̼� ����
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

    public void Stun(float duration)
    {
        isStunned = true;
        anim.SetBool("isFaint", true); // ���� �ִϸ��̼� ���

        Invoke(nameof(EndStun), duration);
    }

    private void EndStun()
    {
        isStunned = false;
        anim.SetBool("isFaint", false);
    }
}