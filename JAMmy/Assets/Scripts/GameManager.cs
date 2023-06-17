using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> dim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
