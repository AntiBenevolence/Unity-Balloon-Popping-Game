using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    
    #region Player Unity Objects

    // Initialize RigidBody 2D Object
    [SerializeField] private Rigidbody2D rb;
   
    #endregion



    #region Movement Variables 

    // Player's X-Axis value
    [SerializeField] private float horizontalMovement;

    // Player's Y-Axis value
    [SerializeField] private float verticalMovement;

    // Horizontal/Vertical movement speed of player
    [SerializeField] private int speed;
    
    #endregion

   
    #region Boolean Variables

    //Determine If Player Is Facing Right 
    [SerializeField] private bool isFacingRight = true;

    #endregion


    // Awake is called before the start of application
    void Awake()
    {
       
        // Reference to player's rigidBody 2D
        rb = GetComponent<Rigidbody2D>();
        
        // Set Horizontal/Vertical movement speed of player
        speed = 10;
       
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {

        // Store value given by the key presses of button associated horinzontal movement
        horizontalMovement = Input.GetAxis("Horizontal");
        
        // Store value given by the key presses of button associated vertical movement
        verticalMovement = Input.GetAxis("Vertical");
    }


    //called potentially multiple times per frame
    void FixedUpdate()
    {

    move(horizontalMovement,verticalMovement);
       
    }


    



    //to be used by PinMovement script
    public bool getDirection() {
        if (isFacingRight)
            return true;
        return false;
    }

   

  

    /* 
    Method Name: move()
    Parameter: 
    float horinzontalDir - Value associated with direction of player in regards to X-Axis 
    float vericalDir - Value associated with direction of player in regards to Y-Axis 
    Description: Manipulates Movement For Player
    */  
    public void move(float horinzontalDir, float verticalDir)
    {

        rb.velocity = new Vector2(horinzontalDir * speed, verticalDir * speed);

        //Facing direction for player sprite
        // If looking right and clicked right (flip to the left)
        if(horinzontalDir < 0 && isFacingRight)
        {
            flip();
        }
       
        // If looking left and clicked left (flip to the right)
        else if (horinzontalDir > 0 && !isFacingRight)
        {
            flip();
        }
    }


    /* 
    Method Name: flip()
    Description: Changes the value of (boolean) "isFacingRight" from true/false 
    depending where player sprite if facing and rotates the player sprite to face 
    left or right
    */  
    void flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    
    
    #region Setters 

    public float GetHorizontalMovement()
    {
        return horizontalMovement;
    }

    public float GetVerticalMovement()
    {
        return verticalMovement;
    }

    #endregion

}