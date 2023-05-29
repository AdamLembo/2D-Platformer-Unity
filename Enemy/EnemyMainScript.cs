using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;

public class EnemyMainScript : MonoBehaviour
{
    public float speed;

    public bool flip = false;
    public bool facingRight;

    public Transform startPos;

    private bool isRight = false;
    private bool isLeft = false;
    [SerializeField] bool isPatrolling = true;
    [SerializeField] float offSetLeft = 0;
    [SerializeField] float offSetRight = 0;

    [SerializeField] float alertRadius;
    [SerializeField] float attackRadius;
    private float stoppingDistance = 1.5f;
    [SerializeField] float attackCooldown;
    [SerializeField] float startAttackCooldown;

    Rigidbody2D rb;
    Transform player;
    Animator animC;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animC = gameObject.GetComponent<Animator>();
        attackCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPatrolling == true)
        {
            if (!isRight)
            {
                if(transform.position.x < startPos.position.x + offSetRight)
                {
                    Move(offSetRight);
                }
                else if(transform.position.x >= startPos.position.x + offSetRight)
                {
                    isRight = true;
                    isLeft = false;
                    if (facingRight)
                    {
                        Flip();
                    }
                }
            }
            else if (!isLeft)
            {
                if(transform.position.x > startPos.position.x + offSetLeft)
                {
                    Move(offSetLeft);
                }
                else if(transform.position.x <= startPos.position.x + offSetLeft)
                {
                    isRight = false;
                    isLeft = true;
                    if (!facingRight)
                    {
                        Flip();
                    }
                }
            }          
        }
        else
        {
            if (player.position.x > transform.position.x && !facingRight)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && facingRight)
            {
                Flip();
            }
        }

        if(Vector2.Distance(transform.position, player.position) <= alertRadius && (Vector2.Distance(transform.position, player.position) > stoppingDistance))
        {
            isPatrolling = false;
            Vector2 target = new Vector2(player.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else if((Vector2.Distance(transform.position, player.position) > alertRadius && isPatrolling == false))
        {
            isPatrolling = true;
            startPos.position = transform.position;
            offSetLeft = -3;
            offSetRight = 3;
        }

        if(Vector2.Distance(player.position, rb.position) <= attackRadius)
        {
            if (attackCooldown <= 0)
            {
                animC.SetBool("isAttacking", true);
                attackCooldown = startAttackCooldown;
            }
            else if(attackCooldown > 0)
            {
                animC.SetBool("isAttacking", false);
                animC.SetTrigger("Idle");
                attackCooldown -= Time.deltaTime;
            }
        }
        else
        {
            animC.SetTrigger("Walking");
            animC.SetBool("isAttacking", false);
        }
      






    }

    public void Attack()
    {
        FindObjectOfType<AudioManager>().Play("Attack");
        PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        playerHealth.HitPlayer(25);
    }

    private void Move(float offset)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(startPos.position.x + offset, transform.position.y, transform.position.z), speed * Time.deltaTime);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, alertRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if(isRight && offSetLeft <= 0)
            {
                offSetLeft++;
                Move(offSetRight);
                speed = 3f;
            }
        }
        if(!isRight && offSetRight >= 0)
        {
            offSetRight--;
            Move(offSetLeft);
            speed = 3f;
        }
        else
        {

        }
    }
}
