using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;



public class GameManager : MonoBehaviour
{
    enum GameState { MenuScreen, GamePlayScreen, EndScreen, Transition };

    /* ----- VARIABLES ----- */
    [SerializeField] private TextMeshProUGUI render;
    [SerializeField] private MultiplayerEventSystem events;
    [SerializeField] private List<GameObject> characters;
    private bool[] charAvailable;
    private List<Vector2> initPos;

    private GameState gState;
    [SerializeField] private float timer;
    private IEnumerator timerCoroutine;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gState = GameState.MenuScreen;

        charAvailable = new bool[]{ true, true, true, true};

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
    public void onNewPlayer(PlayerInput input)
    {
        try { Destroy(GameObject.FindWithTag("ClonetoDelete")); }
        catch { }

        if (gState == GameState.MenuScreen)
        {
            for (int pos = 0; pos < characters.Count; pos++)
            {
                if (charAvailable[pos])
                {
                    if (!characters[pos].activeInHierarchy)
                    {
                        characters[pos].SetActive(true);
                        characters[pos].GetComponentInParent<PlayerMovement>().InitialSet(this, pos);
                    }

                    if (input.actions != null)
                        charAvailable[pos] = false;

                    break;
                }
            }
        }
    }

    public void onLeftPlayer(int charPos)
    {
        if (gState == GameState.MenuScreen)
        {
            characters[charPos].SetActive(false);
            charAvailable[charPos] = true;
        }
    }



    /* ----- MENU BUTTONS ----- */
    public void onPlay()
    {
        gState = GameState.GamePlayScreen;

        foreach (GameObject player in characters)
            if (player.activeInHierarchy)
                player.GetComponentInParent<PlayerMovement>().SetCharacterState(1);
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
