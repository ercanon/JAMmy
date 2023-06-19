using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AuroraManager : MonoBehaviour
{
    /* ----- VARIABLES ----- */
    private Camera cam;

    [SerializeField] private int auroraQuantity;
    [SerializeField] private GameObject auroraPrefab;
    private Transform[] auroraList;
    [HideInInspector] public List<GameObject> orbTrans;

    void Start()
    {
        cam = GetComponent<Camera>();

        auroraList = new Transform[auroraQuantity];
        for (int i = 0; i < auroraQuantity; i++)
        {
            auroraList[i] = Instantiate(auroraPrefab, this.transform).transform;
            auroraList[i].gameObject.SetActive(false);
        }

        orbTrans = new List<GameObject>();
    }

    void Update()
    {
        if (orbTrans.Count > 0)
        {
            for (int list = 0; list < auroraQuantity; list++)
            {
                if (orbTrans[list] == null)
                {
                    auroraList[list].gameObject.SetActive(false);
                    return;
                }

                Vector3 targetPosition = cam.WorldToViewportPoint(orbTrans[list].transform.position);

                if (targetPosition.x >= 0f && targetPosition.x <= 1f && targetPosition.y >= 0f && targetPosition.y <= 1f)
                {
                    auroraList[list].gameObject.SetActive(false);
                    return;
                }

                float angle = Mathf.Atan2(targetPosition.y - 0.5f, targetPosition.x - 0.5f);
                Vector3 arrowPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 1f) * 0.5f;

                auroraList[list].transform.position = cam.ViewportToWorldPoint(arrowPosition + new Vector3(0.5f, 0.5f, 0f));
                auroraList[list].transform.rotation = Quaternion.Euler(0f, 0f, (angle + 90) * Mathf.Rad2Deg);

                auroraList[list].gameObject.SetActive(true);
            }
        }
    }

    public void ClearList()
    {
        foreach (GameObject orb in orbTrans)
            if (orb != null) Destroy(orb);
        orbTrans.Clear();
    }
}
