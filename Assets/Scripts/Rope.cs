using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    GameObject player;
    [SerializeField] private GameObject tail;//마지막 원
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AnchorPlayer(Transform player)
    {
        //player.transform.position=tail.transform.position;
    }
    
}
