using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Balloon_Movement : MonoBehaviour
{
    
    [SerializeField] float movementLR;
    [SerializeField] float movementUD;
    [SerializeField] float moveFactorLR = 1.0f;
    [SerializeField] float moveFactorUD = 1.0f;
    [SerializeField] int speed = 2;
    [SerializeField] bool isFacingRight = true;
    [SerializeField] bool directionDown = true;
    [SerializeField] int level;
    [SerializeField] Animator animator;


    [SerializeField] Vector2 theScale;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] GameObject balloon;
    [SerializeField] AudioSource audioPop;
    [SerializeField] GameObject scorekeeper;
    [SerializeField] GameObject player;

    //Hardcoded boundaries for Camera in Game
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

    // Start is called before the first frame update
    void Start()
    {
        theScale = transform.localScale;
        level = SceneManager.GetActiveScene().buildIndex;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
        if (balloon == null)
            balloon = GameObject.FindGameObjectWithTag("Balloon");
        if (audioPop == null)
            audioPop = balloon.GetComponent<AudioSource>();
        speed = 4;
        if (scorekeeper == null)
            scorekeeper = GameObject.FindGameObjectWithTag("ScoreBoard");
        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
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
        movementLR = moveFactorLR;
        movementUD = moveFactorUD;
        if (level == 1)
        {
            speed = 7;
        }
        else if (level == 2)
        {
            speed = 12;
        }
        else if (level  == 3)
        {
            speed = 5;
        }

    }

    private void FixedUpdate()
    {

        if (level == 1)
        {
            EasyMovement();
            CheckSize();
        }
        else if (level == 2)
        {
            EasyMovement();
            VerticalMovement();
            CheckSize();
        }
        else if (level == 3)
        {
            FleeingMovement();
            checkBoundsOnHard();
            CheckSize();
        }

    }

    void EasyMovement()
    {
        rigid.velocity = new Vector2(movementLR * speed, rigid.velocity.y);
        if (movementLR < 0 && isFacingRight || movementLR > 0 && !isFacingRight)
            Flip();
        Movement();

    }

    void CheckSize()
    {
        if (theScale.x >= 1.0f)
        {
            Destroy(gameObject);
            scorekeeper.GetComponent<Scorekeeper>().ZeroScore();
            SceneManager.LoadScene("Level " + level);

        }
    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
        isFacingRight = !isFacingRight;
    }
    void Movement()
    {
        if((transform.position.x <= leftBound && !isFacingRight) || (transform.position.x >= rightBound && isFacingRight))
        {
            moveFactorLR = -moveFactorLR;
            VerticalMovement();
        }
        
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

    void FleeingMovement()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < fleeDistance)
        {
            
            desiredVelocity = (transform.position - player.transform.position).normalized;
            desiredVelocity *= maxVelocity;

            currentVelocity = rigid.velocity;
            steeringVelocity = (desiredVelocity - currentVelocity);
            steeringVelocity = Vector3.ClampMagnitude(steeringVelocity, maxForce);

            steeringVelocity /= rigid.mass;

            currentVelocity += steeringVelocity;
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxVelocity);

            transform.position += currentVelocity * Time.deltaTime;
            
        }
    }

    //This method will control balloon vertical movement. Once balloon reaches edge, it will move up/down 1.0 unit
    //The balloon will reverse 
    void VerticalMovement()
    {
        if (transform.position.y + moveFactorUD > upBound && !directionDown)
        {
            transform.position = new Vector2(transform.position.x, upBound);
            directionDown = !directionDown;
        }
        else if (transform.position.y - moveFactorUD < downBound && directionDown)
        {
            transform.position = new Vector2(transform.position.x, downBound);
            directionDown = !directionDown;
        }
        else if (transform.position.y - moveFactorUD >= downBound && directionDown)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - moveFactorUD);
        }
        else if (transform.position.y + moveFactorUD <= upBound && !directionDown)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + moveFactorUD);
        }

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
        int tempScore = (int)((theScale.x-.6f) * 500.0f);
        scorekeeper.GetComponent<Scorekeeper>().UpdateScore(tempScore);
    }


    public void GrowBalloon()
    {
        theScale.x += .01f;
        theScale.y += .01f;
        transform.localScale = theScale;
    }

}
