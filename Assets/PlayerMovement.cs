using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//идеи
//Завтра мб сделать так чтобы прилиплялся.
//А ещё сделать collectables и вот к ним уже применить анимацию кароче

public class PlayerMovement : MonoBehaviour
{
    [Header("Reference")]

    public float moveSpeed = 5f;
    public float jumpForce = 50f;
    private Rigidbody rigidB;
    private bool isGrounded = true;
    private bool shouldJump = false;
    private Vector3 jumpDirection;


    private float scaleAmount = 25f;
    private float scaleMultiplier = 1f;

    public float scaleDuration = 0.5f;
    private Vector3 originalScale;
    private bool isScaling = false;

    public Transform orientation;
    public Transform player;
    public Transform playerObj;


    private float sphereRadius;



    // sticky related varibles
    private bool isSticky = false;
    // surface goo is attached at the moment
    private Transform stickedSurface; 
    private Rigidbody stickedSurfaceRigidbody;
   


    private void Start()
    {
        rigidB = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //camera direction vector
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; 
        cameraForward.Normalize();

        Vector3 movement = cameraForward * moveVertical + Camera.main.transform.right * moveHorizontal;
        movement.Normalize();

        rigidB.AddForce(movement * moveSpeed);

        //Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        //rigidB.AddForce(movement * moveSpeed);

       

    }

    private void OnCollisionEnter(Collision collision)
    {
        jumpDirection = collision.contacts[0].normal;
        if (!collision.gameObject.CompareTag("Player"))
        {
            isGrounded = true;
        }
    }

    private void Update()
    {
        Vector3 currentScale = transform.localScale;
        sphereRadius = currentScale.x;


        //Jump by pressing space while object colides with something
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            shouldJump = true;
            if (isSticky)
            {
                DetachFromSurface();
            }
        }
        // Scale up or down based on key input
        if (Input.GetKeyDown(KeyCode.L) && sphereRadius <= 250f)
        {
            StartCoroutine(ScaleCoroutine(transform.localScale + Vector3.one * scaleAmount)); // make Goo bigger 
        }
        else if (Input.GetKeyDown(KeyCode.K) && sphereRadius >= 25f)
        {
            StartCoroutine(ScaleCoroutine(transform.localScale - Vector3.one * scaleAmount)); // make Goo smaller
        }


        //while holding C
        if (Input.GetKey(KeyCode.C) && !isSticky)
        {
            AttachToSurface();
        }
        //not holding C
        else
        {
            if (isSticky)
            {
                DetachFromSurface();
            }
        }

    }

    private void LateUpdate()
    {
        // jumps in the end of the frame if colliding with something
        if (shouldJump && isGrounded)
        {
            rigidB.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            shouldJump = false;

        }
    }




    private IEnumerator ScaleCoroutine(Vector3 targetScale)
    {
        isScaling = true;

        Vector3 startScale = transform.localScale; 

        float elapsedTime = 0f;
        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / scaleDuration); 

            //interpolation with scale
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            yield return null;
        }


        //needed scale
        transform.localScale = targetScale; 
        isScaling = false;
    }

    /*
    private Vector3 ClampScale(Vector3 scale)
    {
        
        scale.x = Mathf.Clamp(scale.x, 25f, 250f);
        scale.y = Mathf.Clamp(scale.y, 25f, 250f);
        scale.z = Mathf.Clamp(scale.z, 25f, 250f);

        return scale;
    }
    */


    private void AttachToSurface()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            Collider hitCollider = hit.collider;
            if (hitCollider != null && isGrounded)
            {
                isSticky = true;
                stickedSurface = hitCollider.transform;

                /*
                //stick to it by becoming a child to that object
                transform.parent = stickedSurface;
                */


                
                //attach to coliders with joints
                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = stickedSurface.GetComponent<Rigidbody>();
                stickedSurfaceRigidbody = joint.connectedBody;
                
            }
        }
    }

    private void DetachFromSurface()
    {
        isSticky = false;
        stickedSurface = null;
        stickedSurfaceRigidbody = null;

        //remove joints to detach
        Destroy(GetComponent<FixedJoint>());
    }




}
