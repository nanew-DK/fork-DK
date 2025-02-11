using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBullet : MonoBehaviour//���� ��� �Ѿ�
{
    private Vector3 direction; // �Ѿ��� �̵� ����
    [SerializeField] private float speed = 10f; // �⺻ �ӵ�
    PirateManager pirateManager;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized; // ���� ����
        transform.rotation = Quaternion.Euler(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg)-90f);
    }
    private void Awake()
    {
        Destroy(this.gameObject, 5f);
        pirateManager = GameObject.Find("GameManager").GetComponent<PirateManager>();
    }

    private void Update()
    {
        // �Ѿ� �̵�
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ �ٸ� ������Ʈ�� �浹 �� ó��
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject); // �浹 �� �Ѿ� �ı�
            pirateManager.DownHeart();
        }
    }
}