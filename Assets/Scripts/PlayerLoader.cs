using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    // Number of players
    private int numPlayers = PlayerManager.numPlayers;

    // The player prefab
    public GameObject player;

    // List of player spawns
    public GameObject[] playerSpawns;

    // List of player healthbars
    public GameObject[] playerHealthbars;

    // List of player pickup Indicators
    public GameObject[] playerIndicators;

    // How many players remain
    public static int playersRemain;

    // Player Controller script
    public PhysicsPlayerController playerScript;

    // Game Win Screen
    public GameObject gameWinScreen;

    // Pickup Cooldown
    // public float pickupCooldown, nextPickup;
    // public GameObject[] pickupList;

    void Start()
    {
        playersRemain = numPlayers;

        // Adds new players based on how many are selected at start screen and assigns relevant gameobjects to them
        for (int i = 0; i < numPlayers; i++)
        {
            // Activates the player's
            playerHealthbars[i].SetActive(true);

            // Instantiates the prefab with Unity's input system
            var newPlayer = PlayerInput.Instantiate(player, controlScheme: "Gamepad", pairWithDevices: new InputDevice[] { Gamepad.all[i] });

            // Moves the player to their spawn point
            newPlayer.transform.position = playerSpawns[i].transform.position;

            // Accesses the player script
            playerScript = newPlayer.GetComponent<PhysicsPlayerController>();

            // Change colour of headband (Maybe a neater way of doing this)
            if (i == 0)
            {
                newPlayer.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f, 0f, 0f, 1f);
            } 
            else if (i == 1)
            {
                newPlayer.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().material.color = new Color(0f, 0f, 1f, 1f);
            }
            else if (i == 2)
            {
                newPlayer.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().material.color = new Color(0f, 1f, 0f, 1f);
            } 
            else if (i == 3)
            {
                newPlayer.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1f, 0f, 1f, 1f);
            }

            // Assign Pickup Indicator to player
            playerScript.pickupIndicator = playerIndicators[i];

            // Assign the healthbar to the player
            playerScript.healthBar = playerHealthbars[i].transform.GetChild(1).GetComponent<Slider>();
        }
    }

    void Update()
    {
        // Pickup Cooldown stuff
        //if (Time.time > nextPickup)
        //{
        //    nextPickup = Time.time + pickupCooldown;
        //    Instantiate(pickupList[Random.Range(0, pickupList.Length)], new Vector3(0, 0, 0), Quaternion.identity);
        //}

        // If one or somehow less players remain set the game win screen to true
        if (playersRemain <= 1)
        {
            gameWinScreen.SetActive(true);
        }
    }
}
