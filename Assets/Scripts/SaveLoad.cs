using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    public static int savePointIndex = 10; // 세이브 포인트 수치 (게임플레이에서 증가)
    public static int currentSavePoint;
    public static int currentSelectedSlot;
    public GameObject[] slotPrefab;
    public GameObject currentSlot;
    public Transform SaveSlotCanvas;

    public float fadeDuration = 0.5f; // 페이드 인 지속 시간
    public float delayBetweenSlots = 0.2f; // 슬롯 간 나타나는 시간 간격

    float[,] uiPos = {
        { -235.8f,  323.3f },
        { 45.2f, 224.7f },
        { -182.5f, 149.1f },
        { -64.5f,  8.5f },
        { -349.9f, -179.8f },
        { -188.7f,  -304.1f },
        { -159.2f, -200.5f },
        { 121.6f, -209.1f },
        { 271f,  -59.6f },
        { 513f, 147f }
    };

    void Start()
    {
        currentSlot.SetActive(false);
        Debug.Log($"최근 지점 : {currentSavePoint}");
        StartCoroutine(ShowSaveSlotsWithFade());
    }

    IEnumerator ShowSaveSlotsWithFade()
    {
        for (int i = 0; i < slotPrefab.Length; i++)
        {
            if (i < savePointIndex)
            {
                slotPrefab[i].gameObject.SetActive(true);
                slotPrefab[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(uiPos[i, 0], uiPos[i, 1]);

                // CanvasGroup 추가 (없다면 생성)
                CanvasGroup cg = slotPrefab[i].GetComponent<CanvasGroup>();
                if (cg == null)
                    cg = slotPrefab[i].AddComponent<CanvasGroup>();

                cg.alpha = 0f; // 처음엔 투명
                StartCoroutine(FadeInSlot(cg)); // 페이드 인 실행
                yield return new WaitForSeconds(delayBetweenSlots); // 슬롯 간격 적용
            }
            else
            {
                slotPrefab[i].gameObject.SetActive(false);
            }
        }
    }

    IEnumerator FadeInSlot(CanvasGroup cg)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            cg.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 1f; // 완전히 보이도록 설정
    }

    public void Selecting(int slotIndex)
    {
        if (currentSlot == null) return;

        currentSlot.SetActive(true);
        currentSlot.transform.SetParent(slotPrefab[slotIndex].transform, false);
        currentSlot.transform.localPosition = Vector3.zero; // 슬롯 중앙 정렬
    }

    public void NotSelecting()
    {
        if (currentSlot != null)
        {
            currentSlot.SetActive(false);
        }
    }

    public void OnSlotClicked(int slotIndex)
    {
        if (slotIndex < savePointIndex)
        {
            currentSelectedSlot = slotIndex;
            Debug.Log($"Slot {slotIndex} 선택됨. 세이브 데이터를 로드합니다.");
            SceneManager.LoadScene("1LevelDesign");
        }
        else
        {
            Debug.Log("이 슬롯은 아직 잠겨 있습니다.");
        }
    }

    public void PriateCalls()
    {
        SceneManager.LoadScene("Pirate");
    }

    public void BossMab()
    {
        SceneManager.LoadScene("");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
