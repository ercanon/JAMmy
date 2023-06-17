using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    /* ----- VARIABLES ----- */
    private Rigidbody2D playerRB;
    private PlayerInput input;

    [SerializeField] private float movementSpeed;
    private Vector2 inputMovement;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        input = gameObject.GetComponent<PlayerInput>();
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
