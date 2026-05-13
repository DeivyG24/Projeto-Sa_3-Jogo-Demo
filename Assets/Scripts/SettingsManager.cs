using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider sensitivitySlider;

    public CameraWallFix mouseLook;

    void Start()
    {
        sensitivitySlider.value = mouseLook.mouseSensitivity;

        sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
    }

    public void SetSensitivity(float value)
    {
        mouseLook.mouseSensitivity = value;
    }
}