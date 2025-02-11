using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    private CameraMove cameraMove; // ī�޶� ����� ���� �߰�

    public class EnemyBehavior : MonoBehaviour
    {
        public void Stun(float duration)
        {
            // ���� ���� ����
            Debug.Log("EnemyBehavior stunned for " + duration + " seconds.");
        }
    }

    public class CameraMove : MonoBehaviour
    {
        public void StartShake()
        {
            // ȭ�� ���� ���� ����
            Debug.Log("Camera shaking!");
        }
    }

    public class EnemyMove : MonoBehaviour
    {
        public void Stun(float duration)
        {
            // ���� ���� ����
            Debug.Log("Enemy stunned for " + duration + " seconds.");
        }
    }


    private void Start()
    {
        // ī�޶� ã��
        cameraMove = FindObjectOfType<CameraMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyMove enemyMove = collision.GetComponent<EnemyMove>();
                EnemyBehavior enemyBehavior = collision.GetComponentInParent<EnemyBehavior>();

                if (enemyMove != null)
                {
                    enemyMove.Stun(3f);
                }
                else if (enemyBehavior != null)
                {
                    enemyBehavior.Stun(3f);
                }
                else
                {
                    Debug.Log("����ù�");
                }

                // ���� ������ �� ȭ�� ���� ����
                if (cameraMove != null)
                {
                    cameraMove.StartShake();
                }
                else
                {
                    Debug.LogWarning("ī�޶� �� ã��");
                }
            }
        }
    }
}
