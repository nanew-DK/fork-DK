using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : Object
{
    
    private void Start()
    {
        speed = 5f;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            //PlayerShip playerScript = collision.GetComponent<PlayerShip>();
            //playerScript.GetHeart();
        }
    }
}
