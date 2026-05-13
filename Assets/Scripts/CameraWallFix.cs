using UnityEngine;

public class CameraWallFix : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    public float mouseSensitivity = 2f;

    [Header("Wall Fix")]
    public float radius = 0.2f;
    public float distance = 0.3f;
    public LayerMask wallMask;

    [Header("Camera Rotation")]
    public Transform playerBody;

    private Vector3 originalLocalPos;

    float xRotation = 0f;

    void Start()
    {
        originalLocalPos = transform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // NÃO MOVE A CÂMERA NO PAUSE
        if (Time.timeScale == 0f)
            return;

        // MOVIMENTO DO MOUSE
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ROTAÇÃO VERTICAL
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // ROTAÇÃO HORIZONTAL
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void LateUpdate()
    {
        Vector3 origin = transform.parent.position;
        Vector3 direction = transform.forward;

        RaycastHit hit;

        // PEGA ALTURA ATUAL (IMPORTANTE PRO CROUCH)
        float currentY = transform.localPosition.y;

        // DETECTA PAREDE
        if (Physics.SphereCast(origin, radius, direction, out hit, distance, wallMask))
        {
            float pushBack = distance - hit.distance;

            transform.localPosition = new Vector3(
                originalLocalPos.x,
                currentY,
                originalLocalPos.z - pushBack
            );
        }
        else
        {
            transform.localPosition = new Vector3(
                originalLocalPos.x,
                currentY,
                originalLocalPos.z
            );
        }
    }
}