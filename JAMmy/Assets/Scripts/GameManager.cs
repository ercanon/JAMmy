using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    /* ----- VARIABLES ----- */
    public List<GameObject> dim;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        
    }

    void Update()
    {
        
    }



    /* ----- WORLD BUIDING ----- */
    public void SetNumPlayers(int size)
    {
        switch (size)
        {
            case 3:
                dim[3].SetActive(true);
                goto case 2;
            case 2:
                dim[2].SetActive(true);
                goto case 1;
            case 1:
                dim[1].SetActive(true);
                goto case 0;
            case 0:
                dim[0].SetActive(true);
                break;
        }
    }

    public void onNewPlayer()
    {
        
    }

    public void onLeftPlayer()
    {

    }
}
