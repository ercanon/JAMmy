using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    /* ----- VARIABLES ----- */
    private Rigidbody2D playerRB;
    private PlayerInput input;
    private Animator anim;

    [SerializeField] private float movementSpeed;
    private Vector2 inputMovement;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        
    }

    void FixedUpdate()
    {
        playerRB.position += inputMovement * Time.deltaTime * movementSpeed;
    }

    /* ----- GAME CONTROLLER ----- */
    public void OnMovement(InputAction.CallbackContext value)
    {
        inputMovement = value.ReadValue<Vector2>();
    }

    public void OnHability1(InputAction.CallbackContext value)
    {
        if (value.started)
        {

        }
    }


    /* ----- MENU CONTROLLER ----- */
    public void OnEnter(InputAction.CallbackContext value)
    {
        if (value.started)
        {

        }
    }

    public void OnMenuMove(InputAction.CallbackContext value)
    {
        if (value.started)
        {

        }
    }

    public void OnCancel(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }



    /* ----- DEVICE MANAGER ----- */
    public void OnControlsChanged()
    {

    }

    public void OnDeviceLost()
    {

    }

    public void OnDeviceRegained()
    {
        StartCoroutine(WaitForDeviceToBeRegained());
    }

    IEnumerator WaitForDeviceToBeRegained()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
