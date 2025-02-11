using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEnemy : BasicEnemy
{
    public GameObject obstacle;
    public Vector3 spawnPoint;
    public float coolTime = 4f;

    protected override void Attack()
    {
        StartCoroutine(ThrowObstacle());
    }

    private IEnumerator ThrowObstacle()
    {
        while (true)
        {
            Instantiate(obstacle,transform.position,Quaternion.identity);
            StartCoroutine(PlayAnimation());
            yield return new WaitForSeconds(coolTime);
        }
    }
}
