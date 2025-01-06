using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEyePattern : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject eyePrefab;
    public void SpawnEye(GameObject bossScript)
    {
        Vector2 randomSpawnPoint= new Vector2 (Random.Range(0,Screen.width), Random.Range(0, Screen.height));
        Vector2 spawnPoint = Camera.main.ScreenToWorldPoint(randomSpawnPoint); 
        GameObject eye=Instantiate(eyePrefab,spawnPoint,Quaternion.identity,bossScript.transform);
        eye.tag = "Boss";
        Destroy(eye, 8f); 
    }

}
