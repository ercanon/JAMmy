using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_WalkFluids : MonoBehaviour
{
    public void ResetAction()
    {

    }
    
    public void StartAction()
    {
        gameObject.layer = LayerMask.NameToLayer("Water");
    }
}
