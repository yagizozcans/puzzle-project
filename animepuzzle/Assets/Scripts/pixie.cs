using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pixie : MonoBehaviour
{
    public Vector3 goposition;

    public float rotateSpeed = 200f;
    public float speed = 5f;
    public float maxSpeed = 5f;

    private Rigidbody2D rb2d;

    public bool mouseTouched = false;

    [HideInInspector]public float mouseTouchedDelayTimer = 0;
    public float mouseTouchedDelayController;

    private float randomPositionTimer = 0;
    public float randomPositionTimerController;



    public float awayForce;

    public Vector2 awayForceLimit;

    float awayTimer;
    public float awayTimerController;

    private void Start()
    {
        speed = 0;
        rb2d = GetComponent<Rigidbody2D>();
        SetPoint();
        mouseTouchedDelayTimer = mouseTouchedDelayController;
    }
    private void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        float distance = Mathf.Clamp(Vector2.Distance(mousePosition, new Vector2(transform.position.x, transform.position.y))-2f,0.05f, Vector2.Distance(mousePosition, new Vector2(transform.position.x, transform.position.y)));

        awayTimer += Time.fixedDeltaTime;

        speed = Mathf.Clamp(speed + Time.fixedDeltaTime, 0, maxSpeed);

        if (1/distance > speed)
        {
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, 1 / distance);
        }
        else
        {
            rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, speed);
        }


        if (!mouseTouched)
        {
            mouseTouchedDelayTimer += Time.fixedDeltaTime;
            randomPositionTimer += Time.fixedDeltaTime;
            if(mouseTouchedDelayTimer >= mouseTouchedDelayController)
            {
                Vector2 direction = new Vector2(goposition.x, goposition.y) - rb2d.position;

                direction.Normalize();

                float rotateAmount = Vector3.Cross(direction, transform.up).z;

                rb2d.angularVelocity = -rotateAmount * rotateSpeed;

                rb2d.velocity = transform.up * speed;
            }
            if(randomPositionTimer >= randomPositionTimerController)
            {
                SetPoint();
                randomPositionTimer = 0;
            }
        }
    }

    void SetPoint()
    {
        Vector3 temporaryPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

        goposition = new Vector3(Random.Range(-temporaryPosition.x + 1, temporaryPosition.x - 1), Random.Range(-temporaryPosition.y + 1, temporaryPosition.y - 1), temporaryPosition.z);

        Debug.Log(goposition);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "mouse")
        {
            Vector2 direction = new Vector2(transform.position.x, transform.position.y) - new Vector2(collision.transform.position.x, collision.transform.position.y);

            direction.Normalize();
            float distance = Mathf.Clamp(Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x, transform.position.y)) - 2f, 0.05f, Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x, transform.position.y)));
            awayForce = 1 / distance;
            awayForce = Mathf.Clamp(awayForce, awayForceLimit.x, awayForceLimit.y);
            if (awayTimer > awayTimerController)
            {
                transform.GetComponent<Rigidbody2D>().AddForce(direction * awayForce, ForceMode2D.Impulse);
                awayTimer = 0;
            }

            mouseTouched = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "mouse")
        {
            speed = 0;
            mouseTouched = false;
            mouseTouchedDelayTimer = 0;
        }
    }
}
