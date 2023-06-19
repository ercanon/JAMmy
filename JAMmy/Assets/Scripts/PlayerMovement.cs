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
    private Animator anim;
    private GameManager gameMan;

    [SerializeField] private float movementSpeed;
    private Vector2 inputMovement;
    private CharacterState cState;
    private CharacterDirection cDir;
    private int charID;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();

        cState = CharacterState.Pause;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
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

    public void InitialSet(GameManager gm, int id)
    {
        gameMan = gm;
        charID = id;
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
    public void OnSubmit(InputAction.CallbackContext value)
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
            gameMan.onLeftPlayer(charID);
        }
    }

    public void OnJoin(InputAction.CallbackContext value)
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
