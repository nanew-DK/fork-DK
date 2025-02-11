using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField] protected float speed = 1f;
    PirateManager pirateManager;
    void Awake()
    {
        pirateManager = GameObject.Find("GameManager").GetComponent<PirateManager>();
        Destroy(this.gameObject, 5f);
    }

    private void FixedUpdate()
    {
        Move();
    }


    protected void Move()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.left);
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            pirateManager.DownHeart();
        }
    }
}
