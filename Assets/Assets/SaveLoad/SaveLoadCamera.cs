using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadCamera : MonoBehaviour
{
    public float zoomAmount = 1.05f; // 처음 확대할 비율
    public float zoomSpeed = 0.5f; // 축소 속도

    private float originalSize;

    void Start()
    {
        originalSize = Camera.main.orthographicSize; // 원래 크기 저장

        // 씬 시작 시 확대된 상태로 설정
        Camera.main.orthographicSize = originalSize * zoomAmount;

        // 이후 원래 크기로 축소하는 효과 적용
        StartCoroutine(ZoomOutToOriginal());
    }

    IEnumerator ZoomOutToOriginal()
    {
        float elapsedTime = 0f;
        float duration = 1f / zoomSpeed; // 축소 시간

        float startSize = Camera.main.orthographicSize; // 현재 크기 (확대된 상태)
        float targetSize = originalSize; // 원래 크기

        while (elapsedTime < duration)
        {
            Camera.main.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Camera.main.orthographicSize = targetSize; // 최종 크기 설정
    }
}
