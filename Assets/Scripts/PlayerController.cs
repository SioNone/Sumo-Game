using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float basePlayerSpeed, currentPlayerSpeed, playerDashForce, playerDashCooldown, nextDash, playerLife;
    public GameObject respawn;
    private Rigidbody2D playerRigidbody;
    private Vector2 movementInput;

    void Start()
    {
        currentPlayerSpeed = basePlayerSpeed;
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(movementInput.x * currentPlayerSpeed * Time.deltaTime, movementInput.y * currentPlayerSpeed * Time.deltaTime, 0);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
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
