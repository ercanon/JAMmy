using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_OrbSpeed : MonoBehaviour
{
    [SerializeField] private float multiplier;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().orbPickUp *= multiplier;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().orbPickUp /= multiplier;
        }
    }
}