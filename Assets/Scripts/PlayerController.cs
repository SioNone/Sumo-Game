using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float basePlayerSpeed, currentPlayerSpeed, playerDashForce, playerDashCooldown, nextDash, playerLife;
    public GameObject respawn;
    private Rigidbody2D playerRigidbody;

    void Start()
    {
        currentPlayerSpeed = basePlayerSpeed;
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal") * currentPlayerSpeed * Time.deltaTime, Input.GetAxis("Vertical") * currentPlayerSpeed * Time.deltaTime, 0);

        if (Input.GetKey("joystick button 1") && Time.time > nextDash)
        {
            nextDash = Time.time + playerDashCooldown;
            Debug.Log("X Pressed");
            playerRigidbody.AddForce(transform.up * playerDashForce, ForceMode2D.Impulse);
        } 
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
