using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBehavior : MonoBehaviour
{
    #region Inspector Variables    
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected Transform PlayerTransform;
    [SerializeField] protected float timer;//attack cooltime
    public GameObject hotZone;
    public GameObject triggerArea;
    public float curveTime = 0f;//돌아보는 시간
    #endregion

    #region unvisible Variables
    protected Transform leftLimit;
    protected Transform rightLimit;
    [HideInInspector] public Transform target;
    [HideInInspector] public bool inRange;
    protected Animator animator;
    protected float distance;
    protected bool attackMode;
    protected GameObject Ground;
    [HideInInspector] public bool cooling;
    protected float intTimer;
    protected bool isStunned;
    #endregion


    protected void Awake()
    {
        GameObject leftObj = new GameObject("LeftLimit");
        GameObject rightObj = new GameObject("RightLimit");
        leftLimit = leftObj.transform;
        rightLimit = rightObj.transform;
        SelectTarget();
        intTimer = timer;
        animator = GetComponent<Animator>();
    }
    protected void Update()
    {
        if (isStunned == true)
        {
            moveSpeed = 0f;
            return;
        }
        if (!attackMode)
        {
            Move();
        }
        if (!InsideOfLimits() && !inRange)
        {
            SelectTarget();
        }
        if (inRange)
        {
            EnemyLogic();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Ground = collision.gameObject;
            SpriteRenderer spriteRenderer = collision.collider.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                return;
            }

            Bounds bounds = spriteRenderer.bounds;

            leftLimit.transform.position = new Vector2(bounds.min.x, bounds.max.y);
            rightLimit.transform.position = new Vector2(bounds.max.x, bounds.max.y);
        }
    }
    protected void EnemyLogic()
    {
        if (isStunned) return;
        distance = Vector2.Distance(this.transform.position, target.transform.position);
        if (distance > attackDistance)
        {
            StopAttack();
        }
        else if (attackDistance >= distance && cooling == false)
        {
            Attack();
        }
        if (cooling)
        {
            CoolDown();
            animator.SetBool("Attack", false);
        }
    }
    public bool CheckPlatform()
    {
        if (target != null)
        {
            return target.transform.position.y > transform.position.y - 0.5f;
        }
        return false;
    }

    protected void Attack()
    {
        timer = intTimer;
        attackMode = true;
        animator.SetBool("Attack", true);
        animator.SetBool("CanWalk", false);
    }

    protected void StopAttack()
    {
        cooling = false;
        attackMode = false;
        animator.SetBool("Attack", false);
        animator.SetBool("CanWalk", true);
    }
    protected void Move()
    {
        animator.SetBool("CanWalk", true);

        Vector2 direction = new Vector2((target.transform.position.x - transform.position.x), 0).normalized; // target으로 향하는 방향 계산
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 3.5f, 6);
        if (hit.collider != null)
        {
            SelectTarget();
        }

        // 현재 애니메이션이 "Enemy_Faint" 상태일 경우 이동하지 않음
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Faint")) // 추가!
        {
            Vector2 targetPos = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }
    }
    protected void CoolDown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }
    protected void TriggerCooling()
    {
        cooling = true;
    }
    protected bool InsideOfLimits()
    {
        return transform.position.x > leftLimit.transform.position.x && transform.position.x < rightLimit.transform.position.x;
    }

    public void SelectTarget()
    {
        float distanceToLeft = Vector2.Distance(transform.position, leftLimit.transform.position);
        float distanceToRight = Vector2.Distance(transform.position, rightLimit.transform.position);

        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit.transform;
        }
        else
        {
            target = rightLimit.transform;
        }

        Flip();

    }
    public void Flip()
    {
        StartCoroutine(WaitFlip());
    }
    IEnumerator WaitFlip()
    {
        yield return new WaitForSeconds(curveTime);
        Vector3 rotation = transform.eulerAngles;
        if (transform.position.x < target.transform.position.x)
        {
            rotation.y = 180f;
        }
        else
        {
            rotation.y = 0f;
        }
        transform.eulerAngles = rotation;
        StopCoroutine(WaitFlip());
    }
    public bool GetAttackMode()
    {
        return attackMode;
    }
    public IEnumerator Slow()
    {
        moveSpeed = 1f;
        yield return new WaitForSeconds(1.5f);
        moveSpeed = 2f;
    }

    // 버프 효과
    public IEnumerator Buff()
    {
        moveSpeed = 6f;
        timer /= 5f;
        intTimer /= 5f;
        yield return new WaitForSeconds(3f);
        moveSpeed = 2.5f;
        intTimer *= 5f;
    }
    public void Stun(float duration)
    {
        Debug.Log("기절");
        isStunned = true;
        animator.SetBool("isFaint", true); // 기절 애니메이션 재생

        Invoke(nameof(EndStun), duration);
    }

    private void EndStun()
    {
        moveSpeed = 2f;
        isStunned = false;
        animator.SetBool("isFaint", false);

        // 만약 공격 중이었다면 공격 상태로 전환
        if (attackMode)
        {
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("CanWalk", true);
        }
    }
}
