using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Find : MonoBehaviour
{
    public void FindAndOpenPanel()
    {
        GameObject ui= GameObject.Find("UIManager");
        UiManager uiManager = ui.GetComponent<UiManager>();
        uiManager.OpenSetting();
    }
}
