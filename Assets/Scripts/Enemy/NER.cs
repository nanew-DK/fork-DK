using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NER : EnemyMove
{
    public GameObject attackRange;
    public GameObject NER_bullet;
    private GameObject target;

    private CircleCollider2D rangeCollider;
    private BoxCollider2D takeDamageCollider;
    private SpriteRenderer spriteRenderer;

    private float attackTurm = 0f;
    private bool canAttack;
    private bool Wait = true;

    //���
    private void Update()
    {
        if (isStunned == true) return;
        anim.speed = 1f * 2f / attackSpeed;
        if (canAttack && Wait)
        {
            Attack();
            StartCoroutine("WaitAttack");
        }
        if (target != null)
        {
            spriteRenderer.flipX = target.transform.position.x < transform.position.x;
        }
    }
    //��Ÿ� �ޱ�
    void Start()
    {
        attackSpeed = 2f;
        rangeCollider = attackRange.GetComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
        takeDamageCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    //�÷��̾� ����
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            target = collision.gameObject;
            anim.SetBool("isReady", true);
            StopMoving();
            speed = 0;
            StartCoroutine("Aim");
        }
    }
    //�÷��̾� ��ħ
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            target = null;
            StopMoving();
            anim.SetBool("isReady", false);
            canAttack = false;
            speed = 2.5f;
            StopCoroutine("Aim");
        }
    }
    //���ؽð�
    private IEnumerator Aim()
    {
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
    //���� ��
    private IEnumerator WaitAttack()
    {
        Wait = false;
        yield return new WaitForSeconds(attackSpeed);
        Wait = true;
    }
    //���� ����
    private void Attack()
    {
        anim.SetTrigger("isTrigger");
        Vector3 directionToPlayer = target.transform.position - transform.position;
        directionToPlayer.z = 0f;

        GameObject cpy_bullet = Instantiate(NER_bullet, transform.position, transform.rotation);
        Destroy(cpy_bullet, 5f);

        NER_bullet bulletComponent = cpy_bullet.GetComponent<NER_bullet>();
        bulletComponent.SetDirection(directionToPlayer);
    }
    //�̵� �ִϸ��̼� ����
    protected override void StartMoving()
    {
        if (anim != null)
        {
            anim.SetBool("isMoving", true);
        }
    }

    protected override void StopMoving()
    {
        if (anim != null)
        {
            anim.SetBool("isMoving", false);
        }
    }
}
