using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager 올바르게 추가

public class SpawnManager : MonoBehaviour
{
    public SavePoint[] points;
    public GameObject player;

    public static class SaveLoad
    {
        public static int currentSelectedSlot = 0; // 기본값 설정
    }


    void Start()
    {
        player.transform.position = points[SaveLoad.currentSelectedSlot].transform.position;
    }

    public void GoBack()
    {
        SceneManager.LoadScene("SaveLoad");
    }
}
