using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaCheck : MonoBehaviour
{
    private EnemyBehavior enemyParent;

    private void Awake()
    {
        enemyParent = GetComponentInParent<EnemyBehavior>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            
            gameObject.SetActive(false);
            enemyParent.target = collision.transform;
            enemyParent.hotZone.SetActive(true);
            enemyParent.inRange = true;
        }
    }
}
