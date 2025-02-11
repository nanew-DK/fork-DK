using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

public class Bard : EnemyMove
{
    public GameObject attackRange;
    public GameObject buffPrefab;    // 버프 범위
    private GameObject target;

    private CircleCollider2D rangeCollider;
    private SpriteRenderer spriteRenderer;

    [SerializeField] float Attackspeed = 10f;
    [SerializeField] private float increaseRate = 1f;  // 수치 증가 속도
    [SerializeField] private float decreaseRate = 0.5f; // 수치 감소 속도

    private float chargeTime = 0;
    private bool isPlayerDetected = false;
    private bool Wait = true;
    private bool isChargingAnimPlayed = false;

    //인식범위 받기
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rangeCollider = attackRange.GetComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
    }
    private void Update()
    {
        if (isStunned) return;
        //공격 가능 유무
        if (chargeTime >= 3f)
        {
            Music();
        }
        if (chargeTime >= 2.2f && !isChargingAnimPlayed)
        {
            anim.SetTrigger("isCharging");
            anim.SetBool("isCollide", false);
            isChargingAnimPlayed = true; // 애니메이션 실행됨
        }
        //비파 치는 시간
        if (isPlayerDetected)
        {
            if (chargeTime < 3 && Wait)
                chargeTime += increaseRate * Time.deltaTime; // 일정 시간마다 수치 증가
        }
        else
        {
            if (chargeTime > 0)
                chargeTime -= decreaseRate * Time.deltaTime; // 일정 시간마다 수치 감소
        }
        //좌우 보기
        if (target != null)
        {
            spriteRenderer.flipX = target.transform.position.x < transform.position.x;
        }
    }
    //플레이어 감지
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            speed = 0;
            target = collision.gameObject;
            anim.SetBool("isCollide", true);
            isPlayerDetected = true;
        }
    }
    //플레이어 놓침
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            speed = 2.5f;
            target = null;
            anim.SetBool("isCollide", false);
            Debug.Log("플레이어 놓침");
            isPlayerDetected = false;
        }
    }
    //공격 텀
    private IEnumerator WaitAttack()
    {
        Wait = false;
        yield return new WaitForSeconds(Attackspeed);
        Wait = true;
    }
    //버프 부여
    private void Music()
    {
        StartCoroutine("WaitAttack");
        Debug.Log("연주 시작");
        chargeTime = 0;

        // 버프 오브젝트 생성
        GameObject currentBuff = Instantiate(buffPrefab, transform.position, Quaternion.identity);

        // 서서히 사라지는 코루틴 실행
        StartCoroutine(FadeOutAndDestroy(currentBuff, 1f));

        isChargingAnimPlayed = false; // 애니메이션 초기화
    }

    private IEnumerator FadeOutAndDestroy(GameObject obj, float duration)
    {
        SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
        if (sprite == null) yield break; // 스프라이트가 없으면 종료

        float elapsedTime = 0f;
        Color color = sprite.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / duration); // 점점 투명하게
            sprite.color = color;
            yield return null;
        }

        Destroy(obj);
    }
}