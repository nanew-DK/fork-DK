using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bard_Buff : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            EnemyMove EA = collision.GetComponent<EnemyMove>();
            EA.StartCoroutine("Buff");
        }
    }
}
