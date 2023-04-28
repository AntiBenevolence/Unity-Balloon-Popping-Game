using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinMovement : MonoBehaviour
{
    [SerializeField] float movement;
    [SerializeField] Rigidbody2D fireball;
    [SerializeField] int speed;
    [SerializeField] GameObject controller;
    [SerializeField] GameObject player;
    [SerializeField] GameObject balloon;
    [SerializeField] bool isFacingRight;
    [SerializeField] bool isFlipped;


    // Start is called before the first frame update
    void Start()
    {
        if (fireball == null)
            fireball = GetComponent<Rigidbody2D>();
        if (controller == null)
            controller = GameObject.FindGameObjectWithTag("Fire");
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        movement = 1.0f;
        speed = 10;
        isFlipped = false;

        isFacingRight = player.GetComponent<Player_Movement>().getDirection();
        if (!isFacingRight && !isFlipped)
        {
            Flip();
            isFlipped = true;
            movement = -movement;
        }
        else if (isFacingRight && isFlipped)
        {
            Flip();
            isFlipped = false;
            movement = -movement;
        }

        fireball.velocity = new Vector2(movement * speed, fireball.velocity.y);
    }

    // Update is called once per frame
        void Update()
    {


    }

    //called potentially multiple times per frame
    //used for physics & movement
    void FixedUpdate()
    {
        if (fireball.transform.position.x >= 25.0f || fireball.transform.position.x <= -25.0f)
            Destroy(gameObject);
    }


    void Flip()
    {
        transform.Rotate(0, 180, 0);
        transform.Rotate(180, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //controller.GetComponent<Scorekeeper>().UpdateScore();
        Destroy(gameObject);
    }
}
