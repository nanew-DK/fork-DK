using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonEnemy : BasicEnemy
{
    public GameObject cannonField;
    protected override void Attack()
    {
        if (Hp <= 0)
        {
            CancelInvoke("Attack");
            return;
        }
        Vector3 spawnPosition = new Vector3(transform.position.x - 8.5f, transform.position.y, transform.position.z);
        GameObject CF = Instantiate(cannonField, spawnPosition, transform.rotation);
        StartCoroutine(PlayAnimation());
        Destroy(CF, 2f);
        Invoke("Attack", 4f);
    }
    
}
