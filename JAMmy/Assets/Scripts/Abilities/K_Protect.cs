using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Protect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "AbilityEnemy")
        {
            Destroy(collision.gameObject);
        }
    }
}
