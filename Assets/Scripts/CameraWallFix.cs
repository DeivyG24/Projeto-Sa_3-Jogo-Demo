using UnityEngine;

public class CameraWallFix : MonoBehaviour
{
    public float radius = 0.2f;
    public float distance = 0.3f;
    public LayerMask wallMask;

    private Vector3 originalLocalPos;

    void Start()
    {
        originalLocalPos = transform.localPosition;
    }

    void LateUpdate()
    {
        Vector3 origin = transform.parent.position;
        Vector3 direction = transform.forward;

        RaycastHit hit;

        float currentY = transform.localPosition.y; // 👈 pega altura atual

        if (Physics.SphereCast(origin, radius, direction, out hit, distance, wallMask))
        {
            float pushBack = distance - hit.distance;

            transform.localPosition = new Vector3(
                originalLocalPos.x,
                currentY, // 👈 mantém Y correto (crouch funciona)
                originalLocalPos.z - pushBack
            );
        }
        else
        {
            transform.localPosition = new Vector3(
                originalLocalPos.x,
                currentY, // 👈 mantém Y
                originalLocalPos.z
            );
        }
    }
}