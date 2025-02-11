using System.Collections;
using UnityEngine;

public class SaveCamera : MonoBehaviour
{
    private Camera cam;
    public float zoomFactor = 1.05f; // 확대 비율 (거의 변화 없음)
    public float zoomDuration = 0.5f; // 축소 지속 시간

    private float originalZoom; // 원래 줌 크기
    private float maxZoom; // 확대 크기

    void Start()
    {
        cam = GetComponent<Camera>();
        originalZoom = cam.orthographicSize;
        maxZoom = originalZoom * zoomFactor; // 원래 크기에서 1.05배만 확대

        // 시작하자마자 아주 살짝 확대 적용
        cam.orthographicSize = maxZoom;

        // 축소 코루틴 실행
        StartCoroutine(ZoomEffect());
    }

    private IEnumerator ZoomEffect()
    {
        float elapsedTime = 0f;

        // 축소 속도가 점차 느려지도록 SmoothStep 적용
        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / zoomDuration); // 가속 후 점차 감속
            cam.orthographicSize = Mathf.Lerp(maxZoom, originalZoom, t);
            yield return null;
        }

        cam.orthographicSize = originalZoom; // 정확한 값 유지
    }
}
