using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
//총 공격 명령 시행시 작동, 총알 복사본 제작
public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public GameObject player;
    private Bullet bulletScript;
    private Vector3 bulletDirection;

    void Start()
    {
        bulletScript = bullet.GetComponent<Bullet>();
    }

    // 총알 발사 메소드
    public void doGunAttack()
    {
        Debug.Log("총 발사");
        if (player.transform.position.x < this.transform.position.x)
        {
            bulletDirection = -transform.right;
        }
        else
        {
            bulletDirection = transform.right;
        }

        
        GameObject cpy_bullet = Instantiate(bullet, transform.position, transform.rotation);

        // 생성된 총알 스크립트에서 방향 설정
        Bullet bulletComponent = cpy_bullet.GetComponent<Bullet>();
        bulletComponent.SetDirection(bulletDirection);

        Destroy(cpy_bullet, 5f);
    }
}
