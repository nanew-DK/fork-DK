using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // 필요한 게임 오브젝트 참조
    public GameObject player;    // 플레이어 오브젝트
    public GameObject strike1;   // 첫 번째 공격 이펙트
    public GameObject strike2;   // 두 번째 공격 이펙트
    public GameObject strike3;   // 세 번째 공격 이펙트

    private int whatSwordAttack = 0;    // 현재 공격 단계를 추적 (0: 시작, 1: 첫번째 후, 2: 두번째 후)
    private Animator anim;              // 플레이어의 애니메이터 컴포넌트

    private void Start()
    {
        anim = player.GetComponent<Animator>();  // 플레이어 애니메이터 가져오기
    }

    public void doSwordAttack()
    {
        // 현재 공격 단계에 따라 적절한 공격 메서드 호출
        if (whatSwordAttack == 0)
        {
            firstSword();      // 첫 번째 공격
        }
        else if (whatSwordAttack == 1)
        {
            secondSword();     // 두 번째 공격
        }
        else if (whatSwordAttack == 2)
        {
            thirdSword();      // 세 번째 공격
        }
    }

    // 첫 번째 공격 후 일정 시간이 지나면 콤보를 리셋하는 코루틴
    private IEnumerator whatAttack1()
    {
        yield return new WaitForSeconds(1.5f);    // 1.5초 대기
        whatSwordAttack = 0;                      // 콤보 리셋
    }

    // 두 번째 공격 후 일정 시간이 지나면 콤보를 리셋하는 코루틴
    private IEnumerator whatAttack2()
    {
        yield return new WaitForSeconds(1.5f);    // 1.5초 대기
        whatSwordAttack = 0;                      // 콤보 리셋
    }

    private void firstSword()
    {
        whatSwordAttack++;     // 다음 콤보로 진행

        Debug.Log("첫번째");
        // 애니메이션 트리거 설정
        anim.ResetTrigger("IsAttack1"); // 이전 트리거를 리셋하여 애니메이션이 깔끔하게 재생되도록 함
        anim.SetTrigger("IsAttack1");   // 첫 번째 공격 애니메이션 재생

        // 공격 이펙트 생성 및 제거
        GameObject a = Instantiate(strike1, this.transform);  // 첫 번째 공격 이펙트 생성
        StartCoroutine("whatAttack1");                       // 콤보 리셋 타이머 시작
        Destroy(a, 0.5f);                                    // 0.5초 후 이펙트 제거
    }

    private void secondSword()
    {
        whatSwordAttack++;     // 다음 콤보로 진행

        Debug.Log("두번째");
        // 애니메이션 트리거 설정
        anim.ResetTrigger("IsAttack2");
        anim.SetTrigger("IsAttack2");   // 두 번째 공격 애니메이션 재생

        // 공격 이펙트 생성 및 제거
        GameObject b = Instantiate(strike2, this.transform);  // 두 번째 공격 이펙트 생성
        StopCoroutine("whatAttack1");                        // 이전 콤보 리셋 타이머 중지
        StartCoroutine("whatAttack2");                       // 새로운 콤보 리셋 타이머 시작
        Destroy(b, 0.5f);                                    // 0.5초 후 이펙트 제거
    }

    private void thirdSword()
    {
        whatSwordAttack = 0;   // 콤보 즉시 리셋

        Debug.Log("세번째");
        // 애니메이션 트리거 설정
        anim.ResetTrigger("IsAttack3");
        anim.SetTrigger("IsAttack3");   // 세 번째 공격 애니메이션 재생

        // 공격 이펙트 생성 및 제거
        GameObject c = Instantiate(strike3, this.transform);  // 세 번째 공격 이펙트 생성
        StopCoroutine("whatAttack2");                        // 이전 콤보 리셋 타이머 중지
        Destroy(c, 0.5f);                                    // 0.5초 후 이펙트 제거
    }

    // 플레이어의 위치와 회전을 조정하는 헬퍼 메서드
    private void AdjustPlayerPositionAndRotation(GameObject obj, float leftAngle, float rightAngle)
    {
        // 플레이어가 오브젝트의 왼쪽에 있는 경우
        if (player.transform.position.x < this.transform.position.x)
        {
            player.transform.position += transform.right * 1f;  // 오른쪽으로 이동
            obj.transform.rotation = Quaternion.Euler(0, 0, leftAngle);  // 왼쪽 각도로 회전
        }
        // 플레이어가 오브젝트의 오른쪽에 있는 경우
        else
        {
            player.transform.position += -transform.right * 1f;  // 왼쪽으로 이동
            obj.transform.rotation = Quaternion.Euler(0, 0, rightAngle);  // 오른쪽 각도로 회전
        }
    }
}
