using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerData {
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private WilliamController williamController;
    private ReaperController reaperController;
    private EntityControlableController currentController;

    private float inputHorizontalMovement;
    private float inputVerticalMovement;
    private bool inputJump;
    private bool inputBasicAttack;

    private bool inputDash;

    public bool IsOnGround { get; set; }
    public bool IsDashing { get ; set; }
    public Rigidbody2D RigidBody { get ; set ; }

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();

        williamController = GetComponentInChildren<WilliamController>();
        reaperController = GetComponentInChildren<ReaperController>();

        OnLightEnter();
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

        inputDash = Input.GetButtonDown("Fire3");
        inputBasicAttack = Input.GetButtonDown("Fire1");

        currentController.animator.SetFloat("Speed", Mathf.Abs(inputHorizontalMovement));
        currentController.animator.SetBool("IsJumping", RigidBody.velocity.y > 0);
        currentController.animator.SetBool("IsFalling", !IsOnGround && !IsDashing);
        currentController.animator.SetBool("IsDashing", IsDashing);
        currentController.animator.SetBool("IsLookingDown", inputVerticalMovement < 0);
        Debug.Log(inputVerticalMovement);
    }

    private void FixedUpdate()
    {
        transform.Translate(inputHorizontalMovement * Time.deltaTime, 0, 0);

        if(currentController.CanUseBasicAttack(this) && inputBasicAttack)
        {
            currentController.UseBasicAttack(this);
            currentController.animator.SetBool("IsShooting", true);
        }
        else
        {
            currentController.animator.SetBool("IsShooting", false);
        }

        if (currentController.Capacity1Usable(this) && inputDash)
        {
            currentController.UseCapacity1(this);
        }

        if (RigidBody.velocity.y == 0 && !IsDashing)
        {
            IsOnGround = true;

            if (inputJump)
            {
                RigidBody.velocity = new Vector2(0, 0);
                RigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

                IsOnGround = false;
            }
        }
        else
        {
            IsOnGround = false;
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

    public IPlayerDataReadOnly Clone()
    {
        return this.MemberwiseClone() as IPlayerDataReadOnly;
    }
}
