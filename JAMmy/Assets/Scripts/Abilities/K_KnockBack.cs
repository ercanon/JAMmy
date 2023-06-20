using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_KnockBack : MonoBehaviour
{
    [SerializeField] private float multiplier = 10f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 repelDirection = collision.transform.position - transform.position;
            repelDirection.Normalize();

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(repelDirection * multiplier, ForceMode2D.Force);
        }
    }
}