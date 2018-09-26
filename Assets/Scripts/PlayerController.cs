using System.Collections;
using System.Collections.Generic;
using Light;
using UnityEngine;

public delegate void PlayerDeathEventHandler();

public class PlayerController : MonoBehaviour, IPlayerData {
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int nbPlayerLives;

    public event PlayerDeathEventHandler OnPlayerDie;

    private WilliamController williamController;
    private ReaperController reaperController;
    private EntityControlableController currentController;
    private LightSensor lightSensor;

    private Rigidbody2D rb;
    public Rigidbody2D Rigidbody { get { return rb; } }

    private float inputHorizontalMovement;
    private bool inputJump;

    private bool inputUseCapacity1;
    
    private int nbPlayerLivesLeft;

    private int numbOfLocks = 0;

    public int NbPlayerLivesLeft
    {
        get { return nbPlayerLivesLeft; }
        set
        {
            nbPlayerLivesLeft = value;
            if (IsPlayerDead())
            {
                OnPlayerDie?.Invoke();
            }
        }
    }

    public bool IsOnGround { get; set; }

    public bool IsDashing { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        nbPlayerLivesLeft = nbPlayerLives;
        williamController = GetComponentInChildren<WilliamController>();
        reaperController = GetComponentInChildren<ReaperController>();

        lightSensor = GetComponent<LightSensor>();
        lightSensor.OnLightExpositionChange += OnLightExpositionChanged;

        OnLightExpositionChanged(true);    
    }
    private void Update()
    {
        inputJump = Input.GetButtonDown("Jump");

        inputHorizontalMovement = Input.GetAxis("Horizontal") * speed;
        if (inputHorizontalMovement > 0)
            currentController.sprite.flipX = false;
        else if (inputHorizontalMovement < 0)
            currentController.sprite.flipX = true;

        inputUseCapacity1 = Input.GetButtonDown("Fire3");

        currentController.animator.SetFloat("Speed", Mathf.Abs(inputHorizontalMovement));
        currentController.animator.SetBool("IsJumping", inputJump && IsOnGround);
        currentController.animator.SetBool("IsFalling", !IsOnGround && !IsDashing);

        if(currentController is WilliamController)
            currentController.animator.SetBool("IsDashing", IsDashing);
    }

    private void FixedUpdate()
    {
        transform.Translate(inputHorizontalMovement * Time.deltaTime, 0, 0);

        if (currentController.Capacity1Usable(this) && inputUseCapacity1)
        {
            currentController.UseCapacity1(this);
        }

        if (rb.velocity.y == 0 && !IsDashing)
        {
            IsOnGround = true;

            if (inputJump)
            {
                rb.velocity = new Vector2(0, 0);
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

                IsOnGround = false;
            }
        }
        else
        {
            IsOnGround = false;
        }
    }

   private void OnLightExpositionChanged(bool exposed)
    {
        if(exposed)
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

            currentController = williamController;
        }
    }

    public void OnLightExit()
    {
        if(numbOfLocks == 0)
        {
            williamController.gameObject.SetActive(false);
            reaperController.gameObject.SetActive(true);

            currentController = reaperController;
        }
    }

    public void DamagePlayer()
    {
        NbPlayerLivesLeft--;
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
}
