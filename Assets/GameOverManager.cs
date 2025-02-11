using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject arrowMark;

    public static class SaveLoad
    {
        public static int currentSelectedSlot = 0;
    }

    public static class SavePoint
    {
        public static int diePoint = 0;
    }

    private void Start()
    {
        arrowMark.SetActive(false);
    }

    public void Redo()
    {
        SaveLoad.currentSelectedSlot = SavePoint.diePoint - 1;
        SceneManager.LoadScene("1LevelDesign");
    }

    public void ArrowMark()
    {
        arrowMark.SetActive(true);
    }
    public void NoArrowMark()
    {
        arrowMark.SetActive(false);
    }
}
