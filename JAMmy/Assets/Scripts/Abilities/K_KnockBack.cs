using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_KnockBack : MonoBehaviour
{
    [SerializeField] private float multiplier = 10f;

    private void OnTriggerStay(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector3 repelDirection = collision.transform.position - transform.position;
            repelDirection.Normalize();

            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            playerRigidbody.AddForce(repelDirection * multiplier, ForceMode.Force);
        }
    }
}