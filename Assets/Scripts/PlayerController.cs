using System.Collections;
using System.Collections.Generic;
using Harmony;
using Light;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void PlayerDeathEventHandler();


public class PlayerController : MonoBehaviour, IPlayerData {

    private Dictionary<string, bool> buttonsPressed;
    private Dictionary<string, bool> buttonsReleased;
    private Dictionary<string, bool> buttonsHeld;

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int nbPlayerLives;
    
    public int NbPlayerLives => nbPlayerLives;
    public static PlayerController instance;
    public event PlayerDeathEventHandler OnPlayerDie;

    private WilliamController williamController;
    private ReaperController reaperController;
    public EntityControlableController CurrentController { get; private set; }
    private EntityControlableController currentController;

    private LightSensor lightSensor;
    public Rigidbody2D Rigidbody { get; private set; }

    private int nbPlayerLivesLeft;

    private int currentLevel;
    private int numbOfLocks = 0;
    private bool movementLock = false;

    public int NbPlayerLivesLeft
    {
        get { return nbPlayerLivesLeft; }
        set
        {
            nbPlayerLivesLeft = value;
            LifePointsUI.instance.UpdateLP();
            if (IsPlayerDead())
            {
                OnPlayerDie?.Invoke();
                SceneManager.LoadScene("Level"+currentLevel);
            }
        }
    }

    public bool IsOnGround { get; set; }
    public bool IsDashing { get; set; }
    public FacingSideUpDown DirectionFacingUpDown { get; set; }
    public FacingSideLeftRight DirectionFacingLeftRight { get; set; }

    private void Awake()
    {
        buttonsPressed = new Dictionary<string, bool>();
        buttonsHeld = new Dictionary<string, bool>();
        buttonsReleased = new Dictionary<string, bool>();

        currentLevel = 1;
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this.gameObject);
        }

        Rigidbody = GetComponent<Rigidbody2D>();

        nbPlayerLivesLeft = nbPlayerLives;
        williamController = GetComponentInChildren<WilliamController>();
        reaperController = GetComponentInChildren<ReaperController>();
        GetComponent<HitSensor>().OnHit += DamagePlayer;

        lightSensor = GetComponent<LightSensor>();
        lightSensor.OnLightExpositionChange += OnLightExpositionChanged;

        OnLightExpositionChanged(true);    
    }

    public void DamagePlayer()
    {
        NbPlayerLivesLeft -= 1;
    }

    private void Update()
    {
        GetInputs();

        if (buttonsHeld[InputsName.LEFT] && !buttonsHeld[InputsName.RIGHT] && !movementLock)
        {
            if (movementLock)
                Debug.Log("Lock");
            DirectionFacingLeftRight = FacingSideLeftRight.Left;
            CurrentController.animator.SetFloat("Speed", Mathf.Abs(-1 * speed * Time.deltaTime));
            CurrentController.sprite.flipX = true;
        }
        else if (buttonsHeld[InputsName.RIGHT] && !buttonsHeld[InputsName.LEFT] && !movementLock)
        {
            DirectionFacingLeftRight = FacingSideLeftRight.Right;
            CurrentController.animator.SetFloat("Speed", Mathf.Abs(1 * speed * Time.deltaTime));
            CurrentController.sprite.flipX = false;
        }
        else
        {
            DirectionFacingLeftRight = FacingSideLeftRight.None;
            CurrentController.animator.SetFloat("Speed", 0);
        }

        if (buttonsHeld[InputsName.UP] && !buttonsHeld[InputsName.DOWN])
        {
            DirectionFacingUpDown = FacingSideUpDown.Up;
            CurrentController.animator.SetInteger("OrientationY", 1);
        }
        else if(buttonsHeld[InputsName.DOWN] && !buttonsHeld[InputsName.UP])
        {
            DirectionFacingUpDown = FacingSideUpDown.Down;
            CurrentController.animator.SetInteger("OrientationY", -1);
        }
        else
        {
            DirectionFacingUpDown = FacingSideUpDown.None;
            CurrentController.animator.SetInteger("OrientationY", 0);
        }

        CurrentController.animator.SetBool("IsJumping", Rigidbody.velocity.y > 0);
        CurrentController.animator.SetBool("IsFalling", !IsOnGround && !IsDashing);
        CurrentController.animator.SetBool("IsDashing", IsDashing);

        if(!CurrentController.Attacking)
        {
            CurrentController.animator.SetBool("IsAttacking", false);
        }
    }

    private void FixedUpdate()
    {
        if(CurrentController.CanUseBasicAttack(this) && buttonsHeld[InputsName.ATTACK])
        {
            CurrentController.UseBasicAttack(this);
            CurrentController.animator.SetBool("IsAttacking", true);
        }

        if(Rigidbody.velocity.y == 0 && !IsDashing)
        {
            IsOnGround = true;
            if (buttonsPressed[InputsName.JUMP])
            {
                Rigidbody.velocity = new Vector2(0, 0);
                Rigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
        else
        {
            IsOnGround = false;
        }

        if (!IsDashing)
        {
            if(buttonsHeld[InputsName.LEFT] && !movementLock)
                Rigidbody.velocity = new Vector2(-Time.deltaTime * speed, Rigidbody.velocity.y);
            if(buttonsHeld[InputsName.RIGHT] && !movementLock)
                Rigidbody.velocity = new Vector2(Time.deltaTime * speed, Rigidbody.velocity.y);

            if (buttonsReleased[InputsName.LEFT])
                Rigidbody.velocity = new Vector2(0, Rigidbody.velocity.y);
            if (buttonsReleased[InputsName.RIGHT])
                Rigidbody.velocity = new Vector2(0, Rigidbody.velocity.y);
        }
    }

    private void LateUpdate()
    {
        if (CurrentController.Capacity1Usable(this) && buttonsPressed[InputsName.SPECIAL_CAPACITY])
        {
            CurrentController.UseCapacity1(this);
        }
    }

    private void OnLightExpositionChanged(bool exposed)
    {
        if (exposed)
            OnLightEnter();
        else
        {
            OnLightExit();
        }
    }

    public void OnLightEnter()
    {
        if(numbOfLocks == 0)
        {
            williamController.gameObject.SetActive(true);
            reaperController.gameObject.SetActive(false);

            CurrentController = williamController;
        }
    }

    public void OnLightExit()
    {
        if(numbOfLocks == 0)
        {
            williamController.gameObject.SetActive(false);
            reaperController.gameObject.SetActive(true);

            CurrentController = reaperController;
        }
    }


    private bool IsPlayerDead()
    {
        if (NbPlayerLivesLeft <= 0)
        {
            return true;
        }

        return false;
    }

    public void LockTransformation()
    {
        numbOfLocks += 1;
    }

    public void UnlockTransformation()
    {
        if(numbOfLocks > 0)
            numbOfLocks -= 1;

        if(numbOfLocks == 0)
            OnLightExpositionChanged(lightSensor.InLight);
    }

    public IPlayerDataReadOnly Clone()
    {
        return this.MemberwiseClone() as IPlayerDataReadOnly;
    }

    private void GetInputs()
    {
        buttonsPressed.Clear();
        buttonsHeld.Clear();
        buttonsReleased.Clear();

        buttonsPressed[InputsName.JUMP] = Input.GetButtonDown(InputsName.JUMP);
        buttonsPressed[InputsName.SPECIAL_CAPACITY] = Input.GetButtonDown(InputsName.SPECIAL_CAPACITY);

        buttonsHeld[InputsName.UP] = Input.GetButton(InputsName.UP);
        buttonsHeld[InputsName.DOWN] = Input.GetButton(InputsName.DOWN);
        buttonsHeld[InputsName.RIGHT] = Input.GetButton(InputsName.RIGHT);
        buttonsHeld[InputsName.LEFT] = Input.GetButton(InputsName.LEFT);
        buttonsHeld[InputsName.ATTACK] = Input.GetButton(InputsName.ATTACK);

        buttonsReleased[InputsName.RIGHT] = Input.GetButtonUp(InputsName.RIGHT);
        buttonsReleased[InputsName.LEFT] = Input.GetButtonUp(InputsName.LEFT);
    }

    public void LockMovement(float time)
    {
        movementLock = true;
        StartCoroutine(UnlockMovement(time));
    }

    private IEnumerator UnlockMovement(float time)
    {
        yield return new WaitForSeconds(time);
        movementLock = false;
    }
}
