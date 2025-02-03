using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    public GameObject heartPrefab;  // 하트 프리팹
    public int maxHP = 5;  // 최대 체력
    public int currentHP = 5;  // 현재 체력
    public List<GameObject> heartObjects = new List<GameObject>();  // 하트 GameObject 리스트

    private bool isInvincible = false;  // 무적 상태 여부
    public float invincibilityDuration = 1f;  // 무적 상태 지속 시간

    void Start()
    {
        CreateHearts();  // 게임 시작 시 하트 객체 생성
        UpdateHearts();  // 초기 하트 이미지 업데이트
    }

    // 하트 프리팹을 이용해 하트 객체 생성
    void CreateHearts()
    {
        for (int i = 0; i < maxHP; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform); // 프리팹을 인스턴스화하여 부모로 설정
            heartObjects.Add(heart);  // 하트 객체 리스트에 추가
        }
    }

    // 체력 감소 처리
    public void TakeDamage(int damage, Vector2 targetpos)
    {
        // 무적 상태일 경우 데미지 무효화
        if (isInvincible) return;

        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }

        // 하트 업데이트
        UpdateHearts();

        // 무적 상태 시작
        StartCoroutine(InvincibilityCoroutine());
    }

    // 하트 상태 업데이트
    void UpdateHearts()
    {
        for (int i = 0; i < heartObjects.Count; i++)
        {
            // 현재 체력에 맞는 하트를 활성화 또는 비활성화
            heartObjects[i].SetActive(i < currentHP);
        }
    }

    // 플레이어 사망 처리
    public void Die()
    {
        Debug.Log("플레이어 사망");
        SceneManager.LoadScene("Gameover");
    }

    // 무적 상태 코루틴
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;  // 무적 상태 시작
        yield return new WaitForSeconds(invincibilityDuration);  // 지정된 시간 동안 대기
        isInvincible = false;  // 무적 상태 종료
    }
}
