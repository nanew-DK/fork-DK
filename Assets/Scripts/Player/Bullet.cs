using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10f;
    private Vector3 direction; // 총알의 발사 방향

    PlayerMove pm;
    float damage = 1f;

    private void Start()
    {
        pm = FindObjectOfType<PlayerMove>();
    }
    // 방향 설정 메소드
    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * bulletSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            EnemyMove EA = collision.GetComponent<EnemyMove>();
            Destroy(this.gameObject);

            if (EA != null)
            {
                Debug.Log("EA");
                if (pm.GetSuccessParrying())
                    EA.TakeDamage((int)damage + 1);
                else
                    EA.TakeDamage((int)damage);
                EA.StartCoroutine("Slow");
            }
            else
            {
                BucklerStats BS = collision.gameObject.GetComponentInParent<BucklerStats>();
                if (BS != null)
                {
                    if (pm.GetSuccessParrying())
                        BS.TakeDamage(damage + 1f, pm.gameObject.transform);
                    else
                    {
                        BS.TakeDamage(damage, pm.gameObject.transform);
                    }

                }
                else
                {
                    EnemyStats ES = collision.gameObject.GetComponentInParent<EnemyStats>();
                    if (pm.GetSuccessParrying())
                        ES.TakeDamage(damage + 1f, pm.gameObject.transform);
                    else
                    {
                        ES.TakeDamage(damage, pm.gameObject.transform);
                    }
                }
                EnemyBehavior EB = collision.GetComponentInParent<EnemyBehavior>();
                EB.StartCoroutine(EB.Slow());
            }
        }
    }
}