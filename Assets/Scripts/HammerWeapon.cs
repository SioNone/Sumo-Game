using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerWeapon : MonoBehaviour
{
    private Rigidbody2D rgdBody;

    void Start()
    {
        rgdBody = GetComponent<Rigidbody2D>();
    }

    void OnColliderEnter2D(Collider2D other)
    {
        Debug.Log("Collision");
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("HIT !");
            Vector2 angularVelo = rgdBody.velocity;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(other.gameObject.transform.position * angularVelo);
        }
    }
}
