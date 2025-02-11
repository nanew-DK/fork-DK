using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;  // �÷��̾�
    public Vector3 offset = new Vector3(0, 3f, -10); // Y���� ��¦ �� �ø� (2.5 -> 3)
    public float smoothSpeed = 0.125f;  // �ε巯�� �̵� �ӵ�

    public float normalZoom = 8f;  // �⺻ ��
    public float idleZoom = 10f;   // �÷��̾ ������ ���� �� ��
    public float zoomSpeed = 2f;   // �� ��ȭ �ӵ�
    public float idleDelay = 2f;   // ���� ���� �� �� �ƿ����� �ɸ��� �ð�

    private Camera cam;
    private float targetZoom;
    private bool isMoving = false; // �÷��̾ �̵� ������ �Ǻ�
    private float idleTimer = 0f;  // ���� ���¿��� ��� �ð�

    // ȭ�� ���� ���� ���� �߰�
    private bool isShaking = false;
    private float shakeDuration = 0.5f;  // ��鸮�� ���� �ð�
    private float shakeMagnitude = 0.2f; // ��鸮�� ����
    private Vector3 originalPosition;

    void Start()
    {
        cam = Camera.main;
        cam.orthographicSize = normalZoom;  // �ʱ� �� ����
        targetZoom = normalZoom;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            if (!isShaking) // ��鸮�� ���ȿ��� ī�޶� ������ ���ع��� �ʵ���
            {
                // �÷��̾� �̵� ���� �Ǻ�
                if (Input.GetKey(KeySetting.Keys[KeyAction.LEFT]) || Input.GetKey(KeySetting.Keys[KeyAction.RIGHT]))
                {
                    isMoving = true;
                    idleTimer = 0f; // �̵��ϸ� Ÿ�̸� �ʱ�ȭ
                }
                else
                {
                    if (!isMoving)
                    {
                        idleTimer += Time.deltaTime; // ���� ���¿��� �ð� ����
                    }
                    isMoving = false;
                }

                // 2�� �̻� ���߸� �� �ƿ� (10f), �ƴϸ� �⺻ �� (8f)
                targetZoom = (idleTimer >= idleDelay) ? idleZoom : normalZoom;

                // ���� �ε巴�� ����
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

                // ��� ��ġ ����
                Vector3 desiredPosition = target.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;
            }
        }
    }

    // ȭ�� ���� ��� �߰�
    public void StartShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        isShaking = true;
        originalPosition = transform.position;

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float xOffset = Random.Range(-1f, 1f) * shakeMagnitude;
            float yOffset = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = originalPosition + new Vector3(xOffset, yOffset, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // ���� ��ġ�� ����
        isShaking = false;
    }
}
