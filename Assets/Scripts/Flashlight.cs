using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;
    private bool isOn = true;

    void Start()
    {
        flashlight.enabled = isOn;
    }

    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            isOn = !isOn;
            flashlight.enabled = isOn;
        }
    }
    
}