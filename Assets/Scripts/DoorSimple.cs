using UnityEngine;
using UnityEngine.InputSystem;

public class DoorSmart : MonoBehaviour
{
    public Transform player;
    public float interactDistance = 3f;

    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;
    private float targetY;
    private float startY;

    void Start()
    {
        startY = transform.eulerAngles.y;
        targetY = startY;
    }

    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);

        if (dist < interactDistance)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                isOpen = !isOpen;

                if (isOpen)
                {
                    // 🔥 DETECTA LADO DO PLAYER
                    Vector3 dirToPlayer = (player.position - transform.position).normalized;
                    float dot = Vector3.Dot(transform.forward, dirToPlayer);

                    if (dot > 0)
                        targetY = startY + openAngle;   // abre pra frente
                    else
                        targetY = startY - openAngle;   // abre pra trás
                }
                else
                {
                    targetY = startY;
                }
            }
        }

        float y = Mathf.LerpAngle(transform.eulerAngles.y, targetY, Time.deltaTime * speed);
        transform.rotation = Quaternion.Euler(0, y, 0);
    }
}