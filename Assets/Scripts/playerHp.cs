using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHp : MonoBehaviour
{
    //현재 체력
    [SerializeField] private float curHealth;
    //최대 체력
    [SerializeField] public float maxHealth;
    //HP 설정
    public Slider HpBarSlider;
    Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;
    public void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
    }
    public void SetUp(float amount)
    {
        maxHealth = amount;
        curHealth = maxHealth;
    }

    
    public void CheckHp() //hp 상시 업데이트
    {
        if (HpBarSlider != null)
            HpBarSlider.value = curHealth / maxHealth;
    }

    public void Damage(float damage) //데미지 받음
    {
        if (maxHealth == 0 || curHealth <= 0) //체력 0이하 패스
            return;
        curHealth -= damage;
        CheckHp();
        if (curHealth <= 0)
        {
            //체력 0 플레이어 사망
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
            OnDamaged(collision.transform.position);
    }
    void OnDamaged(Vector2 targetPos)
    {
        //PlayerDamaged 11레이어
        gameObject.layer = 11;
        //플레이어 무적판정
        spriteRenderer.color = new Color(1, 1, 1, 0.3f);
        //피격 시 뒤로 물러남
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rb.AddForce(new Vector2(dirc, 1) * 5, ForceMode2D.Impulse);

        Invoke("OffDamaged", 0.5f);
    }

    void OffDamaged()
    {
        //무적판정 풀림
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
