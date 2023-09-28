using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor.Presets;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    private int numPlayers = PlayerManager.numPlayers;
    public GameObject player;
    public GameObject[] playerSpawns;
    public GameObject[] playerHealthbars;
    public PlayerController playerScript;

    void Start()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            playerHealthbars[i].SetActive(true);
            var newPlayer = PlayerInput.Instantiate(player, controlScheme: "Gamepad", pairWithDevices: new InputDevice[] { Gamepad.all[i] });
            newPlayer.transform.position = playerSpawns[i].transform.position;
            playerScript = newPlayer.GetComponent<PlayerController>();
            playerScript.healthBar = playerHealthbars[i].transform.GetChild(1).gameObject;
        }
    }
}
