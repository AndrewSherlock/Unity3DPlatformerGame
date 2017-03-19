using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour {
    [HideInInspector]
    public Rigidbody rb;
    Collider col;

    Vector3 velocity;

    Vector3 targetPosition;

    float currentSpeed;
   

    bool isJumping = false;
    float startTime;
    float maxJumpTime;
    public float jumpForce;
    public float jumpAcceleration;
    public float jumpTime;

    bool hasDoubleJumped;
    bool hasReleasedJumpKey = false;

    bool climbing = false;
    public float climbSpeed;

    public GameObject playerObject;
    Vector3 lastMovement;
    public Vector3 lastRot;

    PlayerStatDetails playerStats;

    public bool canMoveAfterSpawn = false;

    void Start () {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!climbing && canMoveAfterSpawn)
        {
            float xInput = Input.GetAxisRaw("Horizontal");
            float ZInput = Input.GetAxisRaw("Vertical");

            if (xInput != 0 || ZInput != 0)
            {
                currentSpeed = PlayerSpeed(currentSpeed, 8f);
            }
            else
            {
                if (currentSpeed > 0)
                {
                    SlowPlayerDown(currentSpeed);
                }
                else
                {
                    currentSpeed = 0;
                }

            }

            Vector3 moveTarget = new Vector3(xInput * currentSpeed, rb.velocity.y, ZInput * currentSpeed);
            GetFacingDirection(new Vector3 (xInput, 0, ZInput), moveTarget);
            rb.velocity = moveTarget;
        }
    }

    private void FixedUpdate() // will miss input, move input to another method, maybe use booleans
    {
        if (!climbing)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                isJumping = true;
                startTime = Time.time;
                maxJumpTime = startTime + .4f;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }
            else if (Input.GetKey(KeyCode.Space) && (maxJumpTime > Time.time))
            {
                rb.AddForce(Vector3.up * jumpAcceleration, ForceMode.Acceleration);
            }

            if (Input.GetKeyUp(KeyCode.Space) && isJumping)
            {
                hasReleasedJumpKey = true;
            }

            if (!hasDoubleJumped && isJumping && Input.GetKeyDown(KeyCode.Space) && hasReleasedJumpKey)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                hasDoubleJumped = true;
            }
        }
    }

    public void StopVelocity()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    float SlowPlayerDown(float currentSpeed)
    {
        if(currentSpeed > 6f)
        {
            // TODO : add a slide slow down maybe
        }
        else
        {
            currentSpeed -= .6f;
        }
        return currentSpeed;
    }

    void GetFacingDirection(Vector3 movementInput, Vector3 movementAmount)
    { // works enough for now, snaps in certain directions 
        if (movementInput.x != 0 || movementInput.z != 0)
        {
            Vector3 lookDirection = new Vector3(movementAmount.x, 0, movementAmount.z);
            lastMovement = movementInput;
            playerObject.transform.rotation = Quaternion.LookRotation(lookDirection);
            lastRot = lookDirection;
        }
        else
        {
            playerObject.transform.rotation = Quaternion.LookRotation(lastRot);
        }
    }

    float PlayerSpeed(float currentSpeed, float maxSpeed)
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += 0.1f;
        }
        else {
            currentSpeed = maxSpeed;
        }
        return currentSpeed;
    }

    void ClimbObject(Collider ropeObject)
    {
        Vector3 ropeStartPoint = new Vector3(ropeObject.bounds.center.x, (ropeObject.bounds.center.y - ropeObject.bounds.size.y / 2), ropeObject.bounds.center.z - 0.05f);
        Vector3 ropeFinishPoint = new Vector3(ropeObject.bounds.center.x, ((ropeObject.bounds.center.y + ropeObject.bounds.size.y / 2) + (col.bounds.size.y/4)), ropeObject.bounds.center.z);

        if (Input.GetKeyDown(KeyCode.C) && !climbing)
        {
            climbing = true;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(ropeStartPoint.x, transform.position.y, ropeStartPoint.z), .2f);
        } else if(Input.GetKeyDown(KeyCode.C) && climbing)
        {
            climbing = false;
            rb.useGravity = true;
        }

        if (climbing)
        {
            rb.useGravity = false;
            float climbInput = Input.GetAxisRaw("Vertical");
            if (climbInput != 0)
            {
                if (transform.position.y < ropeFinishPoint.y)
                {
                    transform.position = transform.position + (Vector3.up * climbInput) * climbSpeed * Time.deltaTime;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            isJumping = false;
            hasDoubleJumped = false;
            hasReleasedJumpKey = false;
            canMoveAfterSpawn = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Climbable")
        {
            ClimbObject(other);
        }
    }
}
