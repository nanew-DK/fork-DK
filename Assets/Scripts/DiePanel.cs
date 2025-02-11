using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiePanel : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Bravo6()
    {
        Debug.Log("다이패널");
        animator.SetTrigger("Dying");
    }
}
