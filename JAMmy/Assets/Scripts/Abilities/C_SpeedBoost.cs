using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_SpeedBoost : MonoBehaviour
{
    [SerializeField] private float multiplier;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ability" || collision.gameObject.tag == "AbilityEnemy")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().movementSpeed *= multiplier;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().movementSpeed /= multiplier;
        }
    }
}
