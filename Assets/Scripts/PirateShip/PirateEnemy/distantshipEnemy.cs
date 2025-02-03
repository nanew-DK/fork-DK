using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distantshipEnemy : BasicEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1f; // 발사 간격
    [SerializeField] private float bulletSpeed = 20f;

    private void Start()
    {
        InvokeRepeating("Shoot", 0f, fireInterval);
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        // 총알 생성
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // 생성된 총알의 ShipBullet 스크립트를 가져옴
        ShipBullet bulletScript = bullet.GetComponent<ShipBullet>();

        if (bulletScript != null)
        {
            // 플레이어 방향 계산
            Vector3 playerPosition = FindPlayerPosition();
            Vector3 fireDirection = (playerPosition - firePoint.position).normalized;

            // 총알 방향 설정
            bulletScript.SetDirection(fireDirection);
        }
    }


    // 플레이어 위치를 찾는 헬퍼 함수
    private Vector3 FindPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
    }
}
