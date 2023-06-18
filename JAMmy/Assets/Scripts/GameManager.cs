using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;



public class GameManager : MonoBehaviour
{
    enum GameState { MenuScreen, GamePlayScreen, EndScreen, Transition };

    /* ----- VARIABLES ----- */
    [SerializeField] private TMPro.TextMeshProUGUI render;
    [SerializeField] private MultiplayerEventSystem events;
    [SerializeField] private List<GameObject> characters;
    private List<Vector2> initPos;

    private GameState gState;
    [SerializeField] private float timer;
    private IEnumerator timerCoroutine;

    private int playerCount;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gState = GameState.MenuScreen;

        playerCount = 0;

        initPos = new List<Vector2>();
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
        if (gState == GameState.MenuScreen)
        {
            characters[playerCount].SetActive(true);
            playerCount++;
        }
    }

    public void onLeftPlayer()
    {
        if (gState == GameState.MenuScreen)
        {
            playerCount--;
            characters[playerCount].SetActive(false);
        }
    }

    public void onPlay()
    {
        gState = GameState.GamePlayScreen;

        foreach (GameObject clone in GameObject.FindGameObjectsWithTag("ClonetoDelete"))
            Destroy(clone);

        foreach (GameObject player in characters)
            if (player.activeInHierarchy)
            {
                player.GetComponent<PlayerInput>().enabled = true;
                player.transform.parent.GetComponent<PlayerMovement>().SetCharacterState(1);
            }
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
