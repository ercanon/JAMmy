using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Anchor : MonoBehaviour
{
    [SerializeField] private float duration;

    private IEnumerator Duration(PlayerMovement player)
    {
        float prevSpeed = player.movementSpeed;

        player.movementSpeed *= 0;

        yield return new WaitForSeconds(duration);

        player.movementSpeed = prevSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ability")
        {
            StartCoroutine(Duration(collision.gameObject.GetComponent<PlayerMovement>()));
        }
    }
}
