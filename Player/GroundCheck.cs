using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    public Player pmove;
    private Animator animC;

    void Start()
    {
        animC = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            if (pmove.jumpPickup)
            {
                pmove.jumpCount = 2;
            }
            if (!pmove.jumpPickup)
            {
                pmove.jumpCount = 1;
            }

            pmove.falling = false;
            animC.SetBool("isFalling", false);
            animC.SetBool("isJumping", false);

        }
    }
}
