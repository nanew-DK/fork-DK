using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager 올바르게 추가

public class SpawnManager : MonoBehaviour
{
    public SavePoint[] points;
    public GameObject player;

    void Start()
    {
        player.transform.position = points[SaveLoad.currentSelectedSlot].transform.position;
    }

    public void GoBack()
    {
        SceneManager.LoadScene("SaveLoad");
    }
}
