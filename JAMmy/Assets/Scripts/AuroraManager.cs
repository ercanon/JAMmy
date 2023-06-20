using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AuroraManager : MonoBehaviour
{
    /* ----- VARIABLES ----- */
    private Camera cam;
    [SerializeField] private Animator anim;
    [HideInInspector] public Vector2 guideCamera;
    private float timer;

    [SerializeField] private GameObject auroraPrefab;
    private Dictionary<GameObject, GameObject> orbAurora;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        cam = GetComponent<Camera>();

        orbAurora = new Dictionary<GameObject, GameObject>();
    }

    void Update()
    {
        foreach (var OA in orbAurora)
        {
            if (OA.Key == null)
            {
                Destroy(OA.Value);
                orbAurora.Remove(OA.Key);
                continue;
            }

            Vector3 targetPosition = cam.WorldToViewportPoint(OA.Key.transform.position);
            if (OA.Value.activeInHierarchy && targetPosition.x >= 0f && targetPosition.x <= 1f && targetPosition.y >= 0f && targetPosition.y <= 1f)
            {
                OA.Value.SetActive(false);
                continue;
            }
            else if (!OA.Value.activeInHierarchy)
                OA.Value.SetActive(true);

            float angle = Mathf.Atan2(targetPosition.y - 0.5f, targetPosition.x - 0.5f);
            Vector3 arrowPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 1f) * 0.5f;

            OA.Value.transform.position = cam.ViewportToWorldPoint(arrowPosition + new Vector3(0.5f, 0.5f, 0f));
            OA.Value.transform.rotation = Quaternion.Euler(0f, 0f, (angle + 90) * Mathf.Rad2Deg);
        }
    }

    public void SetList(GameObject orb)
    {
        orbAurora.Add(orb, Instantiate(auroraPrefab, this.transform));
    }

    public void ClearList()
    {
        foreach (var orb in orbAurora)
        {
            if (orb.Key != null)
            {
                Destroy(orb.Key);
                Destroy(orb.Value);
            }
        }
        orbAurora.Clear();
    }

    public void RotateScreens(int state)
    {
        anim.SetInteger("GameState", state);
        StartCoroutine("UpdateCamera");
    }

    private IEnumerator UpdateCamera()
    {
        timer = 1.25f;

        while (true)
        {
            cam.rect = new Rect(guideCamera, new Vector2(0.5f, 0.5f));

            timer -= Time.deltaTime;
            if (timer <= 0)
                StopCoroutine("UpdateCamera");

            yield return null;
        }
    }
}
