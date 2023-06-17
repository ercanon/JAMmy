using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    /* ----- VARIABLES ----- */
    [SerializeField] private List<GameObject> dim;

    [SerializeField] private TMPro.TextMeshProUGUI render;
    [SerializeField] private float timer;

    private int playerCount;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        playerCount = 0;

        timer *= 60;
        StartCoroutine(StartTimer());
    }

    void Update()
    {
        
    }

    private IEnumerator StartTimer()
    {
        while (true)
        {
            timer -= Time.deltaTime;
            int min = (int)(timer / 60f);
            int sec = (int)(timer - min * 60);

            render.text = string.Format("{0:0}:{1:00}", min, sec);

            if (timer <= 0)
                StopCoroutine(StartTimer());

            yield return null;
        }
    }


    /* ----- WORLD BUIDING ----- */
    public void onNewPlayer()
    {
        //GameObject.FindGameObjectsWithTag("ClonetoDelete");

        if (!dim[playerCount].activeInHierarchy)
        {
            dim[playerCount].SetActive(true);
            playerCount++;
        }
    }

    public void onLeftPlayer()
    {
        if (dim[playerCount].activeInHierarchy)
        {
            playerCount--;
            dim[playerCount].SetActive(true);
        }
    }
}
