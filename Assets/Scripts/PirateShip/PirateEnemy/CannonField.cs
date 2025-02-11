using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonField : MonoBehaviour
{
    PirateManager pirateManager;
    bool exit=false;
    void Awake()
    {
        pirateManager = GameObject.Find("GameManager").GetComponent<PirateManager>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player")&&exit==false)
        {
            pirateManager.DownHeart();
            exit = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(exit) exit = false;
    }
}
