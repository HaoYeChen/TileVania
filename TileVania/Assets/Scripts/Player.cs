using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // movement & jump & death
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpForce = 30f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] AudioClip deathSFX;

    // groundcheck
    private float moveInput;
    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius = 0.5f;
    public LayerMask whatisGround;

    // extrajumps
    private int extraJumps = 1;
    public int extraJumpsValue;

    // Arrow attack
    public GameObject arrowLeft, arrowRight;
    Vector2 arrowPos;
    public float fireRate = 0.5f;
    float nextFire = 0.0f;
    private bool attacking = false;

    //stats
    public int curHealth;
    public int maxHealth = 100;

    //Blood particle effect
    public GameObject blood;

    // State
    bool isAlive = true;
    private bool facingRight = true;

    //test
    private GameSession DeathManager;



    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;

    // Start is called before the first frame update
    // Message then methods
    void Start()
    {
        extraJumps = extraJumpsValue;
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;

        curHealth = maxHealth;
    }

    private void Awake()
    {
        DeathManager = GameObject.FindObjectOfType<GameSession>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }

        Run();
        ClimbLadder();
        Die();
        

        //Jump
        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }

        if (CrossPlatformInputManager.GetButtonDown("Jump") && extraJumps > 0)
        {
            myRigidBody.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (CrossPlatformInputManager.GetButtonDown("Jump") && extraJumps == 0 && isGrounded == true)
        {
            myRigidBody.velocity = Vector2.up * jumpForce;
        }

        //Firing arrows
        if (CrossPlatformInputManager.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            attacking = true;
            myAnimator.SetBool("Fire", true);
            fire();
            
        }
        if (CrossPlatformInputManager.GetButtonUp("Fire1"))
        {
            myAnimator.SetBool("Fire", false);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatisGround);

        moveInput = Input.GetAxis("Horizontal");
        myRigidBody.velocity = new Vector2(moveInput * runSpeed, myRigidBody.velocity.y);

        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    private void Run()
    {
        float controlThrow = CrossPlatformInputManager.GetAxis("Horizontal"); // value is between -1 to +1

        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed);
    }

    private void fire()
    {
        arrowPos = transform.position;
        if (facingRight)
        {
            attacking = true;
            arrowPos += new Vector2(+1f, -0.20f);       
            Instantiate(arrowRight, arrowPos, Quaternion.identity);
            
        }
        else 
        {
            attacking = true;
            arrowPos += new Vector2(-1f, -0.20f);
            Instantiate(arrowLeft, arrowPos, Quaternion.identity);
            
        }
    }

    private void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }

        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
    }

    //private void Jump()
    //{

    //    if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
    //    {
    //        return;
    //    }

    //    if (CrossPlatformInputManager.GetButtonDown("Jump"))
    //    {
    //        Vector2 jumpVelocityToAdd = new Vector2(0f, jumpForce); //(x,y)
    //        myRigidBody.velocity += jumpVelocityToAdd;
    //    }
    //}
    

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards","Bullet")))
        {
            isAlive = false;
            Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gameObject);

            myAnimator.SetTrigger("Dying");
            myBodyCollider.sharedMaterial = null;
            GetComponent<Rigidbody2D>().velocity = deathKick;
            myRigidBody.freezeRotation = false;
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Equals("FallingSpikes"))
        {
            isAlive = false;
            Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gameObject);

            myAnimator.SetTrigger("Dying");
            myBodyCollider.sharedMaterial = null;
            GetComponent<Rigidbody2D>().velocity = deathKick;
            myRigidBody.freezeRotation = false;
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        if (collision.gameObject.name.Equals("ReverseGravityFallingSpikes"))
        {
            
            isAlive = false;
            Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gameObject);

            myAnimator.SetTrigger("Dying");
            myBodyCollider.sharedMaterial = null;
            GetComponent<Rigidbody2D>().velocity = deathKick;
            myRigidBody.freezeRotation = false;
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {

            isAlive = false;
            Instantiate(blood, transform.position, Quaternion.identity);
            Destroy(gameObject);

            myAnimator.SetTrigger("Dying");
            myBodyCollider.sharedMaterial = null;
            GetComponent<Rigidbody2D>().velocity = deathKick;
            myRigidBody.freezeRotation = false;
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    private void Flip()
    {
        // Det sammen måden

        // if the player is moving horizontally
        //bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        //if (playerHasHorizontalSpeed)
        //{
            // reverse the current scaling of x axis
        //    transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        //}

        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    
}
