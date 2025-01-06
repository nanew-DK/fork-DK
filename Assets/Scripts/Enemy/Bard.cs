using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class Bard : EnemyMove
{
    public GameObject attackRange;
    public GameObject buffPrefab;    // 버프 범위
    //public GameObject chargeBar;    // 차지 UI

    private CircleCollider2D rangeCollider;

    [SerializeField] float Attackspeed = 10f;
    [SerializeField] private float increaseRate = 1f;  // 수치 증가 속도
    [SerializeField] private float decreaseRate = 0.5f; // 수치 감소 속도

    private float chargeTime = 0;
    private bool isPlayerDetected = false;
    private bool Wait = true;


    //인식범위 받기
    void Start()
    {
        rangeCollider = attackRange.GetComponent<CircleCollider2D>();
        rangeCollider.isTrigger = true;
        //GameObject CB = Instantiate(chargeBar, transform.position, transform.rotation);
    }
    private void Update()
    {
        //공격 가능 유무
        if ( chargeTime >= 3f)
        {
            Music();
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
    }
    //플레이어 감지
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            speed = 0;
            Debug.Log("플레이어 감지");
            isPlayerDetected = true;
        }
    }
    //플레이어 놓침
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            speed = 2.5f;
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
        GameObject currentBuff = Instantiate(buffPrefab, transform.position, Quaternion.identity);
        Destroy(currentBuff, 1f);
    }
}
