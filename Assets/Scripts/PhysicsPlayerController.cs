using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PhysicsPlayerController : MonoBehaviour
{
    // Movement Variables
    public float basePlayerSpeed;
    public float currentPlayerSpeed;

    // Player Stats
    public float playerLife;
    public float pushForce;
    public float pushCooldown;

    private float nextPush;

    private Vector2 aimDirection;
    private Vector2 movement;
    public Rigidbody2D rgdBody;

    private bool playerInRange;
    private Transform playerPresent;

    // Control stuff
    private Vector2 movementInput, aimInput;
    private bool leftShoulder, rightShoulder;

    void Start()
    {
        currentPlayerSpeed = basePlayerSpeed;
        rgdBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input is checked in Update
        aimDirection = new Vector2(aimInput.x, aimInput.y);
        movement = new Vector2(movementInput.x, movementInput.y);
    }

    void FixedUpdate()
    {
        // Physics based movement is applied in Fixed Update
        // Movement
        rgdBody.velocity = new Vector2(movement.x * basePlayerSpeed * Time.deltaTime, movement.y * basePlayerSpeed * Time.deltaTime);

        // Aim
        if (aimDirection != Vector2.zero)
        {
            float heading = Mathf.Atan2(aimInput.y, aimInput.x);
            transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
        }
    }

    // What will happen when player moves left joystick
    public void OnMove(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    // What will happen when player moves right joystick
    public void OnAim(InputAction.CallbackContext ctx)
    {
        aimInput = ctx.ReadValue<Vector2>();
    }

    public void OnPush(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && playerInRange && Time.time > nextPush)
        {
            nextPush = Time.time + pushCooldown;

            var direction = (transform.position - playerPresent.position).normalized;
            playerPresent.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * (pushForce * 1.5f), ForceMode2D.Impulse);
        }
    }

    public void OnPowerup(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            leftShoulder = true;
            Debug.Log("rightShoulder");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
            playerPresent = other.transform;
        }
        else
        {
            playerInRange = false;
        }
    }
}
