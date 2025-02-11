using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEffect : MonoBehaviour
{
    public Animator anim;
    private Color color;


    void Start()
    {
        if (anim == null)
            anim = GetComponent<Animator>();

        this.gameObject.SetActive(false);  // ������ �� �г��� ����
    }

    public void PlaySkillEffect()
    {
        this.gameObject.SetActive(true);  // �г� ���̰� �ϱ�
        anim.SetTrigger("Play"); // �ִϸ��̼� ����

        // ���� �ð� �� �г� �����
        Invoke("EndEffect", 1.35f);
    }
    private void EndEffect()
    {
        this.gameObject.SetActive(false);
    }
}