using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBullet : MonoBehaviour//적이 쏘는 총알
{
    private Vector3 direction; // 총알의 이동 방향
    [SerializeField] private float speed = 10f; // 기본 속도
    PirateManager pirateManager;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized; // 방향 설정
        transform.rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)-90f);
    }
    private void Awake()
    {
        Destroy(this.gameObject, 5f);
        pirateManager = GameObject.Find("GameManager").GetComponent<PirateManager>();
    }

    private void Update()
    {
        // 총알 이동
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어나 다른 오브젝트와 충돌 시 처리
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject); // 충돌 시 총알 파괴
            pirateManager.DownHeart();
        }
    }
}