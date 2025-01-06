using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private EnemyBehavior enemyParent;
    private bool inRange;
    private Animator anim;

    private void Awake()
    {
        enemyParent=GetComponentInParent<EnemyBehavior>();
        anim=GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack"))
        {
            
            if(!enemyParent.CheckPlatform())
            {
                inRange = false;
                gameObject.SetActive(false);
                enemyParent.triggerArea.SetActive(true);
                enemyParent.inRange = false;
                enemyParent.SelectTarget();
            }
           
            enemyParent.Flip();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }
    void _()
    {
        return;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {

            inRange = false;
            gameObject.SetActive(false);
            enemyParent.triggerArea.SetActive(true);
            enemyParent.inRange=false;
            enemyParent.SelectTarget();
        }
    }
}
