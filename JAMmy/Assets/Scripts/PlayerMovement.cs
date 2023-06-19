using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    enum CharacterState { Pause, Idle, Walking };
    enum CharacterDirection { Right, Left };

    /* ----- VARIABLES ----- */
    private Rigidbody2D playerRB;
    private PlayerInput input;
    private Animator anim;

    [SerializeField] private float movementSpeed;
    private Vector2 inputMovement;
    private CharacterState cState;
    private CharacterDirection cDir;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        anim = transform.GetChild(0).GetComponent<Animator>();

        cState = CharacterState.Idle;
    }

    private void OnEnable()
    {
        
    }

    void FixedUpdate()
    {
        playerRB.position += inputMovement * Time.deltaTime * movementSpeed;

        if (cState == CharacterState.Walking && inputMovement == new Vector2(0, 0))
        {
            cState = CharacterState.Idle;
            anim.SetBool("Walking", false);
        }
    }

    public void SetCharacterState(int state)
    {
        cState = (CharacterState)state;
    }

    /* ----- GAME CONTROLLER ----- */
    public void OnMovement(InputAction.CallbackContext value)
    {
        if (cState != CharacterState.Pause)
        {
            cState = CharacterState.Walking;
            anim.SetBool("Walking", true);

            inputMovement = value.ReadValue<Vector2>();

            if (inputMovement.x < 0 && cDir != CharacterDirection.Left)
            {
                cDir = CharacterDirection.Left;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (inputMovement.x > 0 && cDir != CharacterDirection.Right)
            {
                cDir = CharacterDirection.Right;
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
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
