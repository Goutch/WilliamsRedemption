using System.Collections;
using System.Collections.Generic;
using Light;
using UnityEngine;

public delegate void PlayerDeathEventHandler();

public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int nbPlayerLives;

    public event PlayerDeathEventHandler OnPlayerDie;
    private WilliamController williamController;
    private ReaperController reaperController;
    private EntityControlableController currentController;
    private IPlayerData data = new PlayerData();
    
    private float inputHorizontalMovement;
    private bool inputJump;

    private bool inputUseCapacity1;
    
    private int nbPlayerLivesLeft;
    
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

    private void Awake()
    {
        data.RigidBody = GetComponent<Rigidbody2D>();
        nbPlayerLivesLeft = nbPlayerLives;
        williamController = GetComponentInChildren<WilliamController>();
        reaperController = GetComponentInChildren<ReaperController>();

        GetComponent<LightSensor>().OnLightExpositionChange+=OnLightExpositionChanged;
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
        currentController.animator.SetBool("IsJumping", inputJump && data.IsOnGround);
        currentController.animator.SetBool("IsFalling", !data.IsOnGround && !data.IsDashing);

        if(currentController is WilliamController)
            currentController.animator.SetBool("IsDashing", data.IsDashing);
    }

    private void FixedUpdate()
    {
        transform.Translate(inputHorizontalMovement * Time.deltaTime, 0, 0);

        if (currentController.Capacity1Usable(data) && inputUseCapacity1)
        {
            currentController.UseCapacity1(data);
        }

        if (data.RigidBody.velocity.y == 0 && !data.IsDashing)
        {
            data.IsOnGround = true;

            if (inputJump)
            {
                data.RigidBody.velocity = new Vector2(0, 0);
                data.RigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

                data.IsOnGround = false;
            }
        }
        else
        {
            data.IsOnGround = false;
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
        williamController.gameObject.SetActive(true);
        reaperController.gameObject.SetActive(false);

        currentController = williamController;
    }

    public void OnLightExit()
    {
        williamController.gameObject.SetActive(false);
        reaperController.gameObject.SetActive(true);

        currentController = reaperController;
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
}
