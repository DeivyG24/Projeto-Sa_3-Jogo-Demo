using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // 🔥 ADICIONADO

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 11f;
    public float sprintSpeed = 17f;
    public float crouchSpeed = 5f;

    [Header("Slide")]
    public float slideForce = 30f;
    public float slideTime = 1f;

    [Header("Jump")]
    public float jumpForce = 10f;
    public float gravityMultiplier = 2.5f;
    public float coyoteTime = 0.2f;

    [Header("Stamina")]
    public float maxStamina = 4.25f;
    public float staminaDrain = 1.8f;
    public float staminaRecovery = 1.2f;

    public Slider staminaSlider; // 🔥 ADICIONADO

    [Header("Camera")]
    public Transform cameraTransform;
    public float normalCamY = 1.6f;
    public float crouchCamY = 0.9f;
    public float crouchSmooth = 10f;

    [Header("Head Bob")]
    public float bobSpeed = 16f;
    public float bobAmount = 0.1f;

    [Header("Health")]
    public int maxHealth = 100;

    private int currentHealth;

    private Rigidbody rb;
    private CapsuleCollider col;

    private bool isGrounded;
    private float coyoteTimer;

    private float stamina;
    private bool isSliding;
    private float slideTimer;

    private float currentCamY;
    private float bobTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        stamina = maxStamina;
        currentHealth = maxHealth;
        currentCamY = normalCamY;

        // 🔥 ADICIONADO
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = stamina;
        }
    }

    void Update()
    {
        float x = 0;
        float z = 0;

        if (Keyboard.current.aKey.isPressed) x = -1;
        if (Keyboard.current.dKey.isPressed) x = 1;
        if (Keyboard.current.wKey.isPressed) z = 1;
        if (Keyboard.current.sKey.isPressed) z = -1;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * z + right * x;

        if (isGrounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        float speed = walkSpeed;

        bool isRunning = Keyboard.current.leftShiftKey.isPressed && stamina > 0;
        bool isCrouching = Keyboard.current.leftCtrlKey.isPressed;

        if (isRunning)
        {
            speed = sprintSpeed;
            stamina -= staminaDrain * Time.deltaTime;
        }
        else
        {
            stamina += staminaRecovery * Time.deltaTime;
        }

        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        // 🔥 ATUALIZA UI
        if (staminaSlider != null)
        {
            staminaSlider.value = stamina;
        }

        if (isRunning && isCrouching && isGrounded)
        {
            if (!isSliding)
            {
                isSliding = true;
                slideTimer = slideTime;

                Vector3 slideDir = move.magnitude > 0 ? move.normalized : transform.forward;

                rb.linearVelocity = new Vector3(
                    slideDir.x * slideForce,
                    rb.linearVelocity.y,
                    slideDir.z * slideForce
                );
            }
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;

            if (slideTimer <= 0)
            {
                isSliding = false;
            }
        }

        float targetCamY = isCrouching ? crouchCamY : normalCamY;
        currentCamY = Mathf.Lerp(currentCamY, targetCamY, crouchSmooth * Time.deltaTime);

        Vector3 camPos = cameraTransform.localPosition;
        camPos.y = currentCamY;

        if (move.magnitude > 0.1f && isGrounded)
        {
            bobTimer += Time.deltaTime * (isRunning ? bobSpeed * 1.5f : bobSpeed);
            camPos.y += Mathf.Sin(bobTimer) * bobAmount;
        }
        else
        {
            bobTimer = 0;
        }

        cameraTransform.localPosition = camPos;

        if (isCrouching && !isSliding)
            speed = crouchSpeed;

        if (!isSliding)
        {
            Vector3 velocity = move * speed;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && coyoteTimer > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.deltaTime;

            // EMPURRA LEVEMENTE PRA BAIXO
            rb.AddForce(Vector3.down * 5f);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Debug.Log("Morreu");
        }
    }
}