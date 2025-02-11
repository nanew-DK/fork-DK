using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OakBarrel : Object//Rock
{
    PirateManager pirateManager;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        speed = 1f;
        pirateManager = GameObject.Find("GameManager").GetComponent<PirateManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            pirateManager.DownHeart();
        }
    }
}
