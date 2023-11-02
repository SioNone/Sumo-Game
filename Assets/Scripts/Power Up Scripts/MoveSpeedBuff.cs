using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Speed Buff")]

public class MoveSpeedBuff : PowerUpSystem
{

   [SerializeField] public float amount;

    public override void Apply(GameObject target)
    {

        target.GetComponent<PhysicsPlayerController>().currentPlayerSpeed += amount;
        // target.GetComponent<SpriteRenderer>().color = Color.red;
    }

}

