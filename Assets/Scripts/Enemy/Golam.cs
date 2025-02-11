using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golam : EnemyMove
{
    [SerializeField] private int attackPower; // ���ݷ� ����
    [SerializeField] public BoxCollider2D attackRange; // ���� ���� ����
    [SerializeField] private float attackDelay = 2f; // ���� ������
    [SerializeField] private float pushBackForce = 5f;

    private bool canAttack = true;

    protected override void Awake()
    {
        base.Awake(); // �θ� Ŭ������ Awake �޼��带 ȣ��
    }

    protected override void FixedUpdate()
    {
        if (isStunned) return;


        base.CheckPlatform(); // �θ� Ŭ������ �÷��� Ȯ�� �޼��� ȣ��

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
        render.flipX = player.position.x < transform.position.x; // �÷��̾� �������� �ٶ󺸱�

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
        Debug.Log("���� �÷��̾ �����մϴ�!");

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
                Debug.Log("�÷��̾ ���� �����̹Ƿ� �������� �ʽ��ϴ�.");
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
