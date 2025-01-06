using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction { UP,DOWN,LEFT,RIGHT,DASH,SWORD, GUN, RELOAD,SKILL_1,INTERACTION,PARRYING,KEYCOUNT}

public static class KeySetting { public static Dictionary<KeyAction, KeyCode> Keys = new Dictionary<KeyAction, KeyCode>(); }

public class KeyManager : MonoBehaviour
{
    KeyCode[] defaultKeys = new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftShift, KeyCode.Z, KeyCode.X, KeyCode.R, KeyCode.A, KeyCode.Space, KeyCode.F };
    private void Awake()
    {
        for (int i = 0;i<(int)KeyAction.KEYCOUNT;i++)
        {
            KeySetting.Keys.Add((KeyAction)i,defaultKeys[i]);
        }
    }

    private void OnGUI()
    {
        Event keyEvent=Event.current;
        if(keyEvent.isKey)
        {
            KeySetting.Keys[(KeyAction)key]=keyEvent.keyCode;
            key = -1;
        }
    }
    int key = -1;

    public void ChangeKey(int num)
    {
        key=num;
    }
}
