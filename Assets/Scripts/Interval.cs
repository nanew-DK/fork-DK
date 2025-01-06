using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interval : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Size;

    private int numOfEnemies = 0;
    private bool isPlayerInterval=false;
    private bool stageClear=false;
    bool doorOpen=false;


   void Start()
    {
        DetectEnemies();
        SpriteRenderer spr= Size.GetComponent<SpriteRenderer>();
        Color color = spr.color;
        color.a = 0f;
        spr.color = color;

    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            DetectEnemies();
            Debug.Log("Enemy: "+numOfEnemies);
        }
        if (numOfEnemies <= 0&& isPlayerInterval)
        {
            stageClear = true;
        }
        if (stageClear && isPlayerInterval && !doorOpen) 
        {
            
            Debug.Log("다음 구간으로 이동");
            doorOpen = true;
        }

    }
    
    
    void DetectEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(Size.transform.position, Size.transform.localScale, 0f);
        int currentEnemy = 0;
        // 검색된 콜라이더에서 태그가 일치하는 오브젝트 찾기
        isPlayerInterval = false;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                currentEnemy++;
            }
            if (collider.CompareTag("Player")) 
            {
                isPlayerInterval = true;
            }
        }
        numOfEnemies = currentEnemy;
    }
}
