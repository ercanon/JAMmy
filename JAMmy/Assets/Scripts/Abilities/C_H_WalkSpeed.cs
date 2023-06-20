using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_H_WalkSpeed : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private float duration;
    [SerializeField] private float multiplier;

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
        player.movementSpeed *= multiplier;

        yield return new WaitForSeconds(duration);

        player.movementSpeed /= multiplier;
    }
}
