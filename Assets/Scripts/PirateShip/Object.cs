using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Object : MonoBehaviour
{
    [SerializeField]protected float speed = 1f;

    protected virtual void Awake()
    {
        Destroy(this.gameObject, 25f);
    }

    private void FixedUpdate()
    {
        Move();
    }


    protected void Move()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.left);
    }
}
