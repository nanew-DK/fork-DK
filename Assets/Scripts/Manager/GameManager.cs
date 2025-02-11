using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("1LevelDesign");
    }

    public void SaveLoad()
    {
        SceneManager.LoadScene("SaveLoad");
    }
}