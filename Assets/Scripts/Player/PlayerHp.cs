using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private Slider HpBarSlider; // UI 슬라이더
    public float maxHP = 100;     // 최대 HP
    private float currentHP;      // 현재 HP

    PlayerMove Playerscript;
    PlayerAttack PlayerAtk;

    void Start()
    {
        // 초기 HP 설정
        currentHP = maxHP;

        HpBarSlider.maxValue = maxHP;

        // 슬라이더의 초기 값 설정
        HpBarSlider.value = currentHP;

        Playerscript = GetComponent<PlayerMove>();
    }

    public void TakeDamage(float damage, Vector2 targetpos)
    {
        if(Playerscript.GetParrying()==true)
        {
            StartCoroutine(Playerscript.ParryingSuccess());
            return;
        }
        currentHP -= damage;
        // 체력이 0 이하인지 확인
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die(); // 사망 처리
        }
        CheckHp(); // 체력 UI 갱신
        Playerscript.OnDamaged(targetpos);//넉백
    }

    private void Die()
    {
        Debug.Log("플레이어 사망!");
        SceneManager.LoadScene("Gameover");
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //DecreaseHP(1);  // 충돌 시 HP 1 감소*
        }
    }

    public void CheckHp() //hp 상시 업데이트
    {
        if (HpBarSlider != null)
            HpBarSlider.value = currentHP;
    }
}
