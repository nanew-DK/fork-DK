using System.Collections;
using UnityEngine;

public class SaveCamera : MonoBehaviour
{
    private Camera cam;
    public float zoomFactor = 1.05f; // Ȯ�� ���� (���� ��ȭ ����)
    public float zoomDuration = 0.5f; // ��� ���� �ð�

    private float originalZoom; // ���� �� ũ��
    private float maxZoom; // Ȯ�� ũ��

    void Start()
    {
        cam = GetComponent<Camera>();
        originalZoom = cam.orthographicSize;
        maxZoom = originalZoom * zoomFactor; // ���� ũ�⿡�� 1.05�踸 Ȯ��

        // �������ڸ��� ���� ��¦ Ȯ�� ����
        cam.orthographicSize = maxZoom;

        // ��� �ڷ�ƾ ����
        StartCoroutine(ZoomEffect());
    }

    private IEnumerator ZoomEffect()
    {
        float elapsedTime = 0f;

        // ��� �ӵ��� ���� ���������� SmoothStep ����
        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / zoomDuration); // ���� �� ���� ����
            cam.orthographicSize = Mathf.Lerp(maxZoom, originalZoom, t);
            yield return null;
        }

        cam.orthographicSize = originalZoom; // ��Ȯ�� �� ����
    }
}
