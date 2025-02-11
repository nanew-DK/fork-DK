using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PirateManager : MonoBehaviour
{
    [Header("Hearts")]
    [SerializeField] public GameObject heart1;//가장 왼쪽 하트
    [SerializeField] public GameObject heart2;
    [SerializeField] public GameObject heart3;
    [SerializeField] public GameObject heart4;
    [SerializeField] public GameObject heart5;//가장 오른쪽 하트
    int heart = 5;



    [Header("ClearRate")]
    [SerializeField] float clearRate = 0f;
    [SerializeField] private float targetTime = 10f;
    private float ratePerFrame;
    private float targetRate = 100f;


    

    private void Awake()
    {
        clearRate = 0f;
        heart = 5;
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
            //SceneManager.LoadScene("BossScene");//보스씬이랑 연결해야됨
        }
        else
        {
            clearRate += ratePerFrame*Time.deltaTime;
        }
    }

    public void ReStart()
    {
        clearRate = 0f;
        heart = 5;
        SceneManager.LoadScene("PirateShip");
    }
    public void UpHeart()
    {
        if (heart == 5) return;
        heart++;
        CheckHeart();
    }
    public void DownHeart()
    {
        heart--;
        CheckHeart();
        if (heart == 0) ReStart();
    }
    public int GetHeart()
    {
        return heart;
    }
    void CheckHeart()
    {
        heart1.SetActive(heart >= 1);
        heart2.SetActive(heart >= 2);
        heart3.SetActive(heart >= 3);
        heart4.SetActive(heart >= 4);
        heart5.SetActive(heart >= 5);
    }
}
