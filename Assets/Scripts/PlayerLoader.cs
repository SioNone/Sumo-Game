using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor.Presets;
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

    // How many players remain
    public static int playersRemain;

    // Player Controller script
    public PlayerController playerScript;

    // Game Win Screen
    public GameObject gameWinScreen;

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
            playerScript = newPlayer.GetComponent<PlayerController>();

            // Assign the healthbar to the player
            playerScript.healthBar = playerHealthbars[i].transform.GetChild(1).gameObject;
        }
    }

    void Update()
    {
        // If one or somehow less players remain set the game win screen to true
        if (playersRemain <= 1)
        {
            gameWinScreen.SetActive(true);
        }
    }
}
