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
    private List<Vector2> initPos;
    private GameState gState;
    private int[] teamCount;

    [Space]
    [Header("Menus")]
    [SerializeField] private Button startCond;

    [Space]
    [Header("PlayState")]
    [SerializeField] private float timer;
    private float countDown;
    private IEnumerator timerCoroutine;
    [SerializeField] private int quantityOrbs;
    [SerializeField] private GameObject orb;
    [SerializeField] private List<Vector2> OrbSpawn;



    /* ----- GAME FRAMING ----- */
    public void RestartGame()
    {
        foreach (GameObject pending in GameObject.FindGameObjectsWithTag("Ability"))
            Destroy(pending);

        foreach (GameObject pending in GameObject.FindGameObjectsWithTag("AbilityEnemy"))
            Destroy(pending);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gState = GameState.MenuScreen;

        charAvailable = new bool[]{ true, true, true, true};

        initPos = new List<Vector2>();
        foreach (GameObject player in characters)
            initPos.Add(player.transform.position);

        teamCount = new int[] { 0, 0};

        startCond.interactable = false;

        countDown = timer * 60;
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
                WinCond(0);
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

    public void WinCond(int posWinner)
    {
        if (posWinner != 0)
            teamCount[posWinner <= -1 ? 0 : 1]++;

        StopAllCoroutines();
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
        Transform auxTrans = maps[0];
        int listsSize = characters.Count;
        for (int posLists = 0; posLists < listsSize; posLists++)
        {
            if (posLists + 1 == listsSize)
            {
                initPos[posLists] = auxVec;
                maps[posLists] = auxTrans;
            }
            else
            {
                initPos[posLists] = initPos[posLists + 1];
                maps[posLists] = maps[posLists + 1];
            }

            characters[posLists].transform.parent.position = initPos[posLists];
        }

        if (gState++ > GameState.EndScreen)
        {
            transform.GetChild(1).gameObject.SetActive(false);
            Transform aux = transform.GetChild(2);
            aux.gameObject.SetActive(true);
            if (teamCount[0] == teamCount[1])     aux.GetChild(0).gameObject.SetActive(true);
            else if (teamCount[0] > teamCount[1]) aux.GetChild(1).gameObject.SetActive(true);
            else if (teamCount[0] < teamCount[1]) aux.GetChild(2).gameObject.SetActive(true);

            return;
        }

        onPlay();
    }



    /* ----- MENU BUTTONS ----- */
    public void onPlay()
    {
        timerCoroutine = StartTimer();
        StartCoroutine(timerCoroutine);

        for (int list = 0; list < characters.Count; list++)
        {
            if (characters[list].activeInHierarchy)
            {
                List<int> genNum = new List<int>();
                AuroraManager aurora = characters[list].transform.parent.GetComponentInChildren<AuroraManager>();
                for (int i = 0; i < quantityOrbs; i++)
                {
                    int orbSelected = Random.Range(0, OrbSpawn.Count - 1);
                    while (genNum.Contains(orbSelected))
                        orbSelected = Random.Range(0, OrbSpawn.Count - 1);

                    genNum.Add(orbSelected);

                    Vector3 spawn = maps[list].position + (maps[list].localScale.x * (Vector3)OrbSpawn[orbSelected]);

                    aurora.SetList(Instantiate(orb, spawn, Quaternion.identity));
                }

                characters[list].GetComponentInParent<PlayerMovement>().SetCharacter(1);
            }
        }
    }

    public void onQuit()
    {
        Application.Quit();
    }
}
