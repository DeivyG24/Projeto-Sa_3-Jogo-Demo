using UnityEngine;

public class CameraWallFix : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    public float mouseSensitivity = 0.3f;

    [Header("Wall Fix")]
    public float radius = 0.2f;
    public float distance = 0.3f;
    public LayerMask wallMask;

    [Header("Camera Rotation")]
    public Transform playerBody;

    private Vector3 originalLocalPos;

    private float xRotation = 0f;

    private Vector3 currentVelocity;

    void Start()
    {
        originalLocalPos = transform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // PAUSE
        if (Time.timeScale == 0f)
            return;

        // MOUSE INPUT
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

        // ROTAÇÃO VERTICAL
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation =
            Quaternion.Euler(xRotation, 0f, 0f);

        // ROTAÇÃO HORIZONTAL
        playerBody.Rotate(Vector3.up * mouseX);

        // WALL FIX
        Vector3 origin = transform.parent.position;
        Vector3 direction = transform.forward;

        RaycastHit hit;

        float currentY = transform.localPosition.y;

        Vector3 targetPos;

        if (Physics.SphereCast(
            origin,
            radius,
            direction,
            out hit,
            distance,
            wallMask))
        {
            float pushBack = distance - hit.distance;

            targetPos = new Vector3(
                originalLocalPos.x,
                currentY,
                originalLocalPos.z - pushBack
            );
        }
        else
        {
            targetPos = new Vector3(
                originalLocalPos.x,
                currentY,
                originalLocalPos.z
            );
        }

        // SUAVIZA
        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetPos,
            ref currentVelocity,
            0.03f
        );
    }
}