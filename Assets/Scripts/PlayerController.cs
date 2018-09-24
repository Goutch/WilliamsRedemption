using System.Collections;
using System.Collections.Generic;
using Light;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private WilliamController williamController;
    private ReaperController reaperController;
    private EntityControlableController currentController;
    private IPlayerData data = new PlayerData();

    private float inputHorizontalMovement;
    private bool inputJump;

    private bool inputDash;

    private void Awake()
    {
        data.RigidBody = GetComponent<Rigidbody2D>();

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

        inputDash = Input.GetButtonDown("Fire3");

        currentController.animator.SetFloat("Speed", Mathf.Abs(inputHorizontalMovement));
        currentController.animator.SetBool("IsJumping", inputJump && data.IsOnGround);
        currentController.animator.SetBool("IsFalling", !data.IsOnGround && !data.IsDashing);
        currentController.animator.SetBool("IsDashing", data.IsDashing);
    }

    private void FixedUpdate()
    {
        transform.Translate(inputHorizontalMovement * Time.deltaTime, 0, 0);

        if (currentController.Capacity1Usable(data) && inputDash)
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
}
