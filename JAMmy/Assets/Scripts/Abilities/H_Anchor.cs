using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Anchor : MonoBehaviour
{
    [SerializeField] private float duration;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ability" || collision.gameObject.tag == "AbilityEnemy")
        {
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Duration(collision.gameObject.GetComponent<PlayerMovement>()));
        }
    }

    private IEnumerator Duration(PlayerMovement player)
    {
        float prevSpeed = player.movementSpeed;

        player.movementSpeed *= 0;

        yield return new WaitForSeconds(duration);

        player.movementSpeed = prevSpeed;
    }
}
