using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player stats
    public float basePlayerSpeed, currentPlayerSpeed, buffPlayerSpeed, playerLife, force, pushCooldown;
    public float nextPush = 0;

    // Player Pickup Variables
    public bool forcePickup, speedPickup;
    public float speedPickUpDuration, speedPickUpLength;

    // Player respawn point
    public GameObject respawn;

    public GameObject healthBar;
    public GameObject pickupIndicator;

    private float healthBarSectionSize;

    // Controls variables
    private Vector2 movementInput;
    private Vector2 aimInput;
    private bool leftShoulder, rightShoulder;

    void Start()
    {
        currentPlayerSpeed = basePlayerSpeed;
        buffPlayerSpeed = basePlayerSpeed * 1.25f;
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

        // Pickup Indicator Stuff

        if (speedPickup || forcePickup)
        {
            pickupIndicator.SetActive(true);
        } 
        else
        {
            pickupIndicator.SetActive(false);
        }

        // Speed Pickup Stuff

        if (speedPickup)
        {
            currentPlayerSpeed = buffPlayerSpeed;
        } 
        
        if (Time.time >= speedPickUpLength)
        {
            currentPlayerSpeed = basePlayerSpeed;
            speedPickup = false;
        }

        // If Raycast Hits an object, and that object is tagged player, and its less than or equal to 1 unit away from the player, and left shoulder is pressed
        if(hit.collider && hit.collider.tag == "Player" && hit.distance <= 1f && leftShoulder && Time.time > nextPush)
        {
            nextPush = Time.time + pushCooldown;

            // Find direction for other object to be pushed in
            var direction = (transform.position - hit.transform.position).normalized;

            // If the player is buffed by a pickup that increases their push force
            if (forcePickup)
            {
                hit.transform.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * (force * 1.5f), ForceMode2D.Impulse);
                Debug.Log("Extra Force");
                forcePickup = false;
            } 
            else
            {
                // Add Force to that object in the desired direction
                hit.transform.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * force, ForceMode2D.Impulse);
                Debug.Log("Normal Force");
            }

            // Reset left shoulder
            leftShoulder = false;
        } 
        else if (!hit.collider && leftShoulder && Time.time > nextPush)
        {
            nextPush = Time.time + pushCooldown;
            leftShoulder = false;
        }

        // Controls look direction with right joystick
        if (aimDirection != Vector3.zero)
        {
            float heading = Mathf.Atan2(aimInput.y, aimInput.x);
            transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);
        }

        // If player has no health destroy them and remove 1 player from the player count
        if (playerLife <= 0)
        {
            Destroy(gameObject);
            PlayerLoader.playersRemain--;
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

    // What will happen when player presses left shoulder
    public void OnPush(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            leftShoulder = true;
        }
    }

    // What will happen when player presses right shoulder
    public void OnPowerup(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            rightShoulder = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // What will happen when player is pushed out of ring
        if (other.gameObject.tag == "Arena")
        {
            // Set its position to origin
            transform.position = respawn.transform.position;

            // Sets velocity to 0 so none is conserved after respawn
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            // Remove 1 health
            playerLife -= 1;

            // Change healthbar size
            var newHealthBarSize = healthBarSectionSize * playerLife;
            healthBar.transform.localScale = new Vector3(newHealthBarSize, 1, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Force Pickup")
        {
            forcePickup = true;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Speed Pickup")
        {
            speedPickUpLength = Time.time + speedPickUpDuration;
            speedPickup = true;
            Destroy(other.gameObject);
        }
    }
}
