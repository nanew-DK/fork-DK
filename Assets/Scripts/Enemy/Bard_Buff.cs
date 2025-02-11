using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bard_Buff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            EnemyMove enemyMove = collision.GetComponent<EnemyMove>();
            EnemyBehavior enemyBehavior = collision.GetComponentInParent<EnemyBehavior>();

            if (enemyMove != null)
            {
                enemyMove.StartCoroutine("Buff");
            }
            else if (enemyBehavior != null)
            {
                enemyBehavior.StartCoroutine("Buff");
            }
            else
            {
                Debug.Log($"적 {collision.gameObject.name}에는 EnemyMove 또는 EnemyBehavior가 없습니다.");
            }
        }
    }
}
