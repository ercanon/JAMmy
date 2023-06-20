using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Protect : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ability")
        {
            Destroy(collision.gameObject);
        }
    }
}
