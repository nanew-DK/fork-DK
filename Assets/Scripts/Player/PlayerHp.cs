using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    public GameObject heartPrefab;  // ��Ʈ ������
    public GameObject diePanel; // ��� �г�
    public int maxHP = 5;  // �ִ� ü��
    public int currentHP = 5;  // ���� ü��
    public List<GameObject> heartObjects = new List<GameObject>();  // ��Ʈ GameObject ����Ʈ

    private bool isInvincible = false;  // ���� ���� ����
    public float invincibilityDuration = 1f;  // ���� ���� ���� �ð�

    void Start()
    {
        CreateHearts();  // ���� ���� �� ��Ʈ ��ü ����
        UpdateHearts();  // �ʱ� ��Ʈ �̹��� ������Ʈ
    }

    // ��Ʈ �������� �̿��� ��Ʈ ��ü ����
    void CreateHearts()
    {
        for (int i = 0; i < maxHP; i++)
        {
            GameObject heart = Instantiate(heartPrefab, transform); // �������� �ν��Ͻ�ȭ�Ͽ� �θ�� ����
            heartObjects.Add(heart);  // ��Ʈ ��ü ����Ʈ�� �߰�
        }
    }

    // ü�� ���� ó��
    public void TakeDamage(int damage, Vector2 targetpos)
    {
        // ���� ������ ��� ������ ��ȿȭ
        if (isInvincible) return;

        currentHP -= damage;

        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }

        // ��Ʈ ������Ʈ
        UpdateHearts();

        // ���� ���� ����
        StartCoroutine(InvincibilityCoroutine());
    }

    // ��Ʈ ���� ������Ʈ
    void UpdateHearts()
    {
        for (int i = 0; i < heartObjects.Count; i++)
        {
            // ���� ü�¿� �´� ��Ʈ�� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
            heartObjects[i].SetActive(i < currentHP);
        }
    }

    // �÷��̾� ��� ó��
    public void Die()
    {
        Debug.Log("�÷��̾� ���");

        if (diePanel != null)
        {
            DiePanel panelScript = diePanel.GetComponent<DiePanel>();
            if (panelScript != null)
            {
                panelScript.Bravo6(); // �ִϸ��̼� ����
            }
            else
            {
                Debug.LogWarning("diePanel�� DiePanel ��ũ��Ʈ�� ����.");
            }
        }
        else
        {
            Debug.LogWarning("diePanel�� �Ҵ���� �ʾ���.");
        }

        Invoke("LoadGameOverScene", 0.5f);
    }

    // ���� ���� �� �ε�
    private void LoadGameOverScene()
    {
        SceneManager.LoadScene("Gameover");
    }

    // ���� ���� �ڷ�ƾ
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;  // ���� ���� ����
        yield return new WaitForSeconds(invincibilityDuration);  // ������ �ð� ���� ���
        isInvincible = false;  // ���� ���� ����
    }
}
