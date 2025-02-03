using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{//적의 무기에 넣는 스크립트
    protected float WeaponDamage=0f;
    [SerializeField] protected GameObject thisObject;
    private BoxCollider2D hitBox;
    EnemyBehavior behavior;
    private void Awake()
    {
        EnemyStats stats = thisObject.GetComponent<EnemyStats>();
        WeaponDamage = stats.damage;
        behavior=thisObject.GetComponentInParent<EnemyBehavior>(); //enemy가 AttackMode일 때만 플레이어의 체력이 닳게 하려고 함
        hitBox=GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if(behavior.GetAttackMode()==true)
        {
            hitBox.enabled = true;
        }
        else
        {
            hitBox.enabled=false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHP playerScript = collision.gameObject.GetComponent<PlayerHP>();
            if (playerScript != null && behavior.GetAttackMode())
            {
                playerScript.TakeDamage((int)WeaponDamage, transform.position); // float을 int로 변환
                Debug.Log("Enemy hit Player  Damage: " + WeaponDamage);
            }
        }
    }

}
