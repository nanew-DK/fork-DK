using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] int savePos = 0;


    public static int savePointIndex;
    public static int diePoint;
    private bool usedSave = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!usedSave)
            {
                savePointIndex = savePos;
                usedSave = true;
                Debug.Log(savePointIndex);
            }
            diePoint = savePos;
        }
    }
}
