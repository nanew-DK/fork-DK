using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedLeg : EnemyMove
{
    [SerializeField] private int attackPower; // ���ݷ� ����
    [SerializeField] public BoxCollider2D attackRange; // ���� ���� ����
    [SerializeField] private float attackDelay = 1.5f; // ���� ������
    [SerializeField] private float pushBackForce = 5f;

    private float attackCooldown = 0f; // ���� ��Ÿ�� ����

    protected override void Awake()
    {
        base.Awake(); // �θ� Ŭ������ Awake �޼��带 ȣ��
    }

    protected override void FixedUpdate()
    {
        attackCooldown -= Time.deltaTime;

        base.CheckPlatform(); // �θ� Ŭ������ �÷��� Ȯ�� �޼��� ȣ��

        if (isPlayerOnSamePlatform && Vector2.Distance(transform.position, player.position) <= followDistance)
        {
            // �÷��̾ ���� ���� ���� ������
            if (Vector2.Distance(transform.position, player.position) < stopChaseRange)
            {
                StopAndPrepareAttack(); // ���� ���� ���� �����ϸ� ����
            }
        }
        else if (!isChasing)
        {
            // ���� ����
            Patrol();
        }
    }

    protected override void Patrol()
    {
        Debug.Log("PAtrol");
        if (player.position.x < this.transform.position.x) { render.flipX = false; }
        else render.flipX=true; 
        
    }

    // StopAndPrepareAttack �޼���� Golam���� ���� �غ� ������ �߰�
    protected override void StopAndPrepareAttack()
    {
        rigid.velocity = Vector2.zero; // ���� ���߰� ��
        render.flipX = player.position.x < transform.position.x; // �÷��̾� �������� �ٶ󺸱�

        if (attackCooldown <= 0) // ��Ÿ���� ���� ��
        {
            StartCoroutine(PrepareAttack()); // ���� �غ�
        }
        else
        {
            Debug.Log("���� ��� ��, ���� ��Ÿ��: " + attackCooldown);
        }
    }

    private IEnumerator PrepareAttack()
    {
        attackCooldown = attackDelay; // ��Ÿ�� �ʱ�ȭ
        Debug.Log("���� �غ� ��...");

        // ���� �غ� �ð� ���� ����
        rigid.velocity = Vector2.zero;

        yield return new WaitForSeconds(attackDelay); // ���� �غ� �ð�

        Debug.Log("���� ����!");
        Attack(); // ���� ����
    }

    private void Attack()
    {
        // �÷��̾ �������� ����
        PlayerHP playerScript = player.GetComponent<PlayerHP>();
        PlayerMove playerMove = player.GetComponent<PlayerMove>();
        if (playerScript != null && playerMove != null)
        {
            // ���� �������� Ȯ��
            if (player.gameObject.layer != LayerMask.NameToLayer("PlayerDamaged"))
            {
                // ���� �������� �÷��̾�� ������ ����
                playerScript.TakeDamage(attackPower, transform.position); // position�� targetpos�� ����
                playerMove.OnDamaged(transform.position); // �˹� �� ���� ���� Ȱ��ȭ
            }
        }
    }


    // ������ ó��
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

}