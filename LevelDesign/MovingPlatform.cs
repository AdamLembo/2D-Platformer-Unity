using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    Vector2 startPos = Vector2.zero;
    [SerializeField] float offSetRight = 0;
    [SerializeField] float offSetLeft = 0;
    [SerializeField] bool isRight = false;
    [SerializeField] bool isLeft = false;
    [SerializeField] float speed = 2;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRight)
        {
            if(transform.position.x < startPos.x + offSetRight)
            {
                Move(offSetRight);
            }
            else if(transform.position.x >= startPos.x + offSetRight)
            {
                isRight = true;
                isLeft = false;
            }
        }

        if (!isLeft)
        {
            if (transform.position.x > startPos.x + offSetLeft)
            {
                Move(offSetLeft);
            }
            else if (transform.position.x <= startPos.x + offSetLeft)
            {
                isRight = false;
                isLeft = true;
            }
        }
    }

    private void Move(float offset)
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(startPos.x + offset, transform.position.y), speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
