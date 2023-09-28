using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEditor.Presets;
using UnityEngine;

public class PlayerLoader : MonoBehaviour
{
    private int numPlayers = PlayerManager.numPlayers;
    public PlayerInput playerInput;
    public GameObject player;
    public GameObject[] playerSpawns;
    public Preset playerInputPreset;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        for (int i = 0; i < numPlayers; i++)
        {
            var newPlayer = PlayerInput.Instantiate(player, controlScheme: "Gamepad", pairWithDevices: new InputDevice[] { Gamepad.all[i] });
            newPlayer.transform.position = playerSpawns[i].transform.position;
        }
    }
}
