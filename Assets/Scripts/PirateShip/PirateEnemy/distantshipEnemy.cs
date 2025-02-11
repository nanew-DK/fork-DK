using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distantshipEnemy : BasicEnemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1f; // �߻� ����
    [SerializeField] private float bulletSpeed = 10f;
    protected override void Attack()
    {
        InvokeRepeating("Shoot", 0f, fireInterval);
    }

    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        

        Vector3 playerPosition = FindPlayerPosition();
        Vector3 fireDirection = (playerPosition - firePoint.position).normalized;


        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        ShipBullet bulletScript = bullet.GetComponent<ShipBullet>();

        bulletScript.SetDirection(fireDirection);
        StartCoroutine(PlayAnimation());

    }


    private Vector3 FindPlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player != null ? player.transform.position : Vector3.zero;
    }
}
