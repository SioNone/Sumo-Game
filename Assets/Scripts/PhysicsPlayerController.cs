using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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
    private float currentPushCooldown;
    private float currentPlayerLife;

    // Player movement and aiming
    private Vector2 aimDirection;
    private Vector2 movement;
    public Rigidbody2D rgdBody;

    // Player collision detection
    private bool playerInRange;
    private Transform playerPresent;

    // Pickup Stuff
    public GameObject pickupIndicator;

    private bool buffed;
    private bool speedPickup;
    private bool forcePickup;

    public float speedPickUpDuration;
    public float speedPickUpLength;

    // Control stuff
    private Vector2 movementInput, aimInput;
    private bool leftShoulder, rightShoulder;

    // Particle Stuff
    public GameObject sandParticles;

    // Respawn
    public GameObject respawn;

    // Cooldown Bar
    public Slider cooldownBar;

    // Health Bar
    public Slider healthBar;

    // Stun
    public bool isStunned = false;
    public float stunRecovery;

    // SFX
    [SerializeField]
    private AudioClip pushSFX;

    [SerializeField]
    private AudioClip deathSFX;

    [SerializeField]
    private AudioClip pickupSFX;

    // Player Number
    public int playerNumber;

    // Animator
    private Animator playerAnim;

    public Leaderboard leaderboard;

    public string playerAnimator;

    private GameObject otherPlayer;

    void Start()
    {
        // Set Current Player Speed to Base Player Speed by Default
        currentPlayerSpeed = basePlayerSpeed;

        // Set Current Player Life to Base Player Life by Default
        currentPlayerLife = playerLife;

        // Get Rigidbody
        rgdBody = GetComponent<Rigidbody2D>();

        // Get Animator
        playerAnim = GetComponent<Animator>();

        // Set Player Anim Bool
        playerAnim.SetBool(playerAnimator, true);

        // Get Respawn Point
        respawn = GameObject.FindWithTag("Respawn");

        leaderboard = GameObject.FindWithTag("Leaderboard").GetComponent<Leaderboard>();

        sandParticles.SetActive(false);
    }

    void Update()
    {
        // Input is checked in Update
        aimDirection = new Vector2(aimInput.x, aimInput.y);
        movement = new Vector2(movementInput.x, movementInput.y);

        // Movement
        if (movement != Vector2.zero && !isStunned && Countdown.gameStarted)
        {
            transform.position += new Vector3(movementInput.x * currentPlayerSpeed * Time.deltaTime, movementInput.y * currentPlayerSpeed * Time.deltaTime, 0);
            sandParticles.SetActive(true);
        }
        else if (movement == Vector2.zero)
        {
            sandParticles.SetActive(false);
        } 
        else if (isStunned)
        {
            StartCoroutine(Recovery(stunRecovery));
        }

        if (speedPickup || forcePickup)
        {
            pickupIndicator.SetActive(true);
        }
        else
        {
            pickupIndicator.SetActive(false);
        }

        // Cooldown Bar Statements
        if (nextPush >= Time.time)
        {
            currentPushCooldown += Time.deltaTime;
            currentPushCooldown = Mathf.Clamp(currentPushCooldown, 0.0f, pushCooldown);
        }
        
        cooldownBar.value = currentPushCooldown / pushCooldown;

        if (cooldownBar.value >= 0.95)
        {
            cooldownBar.gameObject.SetActive(false);
        } 
        else
        {
            cooldownBar.gameObject.SetActive(true);
        }

        if (speedPickup || forcePickup)
        {
            pickupIndicator.SetActive(true);
        }
        else
        {
            pickupIndicator.SetActive(false);
        }

        // Player Healthbar Stuff
        healthBar.value = currentPlayerLife / playerLife;

        // Player Death
        if (currentPlayerLife <= 0)
        {
            Destroy(gameObject);
            PlayerLoader.playersRemain--;
            leaderboard.UpdateLeaderboard(playerNumber.ToString());
        }
    }

    void FixedUpdate()
    {
        // Physics based movement is applied in Fixed Update
        // Movement
        //if (movement != Vector2.zero)
        //{
            //rgdBody.velocity += new Vector2(movement.x * basePlayerSpeed * Time.deltaTime, movement.y * basePlayerSpeed * Time.deltaTime);
        //}

        // Aim
        if (aimDirection != Vector2.zero)
        {
            float aimHeading = Mathf.Atan2(-aimInput.y, -aimInput.x);
            transform.rotation = Quaternion.Euler(0f, 0f, aimHeading * Mathf.Rad2Deg);
        }
    }

    public IEnumerator Recovery(float recovery)
    {
        yield return new WaitForSeconds(recovery);
        isStunned = false;
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
            SFXManager.instance.PlaySFX(pushSFX, transform, 0.75f);
            playerAnim.SetTrigger("isPushing");

            otherPlayer.GetComponent<PhysicsPlayerController>().isStunned = true;

            nextPush = Time.time + pushCooldown;
            currentPushCooldown = 0f;

            var direction = (transform.position - playerPresent.position).normalized;
            playerPresent.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * (pushForce * 1.5f), ForceMode2D.Impulse);
        }
    }

    public void OnPowerup(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            leftShoulder = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            otherPlayer = other.gameObject;
            playerInRange = true;
            playerPresent = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // What will happen when player is pushed out of ring
        if (other.gameObject.tag == "Arena")
        {
            SFXManager.instance.PlaySFX(deathSFX, transform, 1f);

            // Set its position to origin
            transform.position = respawn.transform.position;

            // Sets velocity to 0 so none is conserved after respawn
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

            // Remove 1 health
            currentPlayerLife -= 1;
        }

        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Force Pickup")
        {
            SFXManager.instance.PlaySFX(pickupSFX, transform, 1f);
            forcePickup = true;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Speed Pickup")
        {
            SFXManager.instance.PlaySFX(pickupSFX, transform, 1f);
            speedPickUpLength = Time.time + speedPickUpDuration;
            speedPickup = true;
            Destroy(other.gameObject);
        }
    }
}
