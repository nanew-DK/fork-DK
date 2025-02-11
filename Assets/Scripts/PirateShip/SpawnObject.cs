using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Item;

    [SerializeField] private float coolTime = 3f;
    
    float screenHeight;
    float screenWidth;
    void Awake()
    {
        screenHeight = Camera.main.orthographicSize ;
        screenWidth = screenHeight * Camera.main.aspect;

        StartCoroutine(Spawn());
    }

    protected virtual IEnumerator Spawn()
    {
        while (true)
        {
            Vector3 randomPos=new(screenWidth,Random.Range(-screenHeight, screenHeight),0);
            Instantiate(Item, randomPos,Quaternion.identity,transform);
            yield return new WaitForSeconds(coolTime);
        }
    }
}
