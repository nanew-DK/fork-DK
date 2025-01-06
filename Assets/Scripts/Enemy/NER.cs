using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NER : EnemyMove
{
    [SerializeField] float attackSpeed = 2f;

    public GameObject attackRange;
    public GameObject NER_bullet;
    public GameObject target;
    private CircleCollider2D rangeCollider;

    private bool canAttack;
    private bool Wait = true;

    //명령
    private void Update()
    {
        if (canAttack && Wait)
        {
            Attack();
            StartCoroutine("WaitAttack");
        }
    }
    //사거리 받기
    void Start()
    {
        rangeCollider = attackRange.GetComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
    }
    //플레이어 감지
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            speed = 0;
            StartCoroutine("Aim");
        }
    }
    //플레이어 놓침
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canAttack = false;
            speed = 2.5f;
            StopCoroutine("Aim");
        }
    }
    //조준시간
    private IEnumerator Aim()
    {
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }
    //공격 텀
    private IEnumerator WaitAttack()
    {
        Wait = false;
        yield return new WaitForSeconds(attackSpeed);
        Wait = true;
    }
    //실제 공격
    private void Attack()
    {
        Vector3 directionToPlayer = target.transform.position - transform.position;
        directionToPlayer.z = 0f;

        GameObject cpy_bullet = Instantiate(NER_bullet, transform.position, transform.rotation);
        Destroy(cpy_bullet, 5f);

        NER_bullet bulletComponent = cpy_bullet.GetComponent<NER_bullet>();
        bulletComponent.SetDirection(directionToPlayer);
    }
}
