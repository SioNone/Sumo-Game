using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player stats
    public float basePlayerSpeed, currentPlayerSpeed, playerLife, force;

    // Player respawn point
    public GameObject respawn;

    // Controls variables
    private Vector2 movementInput;
    private Vector2 aimInput;
    private bool leftShoulder;

    void Start()
    {
        currentPlayerSpeed = basePlayerSpeed;
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
        if(hit.collider && hit.collider.tag == "Player" && hit.distance <= 1f && leftShoulder)
        {
            Debug.Log("Pushed");

            // Find direction for other object to be pushed in
            var direction = (transform.position - hit.transform.position).normalized;

            // Add Force to that object in the desired direction
            hit.transform.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * force, ForceMode2D.Impulse);

            // Reset left shoulder
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
        leftShoulder = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boundary")
        {
            Debug.Log("Killed");
            transform.position = respawn.transform.position;
            playerLife -= 1;
        }

        if (other.gameObject.tag == "Arena")
        {
            currentPlayerSpeed = basePlayerSpeed;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Arena")
        {
            currentPlayerSpeed = basePlayerSpeed * 0.6f;
        }
    }
}
