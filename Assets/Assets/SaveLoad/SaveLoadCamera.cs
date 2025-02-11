using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadCamera : MonoBehaviour
{
    public float zoomAmount = 1.05f; // ó�� Ȯ���� ����
    public float zoomSpeed = 0.5f; // ��� �ӵ�

    private float originalSize;

    void Start()
    {
        originalSize = Camera.main.orthographicSize; // ���� ũ�� ����

        // �� ���� �� Ȯ��� ���·� ����
        Camera.main.orthographicSize = originalSize * zoomAmount;

        // ���� ���� ũ��� ����ϴ� ȿ�� ����
        StartCoroutine(ZoomOutToOriginal());
    }

    IEnumerator ZoomOutToOriginal()
    {
        float elapsedTime = 0f;
        float duration = 1f / zoomSpeed; // ��� �ð�

        float startSize = Camera.main.orthographicSize; // ���� ũ�� (Ȯ��� ����)
        float targetSize = originalSize; // ���� ũ��

        while (elapsedTime < duration)
        {
            Camera.main.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.orthographicSize = targetSize; // ���� ũ�� ����
    }
}
