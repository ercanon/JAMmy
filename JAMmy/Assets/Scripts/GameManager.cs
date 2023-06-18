using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    /* ----- VARIABLES ----- */
    [SerializeField] private List<GameObject> characters;

    [SerializeField] private TMPro.TextMeshProUGUI render;
    [SerializeField] private float timer;
    private IEnumerator timerCoroutine;

    private int playerCount;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCount = 0;

        timer *= 60;
        timerCoroutine = StartTimer();
        StartCoroutine(timerCoroutine);
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
                StopCoroutine(timerCoroutine);

            yield return null;
        }
    }


    /* ----- GAME MANAGEMENT ----- */
    public void onNewPlayer()
    {
        characters[playerCount].SetActive(true);
        playerCount++;
    }

    public void onLeftPlayer()
    {
        playerCount--;
        characters[playerCount].SetActive(false);
    }

    public void onPlay()
    {
        //foreach (GameObject clone in GameObject.FindGameObjectsWithTag("ClonetoDelete"))
        //    Destroy(clone);
    }

    public void onSettings()
    {

    }

    public void onCredits()
    {

    }

    public void onQuit()
    {
        Application.Quit();
    }
}
