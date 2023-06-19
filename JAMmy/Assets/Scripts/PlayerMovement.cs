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
    private PlayerInput inputs;
    private SpriteRenderer sprite;
    private GameManager gameMan;

    [SerializeField] private float movementSpeed;
    private Transform canvaOrb;
    private int orbCount;
    private Vector2 inputMovement;
    private CharacterState cState;
    private CharacterDirection cDir;
    private int charID;



    /* ----- GAME FRAMING ----- */
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        GameObject child = transform.GetChild(0).gameObject;
        anim = child.GetComponent<Animator>();
        inputs = child.GetComponent<PlayerInput>();
        sprite = child.GetComponent<SpriteRenderer>();

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

    public void SetCharacter(int state)
    {
        cState = (CharacterState)state;
        inputs.SwitchCurrentActionMap("Player Controller");
        orbCount = 0;
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
                sprite.flipX = true;
            }
            else if (inputMovement.x > 0 && cDir != CharacterDirection.Right)
            {
                cDir = CharacterDirection.Right;
                sprite.flipX = false;
            }
        }
    }

    public void OnHability1(InputAction.CallbackContext value)
    {
        if (value.started)
        {

        }
    }

    public void OnHability2(InputAction.CallbackContext value)
    {
        if (value.started)
        {

        }
    }

    public void OnHability3(InputAction.CallbackContext value)
    {
        if (value.started)
        {

        }
    }

    public void OnCancelHability(InputAction.CallbackContext value)
    {
        if (value.started)
        {

        }
    }

    public void OnEnemy1(InputAction.CallbackContext value)
    {
        if (value.started)
        {

        }
    }

    public void OnEnemy2(InputAction.CallbackContext value)
    {
        if (value.started)
        {

        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Orb":
                if (canvaOrb == null)
                {
                    canvaOrb = collision.transform;

                    collision.collider.isTrigger = true;
                    canvaOrb.SetParent(transform);
                    canvaOrb.localScale *= 0.6f;
                    canvaOrb.localPosition = new Vector3(0, 0.7f, 0);
                }
                break;

            case "Beacon":
                if (canvaOrb != null)
                { 
                    orbCount++;
                    Destroy(canvaOrb.gameObject);
                    if (orbCount >= 3)
                        gameMan.WinCond();
                }
                break;
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
