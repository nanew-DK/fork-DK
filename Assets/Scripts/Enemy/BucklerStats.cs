using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BucklerStats : EnemyStats
{
    protected override void Awake()
    {
        maxHp = 9f;
        curHp = maxHp;
        damage = 2f;
    }
    
    public override void TakeDamage(float damage, Transform player)
    {
        if ( isDie) return;
        Debug.Log("Enemy hit" + damage);;
        if (this.gameObject.transform.rotation.y == 0)//적이 왼쪽을 보고 있을 때
        {
            if (player.transform.position.x < gameObject.transform.position.x)//플레이어가 적의 왼쪽에 있을 경우 
            {
                
            }
            else
            {
                curHp -= damage + 1;
            }
        }
        else //적이 오른쪽을 보고있을 때 
        {
            if (player.transform.position.x < gameObject.transform.position.x)//플레이어가 적의 왼쪽에 있을 경우 
            {
                curHp -= damage + 1;//백어택
            }
            else
            {
                
            }
        }

        CheckHp();
    }
}
