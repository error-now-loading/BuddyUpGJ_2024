using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModeListener : SceneSingleton<ModeListener>
{
    private List<char> codeStorage = null;
    private string passcode = "SHROOLOO5EVER";



    private void Start()
    {
        codeStorage = new List<char>();
    }

    private void Update()
    {
        HandleModeSwitch();
    }

    private void HandleModeSwitch()
    {
        foreach (var key in Keyboard.current.allKeys)
        {
            if (key.wasPressedThisFrame)
            {
                Debug.Log($"{key.keyCode}");
            }
        }
    }

    private bool IsCodeEntered()
    {
        if (codeStorage.Count != passcode.Length)
        {
            return false;
        }

        else
        {
            string checker = string.Concat(codeStorage);

            if (checker == passcode)
            {
                return true;
            }

            return false;
        }
    }
}