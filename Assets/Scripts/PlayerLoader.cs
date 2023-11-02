using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
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

    // List of Player Sprites
    public List<Sprite> playerSprites;

    public Leaderboard leaderboardScript;

    [SerializeField]
    private GameObject leaderboard;

    [SerializeField]
    private GameObject playerEntry;

    private bool hasDisplayed = false;

    // Pickup Cooldown
    // public float pickupCooldown, nextPickup;
    // public GameObject[] pickupList;

    void Start()
    {
        playersRemain = numPlayers;

        leaderboardScript = GameObject.FindWithTag("Leaderboard").GetComponent<Leaderboard>();

        // Adds new players based on how many are selected at start screen and assigns relevant gameobjects to them
        for (int i = 0; i < numPlayers; i++)
        {
            // Activates the player's
            playerHealthbars[i].SetActive(true);

            // Instantiates the prefab with Unity's input system
            var newPlayer = PlayerInput.Instantiate(player, controlScheme: "Gamepad", pairWithDevices: new InputDevice[] { Gamepad.all[i] });

            // Moves the player to their spawn point
            newPlayer.transform.position = playerSpawns[i].transform.position;

            // Assigns the player the correct sprite
            newPlayer.GetComponent<SpriteRenderer>().sprite = playerSprites[i];

            // Accesses the player script
            playerScript = newPlayer.GetComponent<PhysicsPlayerController>();

            // Assigns number to player
            playerScript.playerNumber = i + 1;

            playerScript.playerAnimator = "player" + (i + 1).ToString();

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
            if (!hasDisplayed)
            {
                PhysicsPlayerController playerScript = GameObject.FindWithTag("Player").GetComponent<PhysicsPlayerController>();
                List<string> playerList = leaderboardScript.GameComplete();
                playerList.Add(playerScript.playerNumber.ToString());
                int j = 1;
                for (int i = playerList.Count - 1; i > -1; i--)
                {
                    GameObject entry = Instantiate(playerEntry);
                    entry.transform.SetParent(leaderboard.gameObject.transform, false);
                    TMP_Text entryText = entry.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                    entryText.text = j + ") " + "Player " + playerList[i];
                    j++;
                }
                hasDisplayed = true;
            }
        }
    }
}
