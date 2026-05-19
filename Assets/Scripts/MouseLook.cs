using UnityEngine;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float mouseSensitivity = 2f;

    [Header("Player")]
    public Transform playerBody;

    [Header("UI")]
    public Slider sensitivitySlider;

    float xRotation = 0f;

    // SMOOTH
    float smoothX;
    float smoothY;

    float currentMouseX;
    float currentMouseY;

    public float smoothTime = 0.05f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // CONFIGURA SLIDER
        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = 0.5f;
            sensitivitySlider.maxValue = 10f;

            sensitivitySlider.value = mouseSensitivity;

            sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
        }
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0f)
            return;

        // INPUT
        float mouseX =
            Input.GetAxisRaw("Mouse X") *
            mouseSensitivity;

        float mouseY =
            Input.GetAxisRaw("Mouse Y") *
            mouseSensitivity;

        // SMOOTH
        currentMouseX = Mathf.SmoothDamp(
            currentMouseX,
            mouseX,
            ref smoothX,
            smoothTime
        );

        currentMouseY = Mathf.SmoothDamp(
            currentMouseY,
            mouseY,
            ref smoothY,
            smoothTime
        );

        // VERTICAL
        xRotation -= currentMouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);

        // HORIZONTAL
        playerBody.Rotate(Vector3.up * currentMouseX);
    }

    // MUDA SENSIBILIDADE
    public void SetSensitivity(float value)
    {
        mouseSensitivity = value;
    }
}