using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public GameObject player;
    public GameObject strike1;
    public GameObject strike2;
    public GameObject strike3;

    private int whatSwordAttack = 0;
    private Animator anim;

    private void Start()
    {
        anim = player.GetComponent<Animator>();  // 플레이어 애니메이터 가져오기
    }

    public void doSwordAttack()
    {
        anim.SetTrigger("IsAttack");  // 애니메이션 트리거 설정

        if (whatSwordAttack == 0)
        {
            firstSword();
            whatSwordAttack++;
            StartCoroutine("whatAttack1");
        }
        else if (whatSwordAttack == 1)
        {
            secondSword();
            whatSwordAttack++;
            StopCoroutine("whatAttack1");
            StartCoroutine("whatAttack2");
        }
        else if (whatSwordAttack == 2)
        {
            thirdSword();
            whatSwordAttack = 0;
        }
    }

    private IEnumerator whatAttack1()
    {
        yield return new WaitForSeconds(1.5f);
        whatSwordAttack = 0;
    }

    private IEnumerator whatAttack2()
    {
        yield return new WaitForSeconds(1.5f);
        whatSwordAttack = 0;
    }

    private void firstSword()
    {
        Debug.Log("첫번째");
        GameObject a = Instantiate(strike1, transform.position, transform.rotation);
        Destroy(a, 1f);
        AdjustPlayerPositionAndRotation(a, -90f, 90f);
    }

    private void secondSword()
    {
        Debug.Log("두번째");
        if (player.transform.position.x < this.transform.position.x)
        {
            transform.position += transform.right * 3f;
        }
        else
        {
            transform.position -= transform.right * 3f;
        }

        GameObject b = Instantiate(strike2, transform.position, transform.rotation);
        Destroy(b, 1f);
        AdjustPlayerPositionAndRotation(b, -90f, 90f);
    }

    private void thirdSword()
    {
        Debug.Log("세번째");
        GameObject c = Instantiate(strike3, transform.position, transform.rotation);
        Destroy(c, 1f);
        player.transform.position += transform.up * 2f;
    }

    private void AdjustPlayerPositionAndRotation(GameObject obj, float leftAngle, float rightAngle)
    {
        if (player.transform.position.x < this.transform.position.x)
        {
            player.transform.position += transform.right * 1f;
            obj.transform.rotation = Quaternion.Euler(0, 0, leftAngle);
        }
        else
        {
            player.transform.position += -transform.right * 1f;
            obj.transform.rotation = Quaternion.Euler(0, 0, rightAngle);
        }
    }
public void ResetAttack()
    {
        anim.ResetTrigger("IsAttack");
    }

}
