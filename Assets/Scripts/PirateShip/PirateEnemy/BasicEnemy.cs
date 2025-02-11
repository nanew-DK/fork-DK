using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    protected int Hp = 2;

    protected float speed = 3f;

    bool arrive = false;//다 내려갔으면 true 로 바뀜
    protected GameObject boundary;//어디까지 내려가는지

    [SerializeField] protected Animator animator;


    private void Start()
    {
        if(animator == null)    animator = GetComponent<Animator>();
        StartCoroutine(Move());
    }

    public void SetBoundary(GameObject bndry)
    {
        boundary = bndry;
    }

    protected IEnumerator Move()
    {
        while (!boundary) yield return new WaitForSeconds(0.1f);
        float stopLine = boundary.transform.position.x + transform.localScale.x / 2;
        while (!arrive)
        {
            transform.Translate(speed * Time.deltaTime * Vector3.left);
            if (this.transform.position.x <= stopLine) 
            {
                arrive = true;
                StopCoroutine(Move());
            }
            yield return new WaitForSeconds(0.002f);
        }
        Attack();
    }

    protected virtual void Attack()
    {
        
    }
    public void TakeDamage()
    {
        Hp--;
        CheckHp();
    }
    protected void CheckHp()
    {
        if (Hp == 0)
        {
            SpawnEnemy spawner = GetComponentInParent<SpawnEnemy>();
            spawner.decreaseNumOfEnemies();
            Destroy(this.gameObject);
        }
    }
    protected IEnumerator PlayAnimation()
    {
        animator.SetBool("Attack", true);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Attack", false);
    }
}
