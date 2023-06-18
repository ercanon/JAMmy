using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class GameManager : MonoBehaviour
{
    /* ----- VARIABLES ----- */
    [SerializeField] private TMPro.TextMeshProUGUI render;
    [SerializeField] private MultiplayerEventSystem events;
    [SerializeField] private List<GameObject> characters;
    private List<Vector2> initPos;

    [SerializeField] private float timer;
    private IEnumerator timerCoroutine;

    private int playerCount;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerCount = 0;

        foreach (GameObject go in characters)
            initPos.Add(go.transform.position);

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

    }

    public void onPlay()
    {
        foreach (GameObject clone in GameObject.FindGameObjectsWithTag("ClonetoDelete"))
            Destroy(clone);

        foreach (GameObject player in characters)
            if (player.activeInHierarchy) 
                player.GetComponent<PlayerInput>().enabled = true;
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
