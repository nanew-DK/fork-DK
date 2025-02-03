using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyAction { UP, DOWN, LEFT, RIGHT, DASH, SWORD, GUN, RELOAD, SKILL_1, INTERACTION, PARRYING, KEYCOUNT }

public static class KeySetting
{
    public static Dictionary<KeyAction, KeyCode> Keys = new Dictionary<KeyAction, KeyCode>();
}

public class KeyManager : MonoBehaviour
{
    KeyCode[] defaultKeys = new KeyCode[]
    {
        KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow,
        KeyCode.LeftShift, KeyCode.Z, KeyCode.X, KeyCode.R, KeyCode.A, KeyCode.Space, KeyCode.F
    };

    public GameObject keyPanel;
    private int key = -1;

    private void Awake()
    {
        InitializeKeyBindings();
    }

    private void InitializeKeyBindings()
    {
        for (int i = 0; i < (int)KeyAction.KEYCOUNT; i++)
        {
            KeyAction action = (KeyAction)i;

            if (!KeySetting.Keys.ContainsKey(action))
            {
                KeySetting.Keys.Add(action, defaultKeys[i]);
            }
            else
            {
                Debug.LogWarning($"KeyAction {action} already exists in the dictionary.");
            }
        }
    }

    private void OnGUI()
    {
        Event keyEvent = Event.current;

        if (keyEvent.isKey && key != -1)
        {
            KeyAction action = (KeyAction)key;

            if (KeySetting.Keys.ContainsValue(keyEvent.keyCode))
            {
                Debug.LogWarning($"Key {keyEvent.keyCode} is already assigned to another action.");
            }
            else
            {
                KeySetting.Keys[action] = keyEvent.keyCode;
                Debug.Log($"Key for {action} changed to {keyEvent.keyCode}");
                key = -1;
            }
        }
    }

    public void ChangeKey(int num)
    {
        key = num;
    }

    
}
