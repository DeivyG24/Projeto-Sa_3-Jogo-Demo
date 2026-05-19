using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Sensitivity")]
    public float mouseSensitivity = 0.3f;

    [Header("Player")]
    public Transform playerBody;

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
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0f)
            return;

        // INPUT CRU
        float mouseX =
            Input.GetAxisRaw("Mouse X") *
            mouseSensitivity *
            100f *
            Time.deltaTime;

        float mouseY =
            Input.GetAxisRaw("Mouse Y") *
            mouseSensitivity *
            100f *
            Time.deltaTime;

        // SUAVIZA
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
}