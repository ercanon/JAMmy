using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_Protect : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private float duration;
    [SerializeField] private float multiplier;
    [SerializeField] private float cooldown;
    private float countDown;
    private IEnumerator coroutine;

    void Start()
    {
        coroutine = Duration();
    }

    public void StartAction()
    {
        StartCoroutine(coroutine);
    }

    private IEnumerator Duration()
    {
        countDown = duration;

        while (true)
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0)
            {
                StopCoroutine(coroutine);
                coroutine = CoolDown();
                StartCoroutine(coroutine);
            }

            yield return null;
        }
    }

    private IEnumerator CoolDown()
    {
        countDown = cooldown;

        while (true)
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0)
            {
                StopCoroutine(coroutine);
                coroutine = Duration();
            }

            yield return null;
        }
    }
}
