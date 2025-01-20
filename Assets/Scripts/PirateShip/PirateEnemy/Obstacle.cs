using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    float screenMinX, screenMaxX, screenMinY, screenMaxY;

    [SerializeField] protected float speed = 1f;

    void Awake()
    {
        Camera cam = Camera.main;
        float height = cam.orthographicSize * 2.0f;
        float width = height * cam.aspect;

        screenMinX = -width / 2.0f;
        StartCoroutine(CallCheckBounds());
    }

    void CheckBounds()
    {
        if (transform.position.x < screenMinX - 1)
        {
            Destroy(this.gameObject);
        }

    }


    private void FixedUpdate()
    {
        Move();
    }


    protected void Move()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.left);
    }

    IEnumerator CallCheckBounds()
    {
        while (true)
        {
            CheckBounds();
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this);
        if(collision.transform.CompareTag("Player"))
        {
            //PlayerShip playerScript = collision.GetComponent<PlayerShip>();
            //playerScript.TakeDamage(1f);
        }
    }
}
