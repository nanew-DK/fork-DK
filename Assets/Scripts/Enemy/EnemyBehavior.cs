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
    [SerializeField] protected Transform enemySprite;
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
    #endregion


    protected void Awake()
    {
        GameObject leftObj = new GameObject("LeftLimit");
        GameObject rightObj = new GameObject("RightLimit");
        leftLimit = leftObj.transform;
        rightLimit = rightObj.transform;

        SelectTarget();
        intTimer=timer;
        animator = GetComponent<Animator>();
    }
    protected void Update()
    {
        if(!attackMode)
        { 
            Move();
        }

        if (!InsideOfLimits() && !inRange )
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
        if(collision.gameObject.layer==6)
        {
            Ground=collision.gameObject;
            SpriteRenderer spriteRenderer = collision.collider.GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
            {
                return;
            }

            Bounds bounds = spriteRenderer.bounds;

            leftLimit.transform.position = new Vector2(bounds.min.x, bounds.max.y);
            rightLimit.transform.position= new Vector2(bounds.max.x, bounds.max.y);
        }
        
    }
    protected void EnemyLogic()
    {
        distance = Vector2.Distance(enemySprite.transform.position, target.transform.position);
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
            return target.transform.position.y  > transform.position.y-0.5f;
        }
        return false;
    }

    protected void Attack()
    {
        timer=intTimer;
        attackMode=true;
        animator.SetBool("Attack", true);
        animator.SetBool("CanWalk", false);
        
    }

    protected void StopAttack()
    {
        cooling = false;
        attackMode =false;
        animator.SetBool("Attack",false);
    }
    protected void Move()
    {
        animator.SetBool("CanWalk",true);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack"))
        {
            Vector2 targetPos = new Vector2(target.transform.position.x, transform.position.y);

            transform.position = Vector2.MoveTowards(transform.position,targetPos,moveSpeed*Time.deltaTime);
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
        cooling=true;
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
        StartCoroutine( waitFlip());
        
    }
    IEnumerator waitFlip()
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
        StopCoroutine(waitFlip());
    }
    public bool GetAttackMode()
    {
        return attackMode;  
    }
    public IEnumerator Slow()
    {
        moveSpeed =1f;
        yield return new WaitForSeconds(1.5f);
        moveSpeed =2f;
    }
}
