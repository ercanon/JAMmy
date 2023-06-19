using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;



public class GameManager : MonoBehaviour
{
    enum GameState { MenuScreen, GamePlayScreen, EndScreen, Transition };

    /* ----- VARIABLES ----- */
    [Header("General")]
    [SerializeField] private TextMeshProUGUI render;
    [SerializeField] private List<GameObject> characters;
    [SerializeField] private List<Camera> cameraList;
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
    [SerializeField] private GameObject Orb;
    [SerializeField] private List<Vector2> OrbSpawn;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gState = GameState.MenuScreen;

        charAvailable = new bool[]{ true, true, true, true};

        initPos.Add(characters[0].transform.position);
        initPos.Add(characters[2].transform.position);
        initPos.Add(characters[3].transform.position);
        initPos.Add(characters[1].transform.position);

        startCond.interactable = false;

        countDown = timer * 60;
        timerCoroutine = StartTimer();
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
        foreach (Camera cam in cameraList)
            cam.GetComponent<AuroraManager>().ClearList();

        StopCoroutine(timerCoroutine);
        countDown = timer * 60;

        foreach (GameObject player in characters)
            if (player.activeInHierarchy)
                player.GetComponentInParent<PlayerMovement>().SetCharacter(1);

        Rect auxRect = cameraList[0].rect;
        Vector2 auxVec = initPos[0];
        int listsSize = cameraList.Count;
        for (int posLists = 0; posLists < listsSize; posLists++)
        {
            if (posLists + 1 == listsSize)
            {
                cameraList[posLists].rect = auxRect;
                initPos[posLists] = auxVec;
            }
            else
            {
                cameraList[posLists].rect = cameraList[posLists + 1].rect;
                initPos[posLists] = initPos[posLists + 1];

            }

            characters[posLists].transform.parent.position = initPos[posLists];
        }

        onPlay();
    }



    /* ----- MENU BUTTONS ----- */
    public void onPlay()
    {
        gState = GameState.GamePlayScreen;

        StartCoroutine(timerCoroutine);

        for (int list = 0; list < characters.Count; list++)
        {
            if (characters[list].activeInHierarchy)
            {
                AuroraManager orbList = cameraList[list].GetComponent<AuroraManager>();

                int[] genNum = { Random.Range(0, 14), Random.Range(0, 14), Random.Range(0, 14) };
                for (int index = 0; index < genNum.Length; index++)
                {
                    while (genNum[0] == genNum[index] && index != 0)
                        genNum[index] = Random.Range(0, 14);

                    Vector3 spawn = characters[list].transform.position;
                    spawn += (Vector3)OrbSpawn[genNum[index]];

                    orbList.orbTrans.Add(Instantiate(Orb, spawn, Quaternion.identity));
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
