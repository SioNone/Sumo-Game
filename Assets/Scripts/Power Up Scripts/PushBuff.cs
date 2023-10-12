using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/Push Buff")]

public class PushBuff : PowerUpSystem
{
   
        [SerializeField] public float amount;

        public override void Apply(GameObject target)
        {

            target.GetComponent<PhysicsPlayerController>().pushForce += amount;
            target.GetComponent<SpriteRenderer>().color = Color.blue;
        }

    }



