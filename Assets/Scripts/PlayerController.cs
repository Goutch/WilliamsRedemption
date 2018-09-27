using System.Collections;
using System.Collections.Generic;
using Light;
using UnityEngine;

public delegate void PlayerDeathEventHandler();


public class PlayerController : MonoBehaviour, IPlayerData {
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int nbPlayerLives;

    public int NbPlayerLives => nbPlayerLives;

    public event PlayerDeathEventHandler OnPlayerDie;

    private WilliamController williamController;
    private ReaperController reaperController;
    private EntityControlableController currentController;
    private LightSensor lightSensor;

    private Rigidbody2D rb;
    public Rigidbody2D Rigidbody { get { return rb; } }

    private float inputHorizontalMovement;
    private float inputVerticalMovement;
    private bool inputJump;
    private bool inputBasicAttack;

    private bool inputCapacity1;

    private int nbPlayerLivesLeft;


    private int numbOfLocks = 0;

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
            }
        }
    }

    public bool IsOnGround { get; set; }
    public bool IsDashing { get; set; }
    public FacingSideUpDown DirectionFacingUpDown { get; set; }

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

        inputVerticalMovement = Input.GetAxis("Vertical");

        inputCapacity1 = Input.GetButtonDown("Fire3");
        inputBasicAttack = Input.GetButtonDown("Fire1");


        if(inputVerticalMovement < 0)
        {
            currentController.animator.SetBool("IsLookingDown", true);
            DirectionFacingUpDown = FacingSideUpDown.Down;
        }
        else if(inputVerticalMovement > 0)
        {
            currentController.animator.SetBool("IsLookingUp", true);
            DirectionFacingUpDown = FacingSideUpDown.Up;
        }
        else
        {
            DirectionFacingUpDown = FacingSideUpDown.None;
        }

        currentController.animator.SetFloat("Speed", Mathf.Abs(inputHorizontalMovement));
        currentController.animator.SetBool("IsJumping", rb.velocity.y > 0);
        currentController.animator.SetBool("IsFalling", !IsOnGround && !IsDashing);
        currentController.animator.SetBool("IsDashing", IsDashing);
    }

    private void FixedUpdate()
    {
        if(currentController.CanUseBasicAttack(this) && inputBasicAttack)
        {
            currentController.UseBasicAttack(this);
            currentController.animator.SetBool("IsAttacking", true);
        }
        else
        {
            currentController.animator.SetBool("IsAttacking", false);
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

        if (!IsDashing)
        {
            if(Input.GetButton("Gauche"))
                rb.velocity = new Vector2(-Time.deltaTime * speed, rb.velocity.y);
            if(Input.GetButton("Droite"))
                rb.velocity = new Vector2(Time.deltaTime * speed, rb.velocity.y);

            if (Input.GetButtonUp("Gauche"))
                rb.velocity = new Vector2(0, rb.velocity.y);
            if (Input.GetButtonUp("Droite"))
                rb.velocity = new Vector2(0, rb.velocity.y);


            //rb.velocity = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * speed, rb.velocity.y);
        }
    }

    private void LateUpdate()
    {
        if (currentController.Capacity1Usable(this) && inputCapacity1)
        {
            currentController.UseCapacity1(this);
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

    public IPlayerDataReadOnly Clone()
    {
        return this.MemberwiseClone() as IPlayerDataReadOnly;
    }
}
