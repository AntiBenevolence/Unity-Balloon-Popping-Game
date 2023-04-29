using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Balloon_Movement : MonoBehaviour
{

    #region Player Unity Objects
    [SerializeField] Vector2 balloonScale;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject balloon;
    [SerializeField] AudioSource audioPop;
    [SerializeField] GameObject scorekeeper;
    [SerializeField] GameObject playerSprite;

    #endregion





    #region Movement Variables
    // Balloon X-Axis value
    [SerializeField] private float horizontalMovement;

    // Balloon Y-Axis value
    [SerializeField] private float verticalMovement;

    // Ballon horizontal speed
    [SerializeField] private float horizontalSpeed = 1.0f;

    // Ballon vertical speed
    [SerializeField] private float verticalSpeed = 1.0f;
    [SerializeField] private int speed = 2;

    # endregion
    

    #region Boolean Variables
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool directionDown = true;

    #endregion
    


    [SerializeField] int level;
    [SerializeField] Animator animator;

   
    #region Hardcoded boundaries for Camera in Game
    [SerializeField] float leftBound = -14.5f;
    [SerializeField] float rightBound = 14.5f;
    [SerializeField] float upBound = 5.0f;
    [SerializeField] float downBound = -5.0f;
    Vector3 desiredVelocity;
    Vector3 steeringVelocity;
    Vector3 currentVelocity;
    [SerializeField] float maxVelocity = 10.0f;
    [SerializeField] float maxForce = 5.0f;
    [SerializeField] int fleeDistance = 15;

    #endregion


     // Awake is called before the start of application
    void Awake()
    {
        //scale ballon size
        balloonScale = transform.localScale;

        //Level Setter 
        level = SceneManager.GetActiveScene().buildIndex;

        // Reference to player's rigidBody 2D
        rb = GetComponent<Rigidbody2D>();

        //Player object
        playerSprite = GameObject.FindGameObjectWithTag("Player");

        //Ballon object
        balloon = GameObject.FindGameObjectWithTag("Balloon");

        //Ballon pop audio
        audioPop = balloon.GetComponent<AudioSource>();

        //Scoreborad Object
        scorekeeper = GameObject.FindGameObjectWithTag("ScoreBoard");

        //Bolloon Object
        animator = GetComponent<Animator>();
       
    }
    

    // Start is called before the first frame update
    void Start()
    {
        speed = 4;

        if (level == 1)
        {
            InvokeRepeating("GrowBalloon", 1.0f, .1f);
        }
        if (level == 2)
        {
            InvokeRepeating("GrowBalloon", 2.0f, .25f);
        }

        if (level == 3)
        {
            InvokeRepeating("GrowBalloon", 3.0f, .25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        verticalMovement = horizontalSpeed;
        horizontalMovement = verticalSpeed;
        
        if (level == 1)
        {
            speed = 7;
        }
        else if (level == 2)
        {
            speed = 12;
        }
        else if (level == 3)
        {
            speed = 5;
        }

    }

    private void FixedUpdate()
    {

        if (level == 1)
        {
            EasyMovement(verticalMovement);
            CheckSize();
        }
        else if (level == 2)
        {
            EasyMovement(verticalMovement);
            mediumMovement();
            CheckSize();
        }
        else if (level == 3)
        {
            hardMovement();
            checkBoundsOnHard();
            CheckSize();
        }

    }

    void flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }


      void Movement()
    {
        if((transform.position.x <= leftBound && !isFacingRight) || (transform.position.x >= rightBound && isFacingRight))
        {
            horizontalSpeed = -horizontalSpeed;
            mediumMovement();
        }
    }



    void EasyMovement(float verticalDir)
    {
        rb.velocity = new Vector2(verticalDir * speed, rb.velocity.y);
        
       
        //Facing direction for booloon sprite
      
        if(verticalDir < 0 && isFacingRight)
        {
            flip();
        }
       
       
        else if (verticalDir > 0 && !isFacingRight)
        {
            flip();
        }
       
        Movement();
    }

    //This method will control balloon vertical movement. Once balloon reaches edge, it will move up/down 1.0 unit
    //The balloon will reverse 
    void mediumMovement()
    {
        if (transform.position.y + verticalSpeed > upBound && !directionDown)
        {
            transform.position = new Vector2(transform.position.x, upBound);
            directionDown = !directionDown;
        }
        else if (transform.position.y - verticalSpeed < downBound && directionDown)
        {
            transform.position = new Vector2(transform.position.x, downBound);
            directionDown = !directionDown;
        }
        else if (transform.position.y - verticalSpeed >= downBound && directionDown)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - verticalSpeed);
        }
        else if (transform.position.y + verticalSpeed <= upBound && !directionDown)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + verticalSpeed);
        }
    }

    //Fleeing Movement
    void hardMovement()
    {
        if (Vector3.Distance(transform.position, playerSprite.transform.position) < fleeDistance)
        {
            
            desiredVelocity = (transform.position - playerSprite.transform.position).normalized;
            desiredVelocity *= maxVelocity;

            currentVelocity = rb.velocity;
            steeringVelocity = (desiredVelocity - currentVelocity);
            steeringVelocity = Vector3.ClampMagnitude(steeringVelocity, maxForce);

            steeringVelocity /= rb.mass;

            currentVelocity += steeringVelocity;
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);

            transform.position += currentVelocity * Time.deltaTime;
            
        }
    }


    void CheckSize()
    {
        if (balloonScale.x >= 1.0f)
        {
            Destroy(gameObject);
            scorekeeper.GetComponent<Scorekeeper>().ZeroScore();
            SceneManager.LoadScene("Level " + level);
        }
    }

    public void GrowBalloon()
    {
        balloonScale.x += .01f;
        balloonScale.y += .01f;
        transform.localScale = balloonScale;
    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Fire")
        {
            CancelInvoke();
            AudioSource.PlayClipAtPoint(audioPop.clip, transform.position);
            StartCoroutine(DestroyBalloon());
        }
        
    }

    //Used to wait until fire animation for baloon is finished before destroying balloon 
    IEnumerator DestroyBalloon()
    {
        animator.SetBool("OnFire", true);
        yield return new WaitForSeconds(1.5f);
        //AudioSource.PlayClipAtPoint(audioPop.clip, transform.position);
        RecordScore();
        Destroy(gameObject);
        scorekeeper.GetComponent<Scorekeeper>().AdvanceLevel();
        if (level == 1 || level == 2)
        {
            SceneManager.LoadScene("Level " + (level + 1));
        }
        else if (level == 3)
        {
            SceneManager.LoadScene("HighScores");
        }
    }
    

    public void RecordScore()
    {
        int tempScore = (int)((balloonScale.x-.6f) * 500.0f);
        scorekeeper.GetComponent<Scorekeeper>().UpdateScore(tempScore);
    }


    

     void checkBoundsOnHard()
    {
        if (transform.position.x <= leftBound - 1.0f)
        {
            transform.position = new Vector2(rightBound + .2f, transform.position.y);
        }
        else if (transform.position.x >= rightBound + 1.0f)
        {
            transform.position = new Vector2(leftBound - .2f, transform.position.y);
        }
        else if (transform.position.y >= upBound + 1.0f)
        {
            transform.position = new Vector2(transform.position.x, downBound - .2f);
        }
        else if (transform.position.y <= downBound - 1.0f)
        {
            transform.position = new Vector2(transform.position.x, upBound + .2f);
        }
    }

}
