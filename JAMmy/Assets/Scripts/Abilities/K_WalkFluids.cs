using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_WalkFluids : MonoBehaviour
{
    [SerializeField] private float duration;

    public void ResetAction()
    {
        StopAllCoroutines();
    }

    public void StartAction()
    {
        StartCoroutine("Duration");
    }

    private IEnumerator Duration()
    {
        gameObject.layer = LayerMask.NameToLayer("Water");

        yield return new WaitForSeconds(duration);

        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
