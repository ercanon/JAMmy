using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Teleport : MonoBehaviour
{
    [SerializeField] private List<Vector2> corners;
    [SerializeField] private List<Transform> maps;
    private Vector2 centerMap;

    void ResetAction()
    {
        
    }

    public void StartAction()
    {
        centerMap = Vector2.zero;
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
            transform.position = (Vector3)(centerMap + corners[Random.Range(0, 3)]);
    }
}
