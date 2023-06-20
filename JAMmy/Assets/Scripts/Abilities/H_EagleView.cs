using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_EagleView : MonoBehaviour
{
    [SerializeField] private float multiplier;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInChildren<Camera>().orthographicSize *= multiplier;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponentInChildren<Camera>().orthographicSize /= multiplier;
        }
    }
}
