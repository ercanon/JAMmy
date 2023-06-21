using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_KnockBack : MonoBehaviour
{
    [SerializeField] private float multiplier;
    [SerializeField] private float durationOn;
    [SerializeField] private float durationOff;
    private bool active;

    private void Start()
    {
        active = true;
        StartCoroutine(Duration(durationOn));
    }

    private IEnumerator Duration(float dur)
    {
        yield return new WaitForSeconds(dur);

        active = !active;
        if (!active)
            StartCoroutine(Duration(durationOff));
        else
            StartCoroutine(Duration(durationOn));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ability" || collision.gameObject.tag == "AbilityEnemy")
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && active)
        {
            Vector3 repelDirection = collision.transform.position - transform.position;
            repelDirection.Normalize();

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(repelDirection * multiplier, ForceMode2D.Force);
        }
    }
}