using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 2.5f;
    [SerializeField] float force = 7f;

    public int jumpCount = 1;
    public bool jumpPickup = false;
    public bool flip = false;
    public bool falling = false;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] float dashingPower = 5f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] TrailRenderer tr;


    private Rigidbody2D rb;
    private float movementHorizontal;

    private Animator animC;


    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animC = gameObject.GetComponent<Animator>();
        jumpCount = 1;
        jumpPickup = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (isDashing)
        {
            return;
        }

        movementHorizontal = Input.GetAxisRaw("Horizontal") * speed;

        animC.SetFloat("movement", Mathf.Abs(movementHorizontal));

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            Vector2 jump = new Vector2(movementHorizontal * speed, force);
            FindObjectOfType<AudioManager>().Play("Jump");
            rb.velocity = jump;
            jumpCount--;
            animC.SetBool("isJumping", true);
            animC.SetBool("isFalling", false);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if(movementHorizontal < 0)
        {
            flip = true;
            Flip();
        }
        else if(movementHorizontal > 0)
        {
            flip = false;
            Flip();
        }
        else
        {
            Flip();
        }


    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        Vector2 movement = new Vector2(movementHorizontal * speed, rb.velocity.y);
        rb.velocity = movement;
    }

    private void Flip()
    {
        if(flip == true)
        {
            transform.localScale = new Vector3(-4f, 4f, 0);
        }
        else
        {
            transform.localScale = new Vector3(4f, 4f, 0);
        }
    }

    public void HandleFall()
    {
        animC.SetBool("isJumping", false);
        animC.SetBool("isFalling", true);
        falling = true;
    }


    private void OnTriggerEnter2D(Collider2D collision) //Pickups
    {
        if (collision.gameObject.tag == "JumpPickup")
        {
            jumpCount = 2;
            FindObjectOfType<AudioManager>().Play("Pickup");
            jumpPickup = true;
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}
