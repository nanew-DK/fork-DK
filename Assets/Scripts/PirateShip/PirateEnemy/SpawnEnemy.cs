using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private int enemyNum = 0;
    [SerializeField] int enemyLimit = 3;
    
    [SerializeField] private float coolTime = 3f;
    [SerializeField] private GameObject stopLine;

    [SerializeField] GameObject[] enemyPrefab;

    float screenHeight;
    float screenWidth;

    
    void Awake()
    {
        enemyNum = 0;
        screenHeight = Camera.main.orthographicSize;
        screenWidth = screenHeight * Camera.main.aspect;

        StartCoroutine(Spawn());
    }

    public void increaseNumOfEnemies()
    {
        enemyNum++;
    }
    public void decreaseNumOfEnemies()
    {
        enemyNum--;
    }

    protected IEnumerator Spawn()
    {
        while (true)
        {
            if(enemyNum<enemyLimit)
            {
                int num=Random.Range(0, enemyPrefab.Length);
                
                Vector2 randomPos = new(screenWidth, Random.Range(-screenHeight, screenHeight));
                GameObject enemy= Instantiate(enemyPrefab[num], randomPos, Quaternion.identity, this.transform);
                BasicEnemy enemyScript= enemy.GetComponent<BasicEnemy>();
                enemyScript.SetBoundary(stopLine);
                increaseNumOfEnemies();
                yield return new WaitForSeconds(coolTime);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }


}
