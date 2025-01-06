using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public float curHp;
    public float maxHp=3f;
    public float damage=1f;

    protected bool isDie=false;
    virtual protected void Awake()
    {
        maxHp = 3f;
        curHp = maxHp;
        damage = 1f;
    }
    virtual public void TakeDamage(float damage,Transform player)
    {
        if (isDie) return;
        
        
        if (this.gameObject.transform.rotation.y == 0)//적이 왼쪽을 보고 있을 때
        {
            if (player.transform.position.x < gameObject.transform.position.x)//플레이어가 적의 왼쪽에 있을 경우 
            {
                Debug.Log("Enemy hit" + damage);
                curHp -= damage;
            }
            else
            {
                Debug.Log("BackAttack!! Enemy hit" + (damage+1));
                curHp -= damage + 1;
            }
        }
        else //적이 오른쪽을 보고있을 때 
        {
            if (player.transform.position.x < gameObject.transform.position.x)//플레이어가 적의 왼쪽에 있을 경우 
            {
                curHp -= damage + 1;//백어택
                Debug.Log("BackAttack!! Enemy hit" + (damage + 1));
            }
            else
            {
                Debug.Log("Enemy hit" + damage);
                curHp -= damage ;
            }
        }

        CheckHp();
    }
    
    virtual protected void CheckHp()
    {
        if (curHp <= 0)
        {
            isDie = true;
            Destroy(this.gameObject, 1f);
        }
    }
    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHp();
        }
    }
    protected bool IsDie()//use different script
    {
        return isDie;
    }
}
