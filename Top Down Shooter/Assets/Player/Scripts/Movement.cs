using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Animator playerAnim;
    public CharacterController playerCont;
    public Camera cam;

    public GameObject dashEffect;
    public float dashSpeed;
    public float dashTime;

    public int speed;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    
    public bool aim;
    public Vector2 mousePos;
    public Transform camPoint;
    public float distance;

    [SerializeField] private PlayerInput playerInput;
    private InputAction aimAction;
    public Shooting hasGun;
    private Vector3 inputVector;
    float gravity;

    int animMoveSpeed;
    int animMoveSpeedStrafe;
    float blendingMove;
    float animationVel;
    [SerializeField] float smoothAnimTime;

    Vector3 camForward;
    Vector3 move;
    Vector3 moveInput;
    float forwardAmount;
    float turnAmount;

    private void Awake()
    {
        aimAction = playerInput.actions["Aim"];
        animMoveSpeed = Animator.StringToHash("MoveSpeed");
        animMoveSpeedStrafe = Animator.StringToHash("MoveSpeedStrafe");
    }

    private void OnEnable()
    {
            aimAction.performed += _ => aim = true;
            aimAction.canceled += _ => aim = false;
    }

    private void OnDisable()
    {
            aimAction.performed -= _ => aim = true;
            aimAction.canceled -= _ => aim = false;    
    }

    private void Aiming()
    {
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(groundPlane.Raycast(ray, out rayLength))
        {
            Vector3 point = ray.GetPoint(rayLength);
            Debug.DrawLine(ray.origin, point, Color.red);
            transform.LookAt(new Vector3(point.x, transform.position.y, point.z));

            Vector3 offset = point - transform.position;
            camPoint.transform.position = transform.position + Vector3.ClampMagnitude(offset, distance);
        }

        camForward = Vector3.Scale(cam.transform.up, new Vector3(1, 0, 1)).normalized;
        move = inputVector.z * camForward + inputVector.x * cam.transform.right;
        if (move.magnitude > 1)
            move.Normalize();
        
        Move(move);

    }

    void Move(Vector3 move)
    {
        this.moveInput = move;

        ConvertMoveInput();
        UpdateAnimator();
    }

    void OnDash()
    {
        StartCoroutine(Dash());
    }
    
    IEnumerator Dash()
    {
        float starTime = Time.time + dashTime;

        while (Time.time <= starTime) 
        {
            playerCont.Move(inputVector * dashSpeed * Time.deltaTime);
            //dashEffect.SetActive(true);

            //if (Time.time >= starTime - 0.01f)
            //{
            //    dashEffect.SetActive(false);
            //    Debug.Log("asd");
            //    yield return null;
            //}

            yield return null;
        } 
    }

    void ConvertMoveInput()
    {
        Vector3 localMove = transform.InverseTransformDirection(moveInput);

        turnAmount = localMove.x;
        forwardAmount = localMove.z;
    }

    void UpdateAnimator()
    {
        playerAnim.SetBool("Aiming", true);
        playerAnim.SetFloat(animMoveSpeed, forwardAmount);
        playerAnim.SetFloat(animMoveSpeedStrafe, turnAmount);
    }

    private void OnMove(InputValue value)
    {
        Vector2 inputMovement = value.Get<Vector2>();
        inputVector = new Vector3(inputMovement.x, 0, inputMovement.y);
    }
    void Look() 
    {
        float lookAngle = Mathf.Atan2(inputVector.x, inputVector.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAngle, ref turnSmoothVelocity, turnSmoothTime);
        
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private void FixedUpdate()
    {
        gravity = -9.81f;
        if (playerCont.isGrounded)
            gravity = 0;
    }

    void walking()
    {
        camPoint.position = transform.position;

        Vector3 fixInput = new Vector3(inputVector.x, 0, inputVector.z);
        if (fixInput.magnitude >= 0.1f)
            Look();

        blendingMove = Mathf.SmoothDamp(blendingMove, fixInput.magnitude, ref animationVel, smoothAnimTime);
        playerAnim.SetBool("Aiming", false);
        playerAnim.SetFloat(animMoveSpeed, blendingMove);
    }

    void Update()
    {       
        inputVector.y = gravity;
        playerCont.Move(inputVector * Time.deltaTime * speed);

        if (hasGun.hasGun)
        {
            if (aim)
                Aiming();
            else if (aim == false)
            {
                walking();
            }
        }
        else
            walking();
        

    }
}
