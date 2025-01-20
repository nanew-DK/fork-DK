using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakBarrel : Object
{
    // Start is called before the first frame update
    private void Start()
    {
        speed = 7f;
    }
    
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            //PlayerShip playerScript = collision.GetComponent<PlayerShip>();
            //playerScript.TakeDamage(2f);
        }
    }
}
