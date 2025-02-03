using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NER_bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 20f;
    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;

        // 방향에 맞게 총알 회전
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    void Update()
    {
        // 방향을 따라 이동
        transform.Translate(Vector3.right * bulletSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerHP PH = collision.GetComponent<PlayerHP>();
            // vector2가 뭘까용
            PH.TakeDamage(1, direction);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(this.gameObject);
        }
    }
}
