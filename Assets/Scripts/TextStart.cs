using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TextStart : MonoBehaviour
{
    public bool DoText; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DoText = false;
            // π∫∞°∏¶ Ω√¿€
            Destroy(this.gameObject);
        }
    }
}
