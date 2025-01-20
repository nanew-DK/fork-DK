using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEnemy : BasicEnemy
{
    public GameObject obstacle;
    public Vector3 spawnPoint;
    public float coolTime=4f;

    private void Awake()
    {
        // SpriteRenderer 컴포넌트 가져오기
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Bounds를 통해 왼쪽 가운데 위치 계산
            Bounds bounds = spriteRenderer.bounds;

            // 왼쪽 중앙 좌표 계산
            spawnPoint = new Vector3(bounds.min.x, bounds.center.y, bounds.center.z);

            // 결과 출력
            Debug.Log("왼쪽 중앙 위치: " + spawnPoint);
        }
        else
        {
            Debug.LogError("SpriteRenderer가 없습니다!");
        }
    }
    protected override void Attack()
    {
        StartCoroutine(ThrowObstacle());
    }

    private IEnumerator ThrowObstacle()
    {
        while (true)
        {
            Instantiate(obstacle,transform.position,Quaternion.identity);
            yield return new WaitForSeconds(coolTime);
        }
    }
}
