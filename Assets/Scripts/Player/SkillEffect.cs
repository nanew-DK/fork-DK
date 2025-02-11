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

        this.gameObject.SetActive(false);  // 시작할 때 패널을 꺼둠
    }

    public void PlaySkillEffect()
    {
        this.gameObject.SetActive(true);  // 패널 보이게 하기
        anim.SetTrigger("Play"); // 애니메이션 실행

        // 일정 시간 후 패널 숨기기
        Invoke("EndEffect", 1.35f);
    }
    private void EndEffect()
    {
        this.gameObject.SetActive(false);
    }
}