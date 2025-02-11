using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    public static int savePointIndex = 10; // ���̺� ����Ʈ ��ġ (�����÷��̿��� ����)
    public static int currentSavePoint;
    public static int currentSelectedSlot;
    public GameObject[] slotPrefab;
    public GameObject currentSlot;
    public Transform SaveSlotCanvas;

    public float fadeDuration = 0.5f; // ���̵� �� ���� �ð�
    public float delayBetweenSlots = 0.2f; // ���� �� ��Ÿ���� �ð� ����

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
        Debug.Log($"�ֱ� ���� : {currentSavePoint}");
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

                // CanvasGroup �߰� (���ٸ� ����)
                CanvasGroup cg = slotPrefab[i].GetComponent<CanvasGroup>();
                if (cg == null)
                    cg = slotPrefab[i].AddComponent<CanvasGroup>();

                cg.alpha = 0f; // ó���� ����
                StartCoroutine(FadeInSlot(cg)); // ���̵� �� ����
                yield return new WaitForSeconds(delayBetweenSlots); // ���� ���� ����
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
        cg.alpha = 1f; // ������ ���̵��� ����
    }

    public void Selecting(int slotIndex)
    {
        if (currentSlot == null) return;

        currentSlot.SetActive(true);
        currentSlot.transform.SetParent(slotPrefab[slotIndex].transform, false);
        currentSlot.transform.localPosition = Vector3.zero; // ���� �߾� ����
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
            Debug.Log($"Slot {slotIndex} ���õ�. ���̺� �����͸� �ε��մϴ�.");
            SceneManager.LoadScene("1LevelDesign");
        }
        else
        {
            Debug.Log("�� ������ ���� ��� �ֽ��ϴ�.");
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
