using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PirateManager : MonoBehaviour
{
    // Start is called before the first frame update

    float clearRate = 0f;
    float targetRate = 100f;
    [SerializeField] private float targetTime = 10f;
    private float ratePerFrame;

    private void Awake()
    {
        clearRate = 0f;
        ratePerFrame=targetRate/targetTime;
    }
    private void Start()
    {
        clearRate = 0f;
        ratePerFrame = targetRate / targetTime;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(clearRate>=targetRate)
        {
            Debug.Log("clear");
            //SceneManager.LoadScene("BossScene");//º¸½º¾ÀÀÌ¶û ¿¬°áÇØ¾ßµÊ
        }
        else
        {
            clearRate += ratePerFrame*Time.deltaTime;

        }
    }

    public void ReStart()
    {
        clearRate = 0f;
        //or
        //SceneManager.LoadScene("PirateScene");
    }
}
