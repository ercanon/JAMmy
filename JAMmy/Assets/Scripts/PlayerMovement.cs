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

    public float movementSpeed;
    [SerializeField] private float orbPickUp;
    private Transform canvaOrb;
    private Vector2 inputMovement;
    private CharacterState cState;
    private CharacterDirection cDir;
    private IEnumerator orbDeposit;
    private int charID;

    [Header("Ability1")]
    [SerializeField] private Component Ability1;
    [SerializeField] private float CoolDown1;
    private IEnumerator Ability1CoolDown;
    private bool ability1Check;

    [Header("Ability2")]
    [SerializeField] private GameObject Ability2;
    [SerializeField] private Transform Partner;
    [SerializeField] private float CoolDown2;
    private IEnumerator Ability2CoolDown;
    private bool ability2Check;

    [Header("Ability3")]
    [SerializeField] private GameObject Ability3;
    [SerializeField] private Transform Enemy1;
    [SerializeField] private Transform Enemy2;
    [SerializeField] private float CoolDown3;
    private IEnumerator Ability3CoolDown;
    private bool abilitySelected;
    private bool ability3Check;



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
        StopAllCoroutines();
        abilitySelected = false;
        ability1Check = true;
        ability2Check = true;
        ability3Check = true;
        Ability1CoolDown = CoolDown(CoolDown1, 1);
        Ability2CoolDown = CoolDown(CoolDown2, 2);
        Ability3CoolDown = CoolDown(CoolDown3, 3);
        orbDeposit = CoolDown(orbPickUp, 0);
        Ability1.SendMessage("ResetAction");

        cState = (CharacterState)state;
        inputs.SwitchCurrentActionMap("Player Controller");
    }

    public void SetOrbDeposit(float multiplier, int sign)
    {
        switch (sign)
        {
            case 0:
                orbDeposit = CoolDown(orbPickUp *= multiplier, 0);
                break;
            case 1:
                orbDeposit = CoolDown(orbPickUp /= multiplier, 0);
                break;
        }
    }

    private IEnumerator CoolDown(float duration, int type)
    {
        yield return new WaitForSeconds(duration);

        switch (type)
        {
            case 0:
                Destroy(canvaOrb.gameObject);
                orbDeposit = CoolDown(orbPickUp, 0);

                yield return null;

                if (GetComponentInChildren<AuroraManager>().orbAurora.Count <= 0 &&
                    Partner.GetComponentInChildren<AuroraManager>().orbAurora.Count <= 0)
                    gameMan.WinCond((int)Mathf.Sign(transform.position.x));
                break;
            case 1:
                ability1Check = true;
                Ability1CoolDown = CoolDown(CoolDown1, 1);
                break;
            case 2:
                ability2Check = true;
                Ability2CoolDown = CoolDown(CoolDown2, 2);
                break;
            case 3:
                ability3Check = true;
                Ability3CoolDown = CoolDown(CoolDown3, 3);
                break;
        }
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

    public void OnAbility1(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (ability1Check && !abilitySelected)
            {
                ability1Check = false;
                Ability1.SendMessage("StartAction");
                StartCoroutine(Ability1CoolDown);
                Debug.Log("Ability1");
            }
        }
    }

    public void OnAbility2(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (ability2Check && !abilitySelected)
            {
                ability2Check = false;
                Instantiate(Ability2, Partner.position, Quaternion.identity);
                StartCoroutine(Ability2CoolDown);
                Debug.Log("Ability2");
            }
        }
    }

    public void OnAbility3(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (ability3Check)
                abilitySelected = true;
            Debug.Log("Ability3");
        }
    }

    public void OnEnemy1(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (abilitySelected && ability3Check)
            {
                ability3Check = false;
                Instantiate(Ability3, Enemy1.position, Quaternion.identity);
                StartCoroutine(Ability3CoolDown);
                abilitySelected = false;
                Debug.Log("Enemy1");
            }
        }
    }

    public void OnEnemy2(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (abilitySelected && ability3Check)
            {
                ability3Check = false;
                Instantiate(Ability3, Enemy2.position, Quaternion.identity);
                StartCoroutine(Ability3CoolDown);
                abilitySelected = false;
                Debug.Log("Enemy2");
            }
        }
    }

    public void OnCancelHability(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            if (abilitySelected)
                abilitySelected = false;
            Debug.Log("CancelAbility");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Orb":
                if (canvaOrb == null)
                {
                    canvaOrb = collision.transform;

                    canvaOrb.SetParent(transform);
                    canvaOrb.localScale *= 0.6f;
                    canvaOrb.localPosition = new Vector3(0, 0.7f, 0);
                }
                break;

            case "Beacon":
                if (canvaOrb != null)
                    StartCoroutine(orbDeposit);
                break;

            case "AbilityEnemy":
                {
                    if (canvaOrb != null)
                    {
                        canvaOrb = collision.transform;

                        canvaOrb.SetParent(null);
                        canvaOrb.localScale /= 0.6f;
                        canvaOrb.localPosition = Vector3.zero;
                    }
                    else
                    {
                        StopCoroutine(orbDeposit);
                        orbDeposit = CoolDown(orbPickUp, 0);
                    }
                }
                break;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Beacon")
        {
            StopCoroutine(orbDeposit);
            orbDeposit = CoolDown(orbPickUp, 0);
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
