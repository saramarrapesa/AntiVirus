using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchMovement : MonoBehaviour
{
    private Rigidbody2D myBody;

    public float speed;
    private bool moveUp;
    private bool dontMove;

    private void Start()
    {
        myBody = GetComponent<Rigidbody2D>();

        dontMove = true;
    }

    private void Update()
    {
        HandleMoving();
    }

    void HandleMoving()
    {
        if (dontMove)
        {
            StopMoving();
        }
        else
        {
            if (moveUp)
            {
                MoveUp();
            }
            else if (!moveUp)
            {
                MoveDown();
            }
        }
    }
    public void AllowMovement(bool movement)
    {
        dontMove = false;
        moveUp = movement;
    }

    public void DontAllowMovement()
    {
        dontMove = true;
    }

    public void MoveUp()
    {
        myBody.velocity = new Vector2(myBody.velocity.x, speed);
    }

    public void MoveDown()
    {
        myBody.velocity = new Vector2(myBody.velocity.x, -speed);
    }

    public void StopMoving()
    {
        myBody.velocity = new Vector2(myBody.velocity.x, 0f);
    }

    void DetectInput()
    {
        float y = Input.GetAxisRaw("Vertical");
        if (y > 0)
        {
            MoveUp();
        }
        else if(y < 0)
        {
            MoveDown();
        }
        else
        {
            StopMoving();
        }
    }
}
