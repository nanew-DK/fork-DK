using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potan : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            BasicEnemy BE = collision.GetComponent<BasicEnemy>();
            BE.TakeDamage();
            Destroy(this.gameObject);
        }
        else if(collision.name== "Obstacle(Clone)")
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
        }
    }
}
