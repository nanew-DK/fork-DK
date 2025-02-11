/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public GameObject CannonBall;

    [SerializeField] float shipSpeed = 6f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float cannonSpeed = 15f;
    //[SerializeField] float timer = 60f;
    private Quaternion originalRotation;
    private float maxY = 3.3f;
    private float maxRotationAngle = 35f;
    private bool Wait = true;

    Vector2 currentPos;

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        currentPos = transform.position;
        //��ġ��ȯ
        if (Input.GetKey(KeySetting.Keys[KeyAction.UP]))
        {
            if (currentPos.y <= maxY)
            {
                transform.position += Vector3.up * shipSpeed * Time.deltaTime;

                // ���� Z�� ȸ���� �������� (-90 ����)
                float currentZAngle = transform.rotation.eulerAngles.z;
                currentZAngle = currentZAngle > 180 ? currentZAngle - 360 : currentZAngle;

                if (currentZAngle < -60)
                {
                    transform.Rotate(new Vector3(0, 0, 30) * rotationSpeed * Time.deltaTime);
                }
            }
        }
        else if (Input.GetKey(KeySetting.Keys[KeyAction.DOWN]))
        {
            if (currentPos.y >= -maxY)
            {
                transform.position += Vector3.down * shipSpeed * Time.deltaTime;

                // ���� Z�� ȸ���� �������� (-90 ����)
                float currentZAngle = transform.rotation.eulerAngles.z;
                currentZAngle = currentZAngle > 180 ? currentZAngle - 360 : currentZAngle;

                if (currentZAngle > -120)
                {
                    transform.Rotate(new Vector3(0, 0, -30) * rotationSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Z) && Wait)
        {
            CannonShoot();
            StartCoroutine("WaitAttack");
        }

        if (currentPos.y >= maxY || currentPos.y <= -maxY)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void CannonShoot()
    {
        Quaternion currentRotation = transform.rotation;

        // ������ ����
        GameObject CB = Instantiate(CannonBall, transform.position, currentRotation);

        // �������� �ڽ��� �������� �̵��ϵ��� ����
        Rigidbody2D rb = CB.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.up * cannonSpeed;
        }

        Debug.Log("���� �߻�!");
        Destroy(CB, 3f);
    }

    private IEnumerator WaitAttack()
    {
        Wait = false;
        yield return new WaitForSeconds(1f);
        Wait = true;
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public GameObject CannonBall;

    [SerializeField] float shipSpeed = 6f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float cannonSpeed = 15f;
    private Quaternion originalRotation;
    private float maxY = 3.3f;
    private float maxRotationAngle = 25f;
    private bool Wait = true;

    Vector2 currentPos;

    private void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        currentPos = transform.position;

        if (Input.GetKey(KeySetting.Keys[KeyAction.UP]))
        {
            if (currentPos.y <= maxY)
            {
                transform.position += Vector3.up * shipSpeed * Time.deltaTime;
                float currentZAngle = transform.rotation.eulerAngles.z;
                currentZAngle = currentZAngle > 180 ? currentZAngle - 360 : currentZAngle;

                if (currentZAngle < 35 && currentZAngle > -35)
                {
                    transform.Rotate(new Vector3(0, 0, 30) * rotationSpeed * Time.deltaTime);
                }
            }
        }
        else if (Input.GetKey(KeySetting.Keys[KeyAction.DOWN]))
        {
            if (currentPos.y >= -maxY)
            {
                transform.position += Vector3.down * shipSpeed * Time.deltaTime;
                float currentZAngle = transform.rotation.eulerAngles.z;
                currentZAngle = currentZAngle > 180 ? currentZAngle - 360 : currentZAngle;

                if (currentZAngle < 35 && currentZAngle > -35) 
                {
                    transform.Rotate(new Vector3(0, 0, -30) * rotationSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Z) && Wait)
        {
            CannonShoot();
            StartCoroutine("WaitAttack");
        }

        if (currentPos.y >= maxY || currentPos.y <= -maxY)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void CannonShoot()
    {
        Quaternion currentRotation = transform.rotation;
        GameObject CB = Instantiate(CannonBall, transform.position, currentRotation);
        Rigidbody2D rb = CB.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.right * cannonSpeed;
        }
        Debug.Log("���� �߻�!");
        Destroy(CB, 3f);
    }

    private IEnumerator WaitAttack()
    {
        Wait = false;
        yield return new WaitForSeconds(1f);
        Wait = true;
    }
}
