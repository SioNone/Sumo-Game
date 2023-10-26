using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerWeapon : MonoBehaviour
{
    private Rigidbody2D rgdBody;

    // Start is called before the first frame update
    void Start()
    {
        rgdBody = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            float angularVelo = rgdBody.angularVelocity;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(other.gameObject.transform.position * angularVelo);
        }
    }
}
