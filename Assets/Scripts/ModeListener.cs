using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ModeListener : SceneSingleton<ModeListener>
{
    private List<Key> codeStorage = new List<Key>() { Key.S, Key.H, Key.R, Key.O, Key.O, Key.L, Key.O, Key.O, Key.Digit5, Key.E, Key.V, Key.E, Key.R };
    private readonly List<Key> passcode = new List<Key>() { Key.S, Key.H, Key.R, Key.O, Key.O, Key.L, Key.O, Key.O, Key.Digit5, Key.E, Key.V, Key.E, Key.R };
    private bool codeEntered = false;



    private void Start()
    {
        codeStorage = new List<Key>();
    }

    private void Update()
    {
        if (!codeEntered)
        {
            HandleModeSwitch();
        }
    }

    private void HandleModeSwitch()
    {
        for (int i = 0; i < Keyboard.current.allKeys.Count; i++)
        {
            KeyControl curKey = Keyboard.current.allKeys[i];
            if (curKey.wasPressedThisFrame)
            {
                AddKeyToBuffer(curKey);
            }
        }

        if (IsCodeEntered())
        {
            // Activate Shrooloo mode flag
            SaveDataUtility.SaveBool(SaveDataUtility.SHROOLOO_MODE_KEY, true);

            codeEntered = true;
        }
    }

    private void AddKeyToBuffer(KeyControl curKey)
    {
        codeStorage.Add(curKey.keyCode);

        // Maintain buffer length
        if (codeStorage.Count > passcode.Count)
        {
            codeStorage.RemoveAt(0);
        }
    }

    private bool IsCodeEntered()
    {
        if (codeStorage.Count != passcode.Count)
        {
            return false;
        }

        for (int i = 0; i < passcode.Count; i++)
        {
            if (codeStorage[i] != passcode[i])
            {
                return false;
            }
        }

        return true;
    }
}