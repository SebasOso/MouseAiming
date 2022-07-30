using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowboyMovement : MonoBehaviour
{

    public float walkSpeed = 2.5f;
    public float jumpHeight = 5f;
    public Transform targetTransform;
    public Transform groundCheckTransform;
    public float GroundRadious = 0.2f;
    public LayerMask mouseAimMask;
    public LayerMask groundMask;

    private Camera mainCamera;
    private float inputMovement;
    private Animator animator;
    private Rigidbody rb;
    private bool isGrounded;
    


    // Aim
    private int FacingSign
    {
        get
        {
            Vector3 perp = Vector3.Cross(transform.forward, targetTransform.forward);
            float dir =Vector3.Dot(perp, targetTransform.up);
            return dir > 0f ? -1 : dir < 0 ? 1 : 0;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

        // Evaluating Variables
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        inputMovement = Input.GetAxis("Horizontal");

        // MouseAim
        Ray ray =mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            targetTransform.position = hit.point;
        }

        // Jump
        if (Input.GetButton("Jump")&& isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, 0);
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -1 * Physics.gravity.y), ForceMode.VelocityChange);
        }

    }
    private void FixedUpdate()
    {
        //Movement
        rb.velocity = new Vector3 (inputMovement * walkSpeed, rb.velocity.y, 0);
        animator.SetFloat("speed", (FacingSign * rb.velocity.x) / walkSpeed);

        //Facing
        rb.MoveRotation(Quaternion.Euler(new Vector3(0, 90 * Mathf.Sign(targetTransform.position.x - transform.position.x), 0)));

        //Ground check
        isGrounded = Physics.CheckSphere(groundCheckTransform.position, GroundRadious, groundMask, QueryTriggerInteraction.Ignore);
        animator.SetBool("isGrounded", isGrounded);
    }
}
