using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Teleport : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private List<Vector2> corners;
    [SerializeField] private List<Transform> maps;
    private Vector2 centerMap;
    private float countDown;
    private IEnumerator coroutine;

    void Start()
    {
        centerMap = Vector2.zero;
        coroutine = CoolDown();
    }

    public void StartAction()
    {
        Vector2 playerPos = (Vector2) transform.position;
        foreach (Transform mapPos in maps)
        {
            if (playerPos.x >= mapPos.position.x + corners[0].x && playerPos.x <= mapPos.position.x + corners[3].x &&
                playerPos.y <= mapPos.position.y + corners[0].y && playerPos.y >= mapPos.position.y + corners[3].y)
            {
                centerMap = (Vector2)mapPos.position;
                break;
            }
        }

        if (centerMap != Vector2.zero)
            StartCoroutine(coroutine);
    }

    private IEnumerator CoolDown()
    {
        transform.position = (Vector3)(centerMap + corners[Random.Range(0, 3)]);
        countDown = cooldown;

        while (true)
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0)
            {
                StopCoroutine(coroutine);
                coroutine = CoolDown();
            }

            yield return null;
        }
    }
}
