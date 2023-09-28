using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player stats
    public float basePlayerSpeed, currentPlayerSpeed, playerLife, force, pushCooldown;
    public float nextPush = 0;

    // Player respawn point
    public GameObject respawn;

    public GameObject healthBar;

    private float healthBarSectionSize;

    // Controls variables
    private Vector2 movementInput;
    private Vector2 aimInput;
    private bool leftShoulder, rightShoulder;

    void Start()
    {
        currentPlayerSpeed = basePlayerSpeed;
        respawn = GameObject.FindWithTag("Respawn");
        healthBarSectionSize = healthBar.transform.localScale.x / 3;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 aimDirection = new Vector3(aimInput.x, 0, aimInput.y);

        // Movement
        transform.position += new Vector3(movementInput.x * currentPlayerSpeed * Time.deltaTime, movementInput.y * currentPlayerSpeed * Time.deltaTime, 0);

        // Setting up raycasts for pushing players
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        Debug.DrawRay(transform.position, transform.right);

        // If Raycast Hits an object, and that object is tagged player, and its less than or equal to 1 unit away from the player, and left shoulder is pressed
        if(hit.collider && hit.collider.tag == "Player" && hit.distance <= 1f && leftShoulder && Time.time > nextPush)
        {
            Debug.Log("Pushed");
            nextPush = Time.time + pushCooldown;

            // Find direction for other object to be pushed in
            var direction = (transform.position - hit.transform.position).normalized;

            // Add Force to that object in the desired direction
            hit.transform.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * force, ForceMode2D.Impulse);

            // Reset left shoulder
            leftShoulder = false;
        } 
        else if (!hit.collider && leftShoulder && Time.time > nextPush)
        {
            Debug.Log("Miss");
            nextPush = Time.time + pushCooldown;
            leftShoulder = false;
        }

        // Controls look direction with right joystick
        if (aimDirection != Vector3.zero)
        {
            float heading = Mathf.Atan2(aimInput.y, aimInput.x);
            transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext ctx)
    {
        aimInput = ctx.ReadValue<Vector2>();
    }

    public void OnPush(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            leftShoulder = true;
        }
    }

    public void OnPowerup(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            rightShoulder = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Arena")
        {
            Debug.Log("Killed");
            transform.position = respawn.transform.position;
            playerLife -= 1;
            var newHealthBarSize = healthBarSectionSize * playerLife;
            healthBar.transform.localScale = new Vector3(newHealthBarSize, 1, 0);
        }
    }
}
