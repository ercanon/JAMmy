using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;



public class GameManager : MonoBehaviour
{
    enum GameState { MenuScreen, Round1, Round2, Round3, Round4, EndScreen };

    /* ----- VARIABLES ----- */
    [Header("General")]
    [SerializeField] private TextMeshProUGUI render;
    [SerializeField] private List<GameObject> characters;
    [SerializeField] private List<Transform> maps;
    private bool[] charAvailable;
    private List<Vector2> initPos = new List<Vector2>();
    private GameState gState;

    [Space]
    [Header("Menus")]
    [SerializeField] private Button startCond;

    [Space]
    [Header("PlayState")]
    [SerializeField] private float timer;
    private float countDown;
    private IEnumerator timerCoroutine;
    [SerializeField] private GameObject orb;
    [SerializeField] private List<Vector2> OrbSpawn;
    [SerializeField] private int quantityOrbs;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gState = GameState.MenuScreen;

        charAvailable = new bool[]{ true, true, true, true};

        foreach (GameObject player in characters)
            initPos.Add(player.transform.position);

        startCond.interactable = false;

        countDown = timer * 60;
    }

    void Update()
    {

    }

    private IEnumerator StartTimer()
    {
        while (true)
        {
            countDown -= Time.deltaTime;
            int min = (int)(countDown / 60f);
            int sec = (int)(countDown - min * 60);

            render.text = string.Format("{0:0}:{1:00}", min, sec);

            if (countDown <= 0)
            {
                StopCoroutine(timerCoroutine);
                WinCond();
            }

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

            //foreach (bool check in charAvailable)
            //    if (check == true)
            //        return;
            startCond.interactable = true; 
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

    public void WinCond()
    {
        StopCoroutine(timerCoroutine);
        countDown = timer * 60;

        foreach (GameObject player in characters)
        {
            if (player.activeInHierarchy)
                player.GetComponentInParent<PlayerMovement>().SetCharacter(1);

            AuroraManager aurora = player.transform.parent.GetComponentInChildren<AuroraManager>();
            aurora.ClearList();
            aurora.RotateScreens((int)gState);
        }

        Vector2 auxVec = initPos[0];
        int listsSize = characters.Count;
        for (int posLists = 0; posLists < listsSize; posLists++)
        {
            if (posLists + 1 == listsSize)
                initPos[posLists] = auxVec;
            else
                initPos[posLists] = initPos[posLists + 1];

            characters[posLists].transform.parent.position = initPos[posLists];
        }

        onPlay();
    }



    /* ----- MENU BUTTONS ----- */
    public void onPlay()
    {
        if (gState++ > GameState.EndScreen) 
            ;

        timerCoroutine = StartTimer();
        StartCoroutine(timerCoroutine);

        for (int list = 0; list < characters.Count; list++)
        {
            if (characters[list].activeInHierarchy)
            {
                int[] genNum = new int[quantityOrbs];
                AuroraManager aurora = characters[list].transform.parent.GetComponentInChildren<AuroraManager>();
                for (int index = 0; index < quantityOrbs; index++)
                {
                    genNum[index] = Random.Range(0, 14);
                    while (index != 0 && genNum[index - 1] == genNum[index])
                        genNum[index] = Random.Range(0, 14);

                    Vector3 spawn = maps[index].position + (Vector3)OrbSpawn[genNum[index]];

                    aurora.SetList(Instantiate(orb, spawn, Quaternion.identity));
                }

                characters[list].GetComponentInParent<PlayerMovement>().SetCharacter(1);
            }
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
